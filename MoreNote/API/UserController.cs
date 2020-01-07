
using System;
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.LeaNoteAPI
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        [HttpPost]
        public JsonResult Info(string token, string userId)
        {
            long myUserId =UserIdConvert.ConvertStrToLong(userId);
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
        [HttpPost]
        public JsonResult GetSyncState(string token, string userId)
        {
            long myUserId = UserIdConvert.ConvertStrToLong(userId);
            if (myUserId==0)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "Invalid user id",
                };
               
                return Json(apiRe, MyJsonConvert.GetOptions());

            }
            else
            {
                if (!AuthService.IsLogin(myUserId, token))
                {
                    ApiRe apiRe = new ApiRe()
                    {
                        Ok = false,
                        Msg = "Not logged in",
                    };
                    string json = JsonSerializer.Serialize(apiRe, MyJsonConvert.GetSimpleOptions());
                 
                    return Json(apiRe, MyJsonConvert.GetOptions());

                }
                User user = UserService.GetUserByUserId(myUserId);
                ApiGetSyncState apiGetSyncState = new ApiGetSyncState()
                {
                    LastSyncUsn = user.Usn,
                    LastSyncTime = user.FullSyncBefore
                };
            
                return  Json(apiGetSyncState,MyJsonConvert.GetOptions());
             

        }
          
        }

    }
}