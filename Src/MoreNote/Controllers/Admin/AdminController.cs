using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Logic.Service;
using MoreNote.Models.Entity.Leanote.User;

namespace MoreNote.Controllers.Admin
{

	[Authorize(Roles = "Admin,SuperAdmin")]
	public class AdminController : AdminBaseController
	{
		private NoteService noteService;

		public AdminController(AttachService attachService
			  , TokenSerivce tokenSerivce
			  , NoteFileService noteFileService
			  , UserService userService
			  , ConfigFileService configFileService
			  , IHttpContextAccessor accessor
			  , NoteService noteService
			 ) :
			base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{
			this.noteService = noteService;
		}

		public IActionResult Index()
		{
			UserInfo user = GetUserBySession();
			ViewBag.user = user;

			SetLocale();
			SetUserInfo();
			var countUser = userService.CountUser();
			int countNote = noteService.CountNote(user.Id);
			int countBlog = noteService.CountBlog(user.Id);

			ViewBag.countUser = countUser;
			ViewBag.countNote = countNote;
			ViewBag.countBlog = countBlog;
			ViewBag.title = "ControlPanel ";
			//return View("Views/Home/About.cshtml");

			return View("Views/admin/index.cshtml");
		}
		[Route("/admin/fileManager")]
		[HttpGet]
		public ActionResult FileManager()
		{
			SetUserInfo();
			//todo:CSRF防御攻击
			return View("Views/admin/fileManager.cshtml");
		}
	}
}