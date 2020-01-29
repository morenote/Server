using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.API
{
    [Route("api/[controller]/[action]")]
    // [ApiController]
    public class TagController : ApiBaseController
    {
        public TagController(IHttpContextAccessor accessor) : base(accessor)
        {
        }

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
        public JsonResult AddTag(string token, string tag)
        {
            NoteTag noteTag = TagService.AddOrUpdateTag(getUserIdByToken(token), tag);
            if (noteTag == null)
            {
                return Json(new ApiRe() { Ok = false, Msg = "添加标签失败" }, MyJsonConvert.GetOptions());
            }
            else
            {
                return Json(noteTag, MyJsonConvert.GetOptions());
            }
        }
        //todo:删除标签
        public IActionResult DeleteTag(string token, string tag, int usn)
        {
            bool result = TagService.DeleteTagApi(getUserIdByToken(token), tag, usn, out int toUsn, out string msg);
            if (result)
            {
                ReUpdate reUpdate = new ReUpdate()
                {
                    Ok = true,
                    Usn = toUsn,
                    Msg = msg
                };
                return Json(reUpdate, MyJsonConvert.GetOptions());
            }
            else
            {
                ApiRe apiRe=new ApiRe()
                {
                    Ok=false,
                    Msg=msg
                };
            return Json(apiRe,MyJsonConvert.GetOptions());

            }
        }


    }
}