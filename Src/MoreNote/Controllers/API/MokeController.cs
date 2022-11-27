using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Logic.Service;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Models.DTO.ng;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API.APIV1
{
    /// <summary>
    /// 返回虚假的数据，仅供开发调试用途
    /// </summary>
    public class MokeController : APIBaseController
    {
        private AttachService attachService;
        private NoteService noteService;
        private TokenSerivce tokenSerivce;
        private NoteContentService noteContentService;
        private NotebookService notebookService;
        private TrashService trashService;
        private IHttpContextAccessor accessor;
        private RepositoryService noteRepositoryService;

        public MokeController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            NoteService noteService,
            NoteContentService noteContentService,
            NotebookService notebookService,
            RepositoryService noteRepositoryService,
            TrashService trashService
           ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.attachService = attachService;
            this.noteService = noteService;
            this.tokenSerivce = tokenSerivce;
            this.noteContentService = noteContentService;
            this.trashService = trashService;
            this.accessor = accessor;
            this.notebookService = notebookService;
            this.noteRepositoryService = noteRepositoryService;


        }
        [Route("/api/chart")]
        [HttpGet]
        public async Task<IActionResult> chart()
        {
            var json=System.IO.File.ReadAllText("MockData/chart.json");
            Response.ContentType = "application/json; charset=utf-8";

            return Content(json);
        }
        [Route("/api/notice")]
        [HttpGet]
        public async Task<IActionResult> notice()
        {
            var json = System.IO.File.ReadAllText("MockData/notice.json");
            Response.ContentType = "application/json; charset=utf-8";

            return Content(json);

        }
        [Route("/api/activities")]
        [HttpGet]
        public async Task<IActionResult> activities()
        {
            var json = System.IO.File.ReadAllText("MockData/activities.json");
            Response.ContentType = "application/json; charset=utf-8";

            return Content(json);

        }
        [Route("/api/list")]
        [HttpGet]
        public async Task<IActionResult> list()
        {
            var json = System.IO.File.ReadAllText("MockData/list.json");
            Response.ContentType = "application/json; charset=utf-8";

            return Content(json);

        }
        [Route("/api/user/current")]
        [HttpGet]
        public async Task<IActionResult> current()
        {
            var json = System.IO.File.ReadAllText("MockData/current.json");
            Response.ContentType = "application/json; charset=utf-8";

            return Content(json);
        }
        ///geo/330000

        [Route("/api/geo/province")]
        [Route("/api/geo/{pid}")]
        [HttpGet]
        public async Task<IActionResult> province()
        {
            var json = System.IO.File.ReadAllText("MockData/current.json");
            Response.ContentType = "application/json; charset=utf-8";

            return Content(json);
        }
    }
        
  
}