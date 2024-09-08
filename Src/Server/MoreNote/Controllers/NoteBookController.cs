using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Notes;
using MoreNote.Models.Entity.Leanote.Notes;

using System.Text.Json;

namespace MoreNote.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin,User")]
	public class NoteBookController : BaseController
	{
		private NoteCollectionService notebookService;

		public NoteBookController(AttachService attachService
			, TokenSerivce tokenSerivce
			, NoteFileService noteFileService
			, UserService userService
			, ConfigFileService configFileService
			, IHttpContextAccessor accessor,
			NoteCollectionService notebookService
			 ) :
			base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{
			this.notebookService = notebookService;
		}
		[HttpGet, HttpPost]
		public IActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// 得到用户的所有笔记本
		/// </summary>
		/// <returns></returns>
		[HttpGet, HttpPost]
		public JsonResult GetNotebooks()
		{
			long? userid = GetUserIdBySession();
			NoteCollection[] noteBoooks = notebookService.GetNoteBookTree(userid);
			return Json(noteBoooks, MyJsonConvert.GetLeanoteOptions());
		}

		/// <summary>
		/// 添加笔记本
		/// </summary>
		/// <param name="notebookId"></param>
		/// <param name="title"></param>
		/// <param name="parentNotebookId"></param>
		/// <returns></returns>
		[HttpGet, HttpPost]
		public JsonResult AddNotebook(string notebookId, string title, string parentNotebookId)
		{
			NoteCollection notebook;
			var result = notebookService.AddNotebook(notebookId.ToLongByHex(), GetUserIdBySession(), parentNotebookId.ToLongByHex(), title, out notebook);
			if (result)
			{
				return Json(notebook, MyJsonConvert.GetLeanoteOptions());
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
		[HttpGet, HttpPost]
		public JsonResult UpdateNotebookTitle(string notebookId, string title)
		{
			var result = notebookService.UpdateNotebookTitle(notebookId.ToLongByHex(), GetUserIdBySession(), title);
			return Json(result, MyJsonConvert.GetSimpleOptions());
		}
		/// <summary>
		/// 笔记本拖放
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpGet, HttpPost]
		public JsonResult DragNotebooks(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return Json(false);
			}
			DragNotebooksInfo info = JsonSerializer.Deserialize<DragNotebooksInfo>(data, MyJsonConvert.GetLeanoteOptions());
			var result = notebookService.DragNotebooks(GetUserIdBySession(), info.curNotebookId, info.parentNotebookId, info.siblings);
			return Json(result);
		}
		[HttpGet, HttpPost]
		public JsonResult SetNotebook2Blog(string notebookId, bool isBlog)
		{
			var result = notebookService.ToBlog(GetUserIdBySession(), notebookId.ToLongByHex(), isBlog);
			return Json(result, MyJsonConvert.GetSimpleOptions());
		}

	}
}