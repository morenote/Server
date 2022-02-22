using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

namespace MoreNote.Controllers
{
    public class ErrorsController1 : BaseController
    {
        public ErrorsController1(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor
            , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
           

        }
        public IActionResult Unauthorized1()
        {
            return Unauthorized();
            
        }

    }
}
