using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.Controllers.Admin
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : AdminBaseController
    {
        private NoteService noteService;

        public AdminController(AttachService attachService
              , TokenSerivce tokenSerivce
              , NoteFileService noteFileService
              , UserService userService
              , ConfigFileService configFileService
              , IHttpContextAccessor accessor
              ,NoteService noteService
              , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
            this.noteService = noteService;
        }

        public IActionResult Index()
        {
            User user = GetUserBySession();
            ViewBag.user = user;

            SetLocale();
            SetUserInfo();
            var countUser=userService.CountUser();
            int countNote = noteService.CountNote(user.UserId);
            int countBlog = noteService.CountBlog(user.UserId);

            ViewBag.countUser=countUser;
            ViewBag.countNote = countNote;
            ViewBag.countBlog = countBlog;
            ViewBag.title = "ControlPanel ";
            //return View("Views/Home/About.cshtml");

            return View("Views/admin/index.cshtml");
        }
         [Route("admin/fileManager")]
        public ActionResult FileManager()
        {
            SetUserInfo();
            //todo:CSRF防御攻击
            return View("Views/admin/fileManager.cshtml");
        }
    }
}