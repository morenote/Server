
using System;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;

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
           ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.authService = authService;
            this.userService = userService;
            this.tokenSerivce = tokenSerivce;

        }

        //获取用户信息

        public IActionResult Info(string token)
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
           var result= await userService.UpdatePwd(user.UserId, oldPwd, pwd);
           re.Ok = result;
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
   
    }
}