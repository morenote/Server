using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Morenote.Framework.Filter.Global;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using MoreNote.Value;

namespace MoreNote.Controllers.Member
{
    [Authorize(Roles = "Admin,SuperAdmin,User")]
    [Route("Member/{action=Index}")]
    public class MemberIndexController : BaseController
    {
        private NoteService noteService;
        public MemberIndexController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor, NoteService noteService
             ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.noteService= noteService;
           
        }

       // [Authorize(Roles = "Admin,SuperAdmin")] CheckLoginFilter
        [TypeFilter(typeof(CheckLoginFilter))]
        [HttpGet]
        public IActionResult Index()
        {

            User user = GetUserBySession();
            ViewBag.user = user;
            


            SetLocale();
            SetUserInfo();
            int countNote = noteService.CountNote(user.Id);
            int countBlog = noteService.CountBlog(user.Id);

            ViewBag.countNote = countNote;
            ViewBag.countBlog = countBlog;
            ViewBag.title = "个人中心";
            //return View("Views/Home/About.cshtml");
            return View("Views/Member/Index.cshtml");
        }
    }
}