using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Value;

namespace MoreNote.Controllers.Member
{
    [Route("Member/{action=Index}")]
    public class MemberIndexController : BaseController
    {
        public MemberIndexController(IHttpContextAccessor accessor) : base(accessor)
        {
        }

        public IActionResult Index()
        {
            string userHex = HttpContext.Session.GetString("_userId");

            if (string.IsNullOrEmpty(userHex))
            {
                //没登陆
                return Redirect("/Auth/login");
            }
            User user = GetUserBySession();
            if (user==null)
            {
                //身份无效
                return Redirect("/Auth/login");
            }
            ViewBag.user = user;
            ViewBag.msg = LanguageResource.GetMsg();
            ViewBag.member = LanguageResource.GetMember();

            int countNote = NoteService.CountNote(user.UserId);
            int countBlog = NoteService.CountBlog(user.UserId);

            ViewBag.countNote = countNote;
            ViewBag.countBlog = countBlog;
            //return View("Views/Home/About.cshtml");
            return View("Views/Member/Index.cshtml");
        }
    }
}