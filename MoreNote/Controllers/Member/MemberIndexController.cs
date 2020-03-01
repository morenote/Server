using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Value;

namespace MoreNote.Controllers.member
{
    [Route("Member/{action=Index}")]
    public class MemberIndexController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.msg = LanguageResource.GetMsg();
            ViewBag.member = LanguageResource.GetMember();
            //return View("Views/Home/About.cshtml");
            return View("Views/Member/Index.cshtml");
        }
    }
}