using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Logic.Entity;
using MoreNote.Value;

namespace MoreNote.Controllers.Member
{
    [Route("/member/user/{action=Username}")]
    public class MemberUserController : BaseController
    {
        public MemberUserController(IHttpContextAccessor accessor) : base(accessor)
        {

        }
        public IActionResult Username()
        {
            string userHex = HttpContext.Session.GetString("_userId");

            if (string.IsNullOrEmpty(userHex))
            {
                //没登陆
                return Redirect("/Auth/login");
            }
            User user = GetUserBySession();
            if (user == null)
            {
                //身份无效
                return Redirect("/Auth/login");
            }
            ViewBag.user = user;
            ViewBag.msg = LanguageResource.GetMsg();
            ViewBag.member = LanguageResource.GetMember();
            ViewBag.title = "用户名";
            return View("Views/Member/user/Username.cshtml");
        }
    
        public IActionResult Email()
        {
            string userHex = HttpContext.Session.GetString("_userId");

            if (string.IsNullOrEmpty(userHex))
            {
                //没登陆
                return Redirect("/Auth/login");
            }
            User user = GetUserBySession();
            if (user == null)
            {
                //身份无效
                return Redirect("/Auth/login");
            }
            ViewBag.user = user;
            ViewBag.msg = LanguageResource.GetMsg();
            ViewBag.member = LanguageResource.GetMember();
            ViewBag.title = "电子邮箱";
            return View("Views/Member/user/email.cshtml");
        }
        public IActionResult Password()
        {
            string userHex = HttpContext.Session.GetString("_userId");

            if (string.IsNullOrEmpty(userHex))
            {
                //没登陆
                return Redirect("/Auth/login");
            }
            User user = GetUserBySession();
            if (user == null)
            {
                //身份无效
                return Redirect("/Auth/login");
            }
            ViewBag.user = user;
            ViewBag.msg = LanguageResource.GetMsg();
            ViewBag.member = LanguageResource.GetMember();
            ViewBag.title = "密码";
            return View("Views/Member/user/password.cshtml");
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Avatar()
        {
            string userHex = HttpContext.Session.GetString("_userId");

            if (string.IsNullOrEmpty(userHex))
            {
                //没登陆
                return Redirect("/Auth/login");
            }
            User user = GetUserBySession();
            if (user == null)
            {
                //身份无效
                return Redirect("/Auth/login");
            }
            ViewBag.user = user;
            ViewBag.msg = LanguageResource.GetMsg();
            ViewBag.member = LanguageResource.GetMember();
            ViewBag.title = "头像";
            return View("Views/Member/user/avatar.cshtml");
        }
    }
}