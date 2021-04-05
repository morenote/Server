using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
// ReSharper disable All
namespace MoreNote.Controllers.Member
{
    public class MemberBaseController : Controller
    {
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        public IActionResult Index()
        {
            return View();
        }
    }
}