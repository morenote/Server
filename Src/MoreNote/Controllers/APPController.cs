using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

namespace MoreNote.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class APPController : BaseController
    {
        public APPController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , ILoggingService loggingService
            , IHttpContextAccessor accessor) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor,loggingService)
        {
           

        }
        public IActionResult Index()
        {
            ViewData["Title"] = "后台管理员界面";
            return View();
        }

    }
}