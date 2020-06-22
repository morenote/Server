using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers
{
    public class ErrorsController1 : Controller
    {
        public IActionResult Unauthorized1()
        {
            return Unauthorized();
            return View();
        }

    }
}
