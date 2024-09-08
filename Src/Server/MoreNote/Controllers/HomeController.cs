using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Framework.Controllers;
using MoreNote.Logic.Model;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Notes;
using System.Diagnostics;

namespace MoreNote.Controllers
{
    public class HomeController : BaseController
	{
		BlogService blogService;
		public HomeController(AttachService attachService,
			BlogService blogService,
			 TokenSerivce tokenSerivce,
			NoteFileService noteFileService,
			UserService userService,
			ConfigFileService configFileService,
			IHttpContextAccessor accessor
			 ) :
			base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{
			this.blogService = blogService;
		}

		// leanote展示页, 没有登录的, 或已登录明确要进该页的

		[HttpGet, HttpPost]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Index()
		{
			//return Content("An API listing authors of docs.asp.net.");
			//var host = Request.Host.Host;
			//var hostingBundle = this.blogService.FindBlogHostingBundle(host);
			//if (hostingBundle != null)
			//{
			//	return Redirect("/Blog/Index");
			//}
			//SetUserInfo();
			ViewBag.title = "leanote";
			//ViewBag.openRegister = configFileService.ReadConfig().SecurityConfig.OpenRegister;
			//ViewBag.openDemo = configFileService.ReadConfig().SecurityConfig.OpenDemo;

			//SetLocale();

			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		[HttpGet, HttpPost]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		[HttpGet, HttpPost]
		public IActionResult ErrorPage()
		{
			return View();
		}
	}
}