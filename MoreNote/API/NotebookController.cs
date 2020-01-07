
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System.Collections.Generic;

namespace MoreNote.LeaNoteAPI
{
    [Route("api/[controller]/[action]")]
   // [ApiController]
    public class NotebookController : Controller
    {
        [HttpPost]
        public JsonResult GetSyncNotebooks(string userId, string token,int afterUsn,int maxEntry)
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
                    return Json(apiRe, MyJsonConvert.GetOptions());
                }
                Notebook[] notebook = NotebookService.GeSyncNotebooks(myUserId, afterUsn, maxEntry);
                return Json(notebook, MyJsonConvert.GetOptions());
            }
        }

    }
}