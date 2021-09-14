using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.Controllers.Admin
{
    public class AdminController : AdminBaseController
    {
        public AdminController(AttachService attachService
              , TokenSerivce tokenSerivce
              , NoteFileService noteFileService
              , UserService userService
              , ConfigFileService configFileService
                    , IHttpContextAccessor accessor
              ) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
        }

        public IActionResult Index()
        {
            return View("Views/admin/index.cshtml");
        }
    }
}