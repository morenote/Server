using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers.member
{
    [Route("Member/{action=Index}")]
    public class MemberIndexController : Controller
    {
        public IActionResult Index()
        {
            //return View("Views/Home/About.cshtml");
            return View("Views/Member/Index.cshtml");
        }
    }
}