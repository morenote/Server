/**
 * copy from https://github.com/ldqk/Masuit.MyBlogs
 * MIT License
 * Change:init
 * */

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult FileManager()
        {
            return View();
        }
    }
}