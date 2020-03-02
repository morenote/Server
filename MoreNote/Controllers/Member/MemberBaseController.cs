using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers.Member
{
    public class MemberBaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}