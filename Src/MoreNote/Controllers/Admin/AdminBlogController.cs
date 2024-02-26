using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Logic.Service;
using MoreNote.Models.Entity.Leanote.User;

namespace MoreNote.Controllers.Admin
{
	public class AdminBlogController : AdminBaseController
	{


		public AdminBlogController(AttachService attachService
			, TokenSerivce tokenSerivce
			, NoteFileService noteFileService
			, UserService userService
			, ConfigFileService configFileService
			, IHttpContextAccessor accessor,
			AccessService accessService


		   ) :
		  base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{

		}
		public IActionResult Index()
		{
			UserInfo user = GetUserBySession();
			ViewBag.user = user;

			SetLocale();



			ViewBag.title = "ControlPanel ";
			//return View("Views/Home/About.cshtml");

			return View("Views/admin/blog/list.cshtml");
		}
	}
}