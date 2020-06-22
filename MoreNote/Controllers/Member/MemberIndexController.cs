using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Index()
        {

            User user = GetUserBySession();
            ViewBag.user = user;
            ViewBag.msg = LanguageResource.GetMsg();
            ViewBag.member = LanguageResource.GetMember();

            int countNote = NoteService.CountNote(user.UserId);
            int countBlog = NoteService.CountBlog(user.UserId);

            ViewBag.countNote = countNote;
            ViewBag.countBlog = countBlog;
            ViewBag.title = "个人中心";
            //return View("Views/Home/About.cshtml");
            return View("Views/Member/Index.cshtml");
        }
    }
}