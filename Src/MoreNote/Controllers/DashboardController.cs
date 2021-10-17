/**
 * copy from https://github.com/ldqk/Masuit.MyBlogs
 * MIT License
 * Change:init
 * */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult FileManager()
        {
            return View();
        }
    }
}