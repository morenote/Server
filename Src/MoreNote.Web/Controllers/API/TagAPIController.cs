using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Tag/[action]")]
    // [ApiController]
    public class TagAPIController : APIBaseController
    {
        private TokenSerivce tokenSerivce;
        private TagService tagService;
        public TagAPIController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            TagService tagService) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
           
            this.tokenSerivce= tokenSerivce;
            this.tagService= tagService;
            

        }

        //todo:获取同步的标签
        public JsonResult GetSyncTags(string token, int afterUsn, int maxEntry)
        {
            User user = tokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "Not logged in",
                };
                return Json(apiRe, MyJsonConvert.GetOptions());
            }
            NoteTag[] noteTags = tagService.GeSyncTags(user.UserId, afterUsn, maxEntry);
            return Json(noteTags, MyJsonConvert.GetOptions());
        }
        //todo:添加Tag
        public JsonResult AddTag(string token, string tag)
        {
            NoteTag noteTag = tagService.AddOrUpdateTag(GetUserIdByToken(token), tag);
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
            bool result = tagService.DeleteTagApi(GetUserIdByToken(token), tag, usn, out int toUsn, out string msg);
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