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
using MoreNote.Models.Model.FIDO2;

using System.Text.Json;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Auth/[action]")]
    public class AuthAPIController : APIBaseController
    {
        private AuthService authService;
      
        public AuthAPIController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor
            ,AuthService authService
            , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
            this.authService = authService;
         
        }

        /// <summary>
        /// 登陆
        ///  成功返回 {Ok: true, Item: token }
        ///  失败返回 {Ok: false, Msg: ""}
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
       //[HttpPost]
        public IActionResult Login(string email, string pwd)
        {
            string tokenValue = "";
            User user;
            var re=new ApiRe();

            if (authService.LoginByPWD(email, pwd, out tokenValue, out user))
            {
                var userToken=new UserToken()
                {
                    Token = tokenValue,
                    UserId = user.UserId,
                    Email = user.Email,
                    Username = user.Username
                };
                re.Ok = true;
                re.Data=userToken;

                return LeanoteJson(re);
            }
            else
            {
                re.Msg = "用户名或密码有误";
                return LeanoteJson(re);
            }
        }

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
        public JsonResult Register(string email, string pwd)
        {
            //ex:API当前不使用cookie和session判断用户身份，
            //API调用必须显式的提供token字段，以证明身份
            //API调用者必须是管理员身份或者超级管理员身份，否则调用无效
            //如果用户设置二次验证必须显示提供二次验证码
            ApiRe apiRe;
            if (authService.Register(email, pwd, 0) && false)
            {
                apiRe = new ApiRe()
                {
                    Ok = true,
                    Msg = "注册成功"
                };
            }
            else
            {
                apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "注册失败"
                };
            }
            return Json(apiRe, MyJsonConvert.GetSimpleOptions());
        }

       
    }
}