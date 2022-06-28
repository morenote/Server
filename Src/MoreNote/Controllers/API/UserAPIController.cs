
using System;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;

using github.hyfree.GM;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.ModelBinder;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Models.DTO.Leanote.ApiRequest;
using MoreNote.Models.DTO.Leanote.USBKey;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/User/[action]")]
    public class UserAPIController : APIBaseController
    {
        private AuthService authService;
        private UserService userService;
        private TokenSerivce tokenSerivce;
        private RealNameService realNameService;
        private EPassService ePassService;
        private GMService gMService;
        private DataSignService dataSignService;
    
        public UserAPIController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , GMService gMService
            , RealNameService realNameService
            , IHttpContextAccessor accessor, AuthService authService
            , EPassService ePass
            ,DataSignService dataSignService
           ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.authService = authService;
            this.userService = userService;
            this.tokenSerivce = tokenSerivce;
            this.realNameService = realNameService;
            this.ePassService = ePass;
            this.gMService = gMService;
            this.dataSignService = dataSignService;
        }

        /// <summary>
        /// 通过token获得用户本人的详情
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        public IActionResult GetUserInfoByToken(string token)
        {
            var user=tokenSerivce.GetUserByToken(token);
            var re=new ApiRe();
            if (user==null)
            {
                re.Msg= "NOTLOGIN";
            }
            re.Ok=true;
            re.Data = user;
            return LeanoteJson(re);
        }
        /// <summary>
        /// 通过邮箱获得用户详情
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public IActionResult GetUserInfoByEmail(string email)
        {
            var user = userService.GetUserByEmail(email);
            var re = new ApiRe();
            if (user == null)
            {
                re.Msg = "NOTLOGIN";
            }
            re.Ok = true;
            re.Data = user;
            return LeanoteJson(re);
        }
        //获取用户的登录策略
        public JsonResult GetUserLoginSecurityStrategy(string UserName)
        {

            var ss=  userService.GetGetUserLoginSecurityStrategy(UserName);
            ApiRe apiRe = new ApiRe()
            {
                Ok = (ss!=null),
                Msg = "",
                Data=ss
                
            };
            return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
        }

        //todo:修改用户名
        public IActionResult UpdateUsername()
        {
            return null;
        }
        //todo:修改密码
        public async Task<IActionResult> UpdatePwd(string token,string oldPwd,string pwd)
        {
            ApiRe re = new ApiRe();
            User user = tokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                re.Msg = "NOTLOGIN";

                return Json(re, MyJsonConvert.GetLeanoteOptions());
            }
            try
            {
                var result = await userService.UpdatePwd(user.UserId, oldPwd, pwd);
                re.Ok = result;
                if (!result)
                {
                    re.Msg = "更新密码失败";

                }
            }
            catch (Exception ex)
            {
                re.Msg = ex.Message;
                re.Ok = false;
            }
          
          
           return LeanoteJson(re);
        }
        //获得同步状态
        //[HttpPost]
        public JsonResult GetSyncState(string token)
        {
            
                User user = tokenSerivce.GetUserByToken(token);
                if (user==null)
                {
                    ApiRe apiRe = new ApiRe()
                    {
                        Ok = false,
                        Msg = "NOTLOGIN",
                    };
                   

                    return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
                }
                ApiGetSyncState apiGetSyncState = new ApiGetSyncState()
                {
                    LastSyncUsn = user.Usn,
                    LastSyncTime = UnixTimeUtil.GetTimeStampInLong(DateTime.Now)
                };
            
                return  Json(apiGetSyncState,MyJsonConvert.GetSimpleOptions());
        }

        //todo:头像设置
        public IActionResult UpdateLogo()
        {
            return null;
        }
        //todo:上传图片
        public IActionResult uploadImage()
        {
            return null;
        }
        public  async Task<IActionResult> GetRealNameInformation(string token, string digitalEnvelopeJson, string dataSignJson)
        {

            var re = new ApiRe();
            DigitalEnvelope digitalEnvelope = null;
            //数字信封
            if (this.config.SecurityConfig.ForceDigitalEnvelope)
            {
                digitalEnvelope = DigitalEnvelope.FromJSON(digitalEnvelopeJson);
                
            }
            //验证签名
            var dataSign = DataSignDTO.FromJSON(dataSignJson);
            var verify = await this.ePassService.VerifyDataSign(dataSign);
            if (!verify)
            {
                return LeanoteJson(re);
            }
            verify = dataSign.SignData.Operate.Equals("/api/User/GetRealNameInformation");
            if (!verify)
            {
                re.Msg = "Operate is not Equals ";
                return LeanoteJson(re);
            }
            //签字签名和数字信封数据

            //签名存证
            this.dataSignService.AddDataSign(dataSign, "GetRealNameInformation");

            User user = tokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "NOTLOGIN",
                };
                return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
            }
            var realName =  this.realNameService.GetRealNameInformationByUserId(user.UserId);
            re.Ok = true;
            re.Data = realName;
            return LeanoteJson(re);
        }
        public async Task<IActionResult> SetRealNameInformation(string token,string sfz,string digitalEnvelopeJson,string dataSignJson)
        {
            var re = new ApiRe();
            DigitalEnvelope digitalEnvelope = null;
            //数字信封
            if (this.config.SecurityConfig.ForceDigitalEnvelope)
            {

                digitalEnvelope = DigitalEnvelope.FromJSON(digitalEnvelopeJson);
                var data = digitalEnvelope.GetPayLoadValue(this.gMService, this.config.SecurityConfig.PrivateKey);
                if (data == null)
                {
                    throw new Exception("数字信封解密失败");
                }
                //赋予解密的数字信封
                sfz = data;
            }
            //验证签名
            var dataSign = DataSignDTO.FromJSON(dataSignJson);
            var verify = await this.ePassService.VerifyDataSign(dataSign);
            if (!verify)
            {
                return LeanoteJson(re);
            }
            verify = dataSign.SignData.Operate.Equals("/api/User/SetRealNameInformation");
            if (!verify)
            {
                re.Msg = "Operate is not Equals ";
                return LeanoteJson(re);
            }
            //签字签名和数字信封数据

            //签名存证
            this.dataSignService.AddDataSign(dataSign, "SetRealNameInformation");

            User user = tokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "NOTLOGIN",
                };
                return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
            }
         
             this.realNameService.SetRealName(user.UserId, sfz);
            re.Ok=true;
            return LeanoteJson(re);
        }




    }
}