﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoreNote.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class APPController : BaseController
    {
        public APPController(IHttpContextAccessor accessor) : base(accessor)
        {


        }
        public IActionResult Index()
        {
            ViewData["Title"] = "后台管理员界面";
            return View();
        }

    }
}