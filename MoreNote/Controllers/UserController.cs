using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Value;

namespace MoreNote.Controllers
{
    public class UserController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult Login()
        {
            ViewBag.msg = LanguageResource.GetMsg();
            return View();
        }
        public IActionResult Register()
        {
            //return Content("An API listing authors of docs.asp.net.");
            ViewBag.title = "leanote";
            ViewBag.msg = LanguageResource.GetMsg();
            return View();
        }
    }
}