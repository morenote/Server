using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers.Admin
{
    public class AdminBlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}