using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.API
{
    [Route("api/[controller]")]
   // [ApiController]
    public class NoteController : Controller
    {
        //todo:获取同步的笔记
        public JsonResult GetSyncNotes(int afterUsn,int maxEntry,string token)
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
            else
            {

            }
            return null;
        }
        //todo:得到笔记本下的笔记
        public IActionResult GetNotes(string token,string notebookId)
        {
            
            return null;
        }
        //todo:得到trash
        public IActionResult GetTrashNotes()
        {
            return null;
        }
        //todo:获取笔记
        public IActionResult GetNote()
        {
            return null;
        }
        //todo:得到note和内容
        public IActionResult GetNoteAndContent()
        {
            return null;

        }
        //todo:格式化URL
        public IActionResult fixPostNotecontent()
        {
            return null;
        }
        //todo:得到内容
        public IActionResult GetNoteContent()
        {
            return null;
        }
        //todo:添加笔记
        public IActionResult AddNote()
        {
            return null;
        }
        //todo:更新笔记
        public IActionResult UpdateNote()
        {
            return null;
        }
        //todo:删除trash
        public IActionResult DeleteTrash()
        {
            return null;
        }
        //todo:导出成PDF
        public IActionResult ExportPdf()
        {
            return null;
        }

    }
}