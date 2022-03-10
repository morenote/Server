using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
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
    public class AdminUserController : AdminBaseController
    {
        private NoteService noteService;
        private AuthService authService;
        public AdminUserController(AttachService attachService
              , TokenSerivce tokenSerivce
              , NoteFileService noteFileService
              , UserService userService
              , ConfigFileService configFileService
              , IHttpContextAccessor accessor
              , NoteService noteService
                ,AuthService authService
              , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
            this.noteService = noteService;
            this.authService=authService;
        }
        int userPageSize=10;

        public IActionResult Index(string keywords,string sorter,int pageSize)
        {
            var isAsc=true;
            var sorterField="";
            var pageNumber=this.GetPage();
            if (pageSize<1)
            {
                pageSize=this.userPageSize;

            }
            var sql=GetSorterSQL(sorter);
            Page pageInfo=null;
            User[] users=null;
            if (!string.IsNullOrEmpty(keywords))
            {
                users=  userService.ListUsers(pageNumber, pageSize, sorterField, isAsc, keywords,out pageInfo);
            }
            else
            {
               users=   userService.ListUsers(pageNumber, pageSize, sorterField, isAsc, out pageInfo);
            }
            ViewBag.pageInfo=pageInfo;
            ViewBag.users=users;
            ViewBag.keywords=keywords;
            SetLocale();

            return View("Views/admin/user/list.cshtml");
        }
        public IActionResult Add()
        {
              return View("Views/admin/user/add.cshtml");
        }

        public IActionResult Register(string email,string pwd)
        {
            var re=new ResponseMessage();
            var message=string.Empty;
            
            if (userService.VDUserName(email,out message)&&userService.VDPassWord(pwd,null,out message))
            {
                authService.Register(email,pwd,null,out message);
                re.Ok=true;
            }
            re.Msg=message;
            return Json(re,MyJsonConvert.GetLeanoteOptions());

        }
    
    }
}