
using System;
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ModelBinder;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.API
{
    [Route("api/[controller]/[action]")]
    public class UserController : ApiBaseController
    {
        public UserController(IHttpContextAccessor accessor) : base(accessor)
        {
        }

        //获取用户信息

        public JsonResult Info(string token, [ModelBinder(BinderType = typeof(Hex2LongModelBinder))]long userId)
        {
           // long myUserId =MyConvert.HexToLong(userId);
            long myUserId = userId;
            if (myUserId!=0)
            {
                if (!AuthService.IsLogin(myUserId,token))
                {
                    ApiRe apiRe = new ApiRe()
                    {
                        Ok = false,
                        Msg = "Not logged in",
                    };
                    return Json(apiRe, MyJsonConvert.GetOptions());
                }
                User user = UserService.GetUserByUserId(myUserId);

                if (user == null)
                {
                    ApiRe apiRe = new ApiRe()
                    {
                        Ok = false,
                        Msg = "The user does not exist",
                    };
                    return Json(apiRe, MyJsonConvert.GetOptions());
                }
                else
                {
                    ApiUser apiUser = new ApiUser()
                    {
                        UserId = user.UserId.ToString("x"),
                        Username = user.Username,
                        Email = user.Email,
                        Logo = user.Logo,
                        Verified = user.Verified
                    };
                    return Json(apiUser, MyJsonConvert.GetOptions());
                }
            }
            else
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "Invalid user id",
                };
                return Json(apiRe, MyJsonConvert.GetOptions());
            }

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
            
                User user = TokenSerivce.GetUserByToken(token);
                if (user==null)
                {
                    ApiRe apiRe = new ApiRe()
                    {
                        Ok = false,
                        Msg = "Not logged in",
                    };
                    string json = JsonSerializer.Serialize(apiRe, MyJsonConvert.GetSimpleOptions());

                    return Json(apiRe, MyJsonConvert.GetOptions());
                }
                ApiGetSyncState apiGetSyncState = new ApiGetSyncState()
                {
                    LastSyncUsn = user.Usn,
                    LastSyncTime = UnixTimeHelper.GetTimeStampInLong(DateTime.Now)
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