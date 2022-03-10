
using System;
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.ModelBinder;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/User/[action]")]
    public class UserAPIController : APIBaseController
    {
        private AuthService authService;
        private UserService userService;
        private TokenSerivce tokenSerivce;
        public UserAPIController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor, AuthService authService
           , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
            this.authService = authService;
            this.userService = userService;
            this.tokenSerivce = tokenSerivce;

        }

        //获取用户信息

        public JsonResult Info(string token, [ModelBinder(BinderType = typeof(Hex2LongModelBinder))]long? userId)
        {
           // long? myUserId =MyConvert.HexToLong(userId);
            long? myUserId = userId;
            if (myUserId!=null)
            {
                if (!authService.IsLogin(myUserId,token))
                {
                    ApiRe apiRe = new ApiRe()
                    {
                        Ok = false,
                        Msg = "NOTLOGIN",
                    };
                    return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
                }
                User user = userService.GetUserByUserId(myUserId);

                if (user == null)
                {
                    ApiRe apiRe = new ApiRe()
                    {
                        Ok = false,
                        Msg = "The user does not exist",
                    };
                    return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
                }
                else
                {
                    ApiUser apiUser = new ApiUser()
                    {
                        UserId = user.UserId.ToHex24(),
                        Username = user.Username,
                        Email = user.Email,
                        Logo = user.Logo,
                        Verified = user.Verified
                    };
                    return Json(apiUser, MyJsonConvert.GetLeanoteOptions());
                }
            }
            else
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "Invalid user id",
                };
                return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
            }

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
        public IActionResult UpdatePwd()
        {
            return null;
        }
        //获得同步状态
      //  [HttpPost]
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
                    string json = JsonSerializer.Serialize(apiRe, MyJsonConvert.GetSimpleOptions());

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
   
    }
}