using Microsoft.AspNetCore.Mvc;
using MoreNote.API;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.API
{
    [Route("api/[controller]/[action]")]
    // [ApiController]
    public class TagController : ApiBaseController
    {
        //todo:获取同步的标签
        public JsonResult GetSyncTags(string token, int afterUsn, int maxEntry)
        {
            User user = TokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "Not logged in",
                };
                return Json(apiRe, MyJsonConvert.GetOptions());
            }
            NoteTag[] noteTags = TagService.GeSyncTags(user.UserId, afterUsn, maxEntry);
            return Json(noteTags, MyJsonConvert.GetOptions());
        }
        //todo:添加Tag
        public IActionResult AddTag()
        {
            return null;
        }
        //todo:删除标签
        public IActionResult DeleteTag()
        {
            return null;
        }
   

    }
}