using Fido2NetLib;
using Fido2NetLib.Objects;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Security.FIDO2.Service;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.DTO.Leanote.Auth;
using MoreNote.Models.Entity.Leanote.Loggin;
using MoreNote.Models.Model.FIDO2;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Auth/[action]")]
    public class AuthAPIController : APIBaseController
    {
        private AuthService authService;
       
        public AuthAPIController(AttachService attachService,
             TokenSerivce tokenSerivce,
             NoteFileService noteFileService,
             UserService userService,
             ConfigFileService configFileService,
             IHttpContextAccessor accessor,
            
             AuthService authService
            ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.authService = authService;
        }
        /// <summary>
        /// 取号(用于客户端请求序列号)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult TakeNumber()
        {
            var re=new ApiRe();
            //产生一个序号
            var id= idGenerator.NextId();//序号
            var random = RandomTool.CreatSafeRandomBase64(16);
            var data = SHAEncryptHelper.Hash256Encrypt(id+random);

            distributedCache.SetBool("TakeNumber", true);
            re.Data= data;
            re.Ok=true;
            return LeanoteJson(re);
        }

        /// <summary>
        /// 登陆
        ///  成功返回 {Ok: true, Item: token }
        ///  失败返回 {Ok: false, Msg: ""}
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
       [HttpPost]
        public async Task<IActionResult> Login(string email, string pwd, string requestNumber)
        {
            string tokenValue = "";

            var re = new ApiRe();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in Request.Headers)
            {
                stringBuilder.Append(item.Key + "=" + item.Value.ToString() + "\r\n");

            }

            LoggingLogin logg = new LoggingLogin()
            {
                Id = this.idGenerator.NextId(),
                LoginDateTime = DateTime.Now,
                LoginMethod = "PassWord",
                Ip = Request.Host.Host,
                BrowserRequestHeader = stringBuilder.ToString(),
            };

            try
            {
                var tokenStr = await authService.LoginByPWD(email, pwd);
                if (!string.IsNullOrEmpty(tokenStr))
                {
                    var user = userService.GetUserByEmail(email);
                    if (this.config.SecurityConfig.LogNeedHmac)
                    {
                        user.VerifyHmac(this.cryptographyProvider);
                        if (!user.Verify)
                        {
                            re.Msg = "VerifyHmac is Error";
                            return LeanoteJson(re);
                        }
                    }
                    var userToken = new UserToken()
                    {
                        Token = tokenStr,
                        UserId = user.Id,
                        Email = user.Email,
                        Username = user.Username
                    };
                    re.Ok = true;
                    //re.Data = userToken;
                    this.distributedCache.SetBool("Password" + requestNumber, true);
                    logg.UserId = user.Id;
                    logg.IsLoginSuccess = true;
                    return LeanoteJson(re);
                }
                else
                {
                    re.Msg = "用户名或密码有误";

                    logg.ErrorMessage = "用户名或密码有误";
                    return LeanoteJson(re);
                }
            }
            catch (Exception ex)
            {
                re.Msg = ex.Message;
                re.Ok = false;
                return LeanoteJson(re);
            }
            finally
            {

                await logg.AddMac(this.cryptographyProvider);
                this.logging.Save(logg);
            }
        }
        [HttpGet, HttpPost]
        public IActionResult TakeToken(string email, string requestNumber)
        {
          
            var re=new ApiRe();
            var user=userService.GetUserByEmail(email);
            if (user==null)
            {
                return LeanoteJson(re);
            }
            var Passwrod_Check = this.distributedCache.GetBool("Password" + requestNumber,false);
            var USBKEY_CHECK = this.distributedCache.GetBool("USBKEY" + requestNumber,false);

            var result=false;

            if (user.LoginSecurityPolicyLevel==LoginSecurityPolicyLevel.compliant)
            {
               result=Passwrod_Check&&USBKEY_CHECK;
            }
            if (user.LoginSecurityPolicyLevel==LoginSecurityPolicyLevel.unlimited || user.LoginSecurityPolicyLevel== LoginSecurityPolicyLevel.loose)
            {
                result = Passwrod_Check || USBKEY_CHECK;
            }



            if (result)
            {
                var token = tokenSerivce.GenerateToken(user.Id, user.Email);

                tokenSerivce.SaveToken(token);
                var userToken = new UserToken()
                {
                    Token = token.TokenStr,
                    UserId = user.Id,
                    Email = user.Email,
                    Username = user.Username
                };


                re.Ok = true;
                re.Data = userToken;
                return LeanoteJson(re);
            }
            return LeanoteJson(re);


        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="tickets"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public IActionResult SubmitLogin(string requestNumber)
        {
            return null;
        }

        [HttpDelete]
        //todo:注销函数
        public JsonResult Logout()
        {
            //ex:API当前不使用cookie和session判断用户身份，
            //API调用必须显式的提供token字段，以证明身份

            ApiRe apiRe = new ApiRe()
            {
                Ok = true,
                Msg = "未提供注销功能"
            };
            return Json(apiRe, MyJsonConvert.GetSimpleOptions());
        }

        //todo:注册
        [HttpPost]
        public async Task<IActionResult> Register(string email, string pwd)
        {
            //ex:API当前不使用cookie和session判断用户身份，
            //API调用必须显式的提供token字段，以证明身份
            //API调用者必须是管理员身份或者超级管理员身份，否则调用无效
            //如果用户设置二次验证必须显示提供二次验证码
            ApiRe re=new ApiRe();
            if (!this.config.SecurityConfig.OpenRegister)
            {
                re.Msg = "服务器管理员已经禁止用户注册功能";
                return LeanoteJson(re);
            }

            if (await authService.Register(email, pwd, 0))
            {
                re = new ApiRe()
                {
                    Ok = true,
                    Msg = "注册成功"
                };
            }
            else
            {
                re = new ApiRe()
                {
                    Ok = false,
                    Msg = "注册失败"
                };
            }
            return Json(re, MyJsonConvert.GetSimpleOptions());
        }
        [HttpPost]
        public async Task<IActionResult> ChangeUserPassword(string token,string password)
        {
            return null;
        }


        [HttpGet]
        public async Task<IActionResult> GetUserLoginLogging(string token)
        {
            var re = new ApiRe()
            {
                Ok = false,
                Data = null
            };
            var user = tokenSerivce.GetUserByToken(token);
            if (!user.IsAdmin())
            {
                re.Msg = "只有Admin账户才可以访问";
                return LeanoteJson(re);
            }
            var data= logging.GetAllUserLoggingLogin();
            if (config.SecurityConfig.LogNeedHmac)
            {
                try
                {
                    foreach (var item in data)
                    {
                        var verify =  item.VerifyHmac(cryptographyProvider);
                        item.Verify = verify;
                    }
                }
                catch (Exception ex)
                {
                    foreach (var item in data)
                    {
                        item.Verify = false;
                    }

                }
            }
           
            re.Data = data;
            re.Ok = true;
            return LeanoteJson(re);


        }

        /// <summary>
        /// 获得用户登录安全策略级别
        /// </summary>
        /// <returns></returns>
        [HttpGet,HttpPost]
        public async Task<IActionResult> GetUserLoginSecurityPolicyLevel(string email)
        {

            var re = new ApiRe()
            {
                Ok = false,
                Data = null
            };
            var user = userService.GetUserByEmail(email);
            if (user==null)
            {
                return LeanoteJson(re);
            }
            re.Ok = true;
            re.Data = user.LoginSecurityPolicyLevel;
            return LeanoteJson(re);

        }

        [HttpPost]
        public async Task<IActionResult> SetUserLoginSecurityPolicyLevel(string token,LoginSecurityPolicyLevel level)
        {

            var re = new ApiRe()
            {
                Ok = false,
                Data = null
            };
            var user = tokenSerivce.GetUserByToken(token);
            if (user==null)
            {
                return LeanoteJson(re);
            }
            userService.SetUserLoginSecurityPolicyLevel(user.Id, level);

            re.Ok = true;
            return LeanoteJson(re);


        }
    }
}