using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System.Text.Json;

namespace MoreNote.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin,User")]
    public class NoteBookController : BaseController
    {
        private NotebookService notebookService;

        public NoteBookController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor, NotebookService notebookService) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.notebookService = notebookService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 得到用户的所有笔记本
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNotebooks()
        {
            long? userid = GetUserIdBySession();
            Notebook[] noteBoooks = notebookService.GetNoteBookTree(userid);
            return Json(noteBoooks, MyJsonConvert.GetOptions());
        }
        /// <summary>
        /// 删除笔记本
        /// </summary>
        /// <param name="notebookId">笔记本id 要求必须是long类型的hex形式</param>
        /// <returns></returns>
        public JsonResult DeleteNotebook(string notebookId)
        {
            var msg = string.Empty;
            var result = notebookService.DeleteNotebook(GetUserIdBySession(), notebookId.ToLongByHex(), out msg);
            return Json(new Re() { Ok = result, Msg = msg },MyJsonConvert.GetSimpleOptions());
        }
        /// <summary>
        /// 添加笔记本
        /// </summary>
        /// <param name="notebookId"></param>
        /// <param name="title"></param>
        /// <param name="parentNotebookId"></param>
        /// <returns></returns>
        public JsonResult AddNotebook(string notebookId,string title,string parentNotebookId)
        {
            Notebook notebook;
            var result=notebookService.AddNotebook(notebookId.ToLongByHex(),GetUserIdBySession(),parentNotebookId.ToLongByHex(),title,out notebook);
            if (result)
            {
                return Json(notebook,MyJsonConvert.GetOptions());
            }
            else
            {
                return Json(false);
            }
        }
        /// <summary>
        /// 更新笔记本标题
        /// </summary>
        /// <param name="notebookId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public JsonResult UpdateNotebookTitle(string notebookId,string title)
        {
            var result=notebookService.UpdateNotebookTitle(notebookId.ToLongByHex(),GetUserIdBySession(),title);
            return Json(result, MyJsonConvert.GetSimpleOptions());
        }
        /// <summary>
        /// 笔记本拖放
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult DragNotebooks(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return Json(false);
            }
            DragNotebooksInfo info= JsonSerializer.Deserialize<DragNotebooksInfo>(data,MyJsonConvert.GetOptions());
            var result=notebookService.DragNotebooks(GetUserIdBySession(),info.curNotebookId,info.parentNotebookId,info.siblings);
            return Json(result);
        }
        public JsonResult SetNotebook2Blog(string notebookId,bool isBlog)
        {
            var result=notebookService.ToBlog(GetUserIdBySession(),notebookId.ToLongByHex(),isBlog);
            return Json(result, MyJsonConvert.GetSimpleOptions());
        }

    }
}