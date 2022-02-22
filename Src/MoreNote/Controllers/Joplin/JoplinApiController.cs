using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Models.DTO.Joplin;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MoreNote.Controllers.Joplin
{
    public class JoplinApiController : JoplinBaseController
    {
        private AuthService AuthService { get; set; }

        public JoplinApiController(AttachService attachService
          , TokenSerivce tokenSerivce
          , NoteFileService noteFileService
          , UserService userService
          , ConfigFileService configFileService
          , IHttpContextAccessor accessor
            , AuthService authService
         
           , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
            this.AuthService = authService;
        }

        [Route("/api/items/root:/info.json:/content")]
        [HttpGet]
        public IActionResult GetInfoJson()
        {
            JoplinServerInfo joplinServerInfo=new JoplinServerInfo();
            return Json(joplinServerInfo,MyJsonConvert.GetSimpleOptions());
        }

        [Route("/api/items/root:/{**what}")]
        [HttpGet]
        public IActionResult GetItems(string what)
        {
            return Content(what);
        }

        [Route("/api/items/root:/{**what}")]
        [HttpDelete]
        public IActionResult DeleteItems(string what)
        {
            return Content(what);
        }

        [Route("/api/items/root:/{**what}")]
        [HttpPost]
        public IActionResult PostItems(string what)
        {
            return Content(what);
        }

    }
}