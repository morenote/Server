using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

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
            TagService tagService
            ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
           
            this.tokenSerivce= tokenSerivce;
            this.tagService= tagService;
            

        }

        //todo:获取同步的标签
        [HttpGet]
        public JsonResult GetSyncTags(string token, int afterUsn, int maxEntry)
        {
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
            NoteTag[] noteTags = tagService.GeSyncTags(user.Id, afterUsn, maxEntry);
            return Json(noteTags, MyJsonConvert.GetLeanoteOptions());
        }
        //todo:添加Tag
        [HttpPost]
        public JsonResult AddTag(string token, string tag)
        {
            NoteTag noteTag = tagService.AddOrUpdateTag(GetUserIdByToken(token), tag);
            if (noteTag == null)
            {
                return Json(new ApiRe() { Ok = false, Msg = "添加标签失败" }, MyJsonConvert.GetLeanoteOptions());
            }
            else
            {
                return Json(noteTag, MyJsonConvert.GetLeanoteOptions());
            }
        }
        //todo:删除标签
        [HttpPost,HttpDelete]
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
                return Json(reUpdate, MyJsonConvert.GetLeanoteOptions());
            }
            else
            {
                ApiRe apiRe=new ApiRe()
                {
                    Ok=false,
                    Msg=msg
                };
            return Json(apiRe,MyJsonConvert.GetLeanoteOptions());

            }
        }


    }
}