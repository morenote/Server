using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.Controllers.Admin
{
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
              ) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.noteService = noteService;
        }

        public IActionResult Index()
        {
            User user = GetUserBySession();
            ViewBag.user = user;

            SetLocale();

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
    }
}