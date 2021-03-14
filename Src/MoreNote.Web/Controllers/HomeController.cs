using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Model;
using MoreNote.Logic.Service;
using MoreNote.Value;
using System.Diagnostics;

namespace MoreNote.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
        }

        // leanote展示页, 没有登录的, 或已登录明确要进该页的
        public IActionResult Index()
        {
            //return Content("An API listing authors of docs.asp.net.");
            ViewBag.title = "leanote";
            ViewBag.msg = LanguageResource.GetMsg();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ErrorPage()
        {
            return View();
        }
    }
}