using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers
{
    public class CrowdfundingController1 : Controller
    {
        /// <summary>
        /// 先写个众筹网站
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {

            return View();
        }
    }
}
