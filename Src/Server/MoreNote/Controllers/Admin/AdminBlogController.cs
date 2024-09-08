using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Notes;
using MoreNote.Models.Entity.Leanote.User;

namespace MoreNote.Controllers.Admin
{
    /// <summary>
    /// 管理者博客控制器
    /// </summary>
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
		/// <summary>
		/// 主页
		/// </summary>
		/// <returns></returns>
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