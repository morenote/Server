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
    
    public class MokeController : APIBaseController
    {
        private AttachService attachService;
        private NoteService noteService;
        private TokenSerivce tokenSerivce;
        private NoteContentService noteContentService;
        private NotebookService notebookService;
        private TrashService trashService;
        private IHttpContextAccessor accessor;
        private NoteRepositoryService noteRepositoryService;

        public MokeController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            NoteService noteService,
            NoteContentService noteContentService,
            NotebookService notebookService,
            NoteRepositoryService noteRepositoryService,
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
        [Route("/chart")]
        public async Task chart()
        {
            var json=System.IO.File.ReadAllText("MockData/chart.json");
            await Response.WriteAsJsonAsync(json);
            await Response.CompleteAsync();
        }
        [Route("/api/notice")]
        public async Task notice()
        {
            var json = System.IO.File.ReadAllText("MockData/notice.json");
            await Response.WriteAsync(json);
            Response.ContentType = "application/json; charset=utf-8";
            await Response.CompleteAsync();

        }
        [Route("/api/activities")]
        public async Task activities()
        {
            var json = System.IO.File.ReadAllText("MockData/activities.json");
            await Response.WriteAsync(json);
            Response.ContentType = "application/json; charset=utf-8";
            await Response.CompleteAsync();

        }
        [Route("/api/list")]
        public async Task list()
        {
            var json = System.IO.File.ReadAllText("MockData/list.json");
            await Response.WriteAsync(json);
            Response.ContentType = "application/json; charset=utf-8";
            await Response.CompleteAsync();

        }
        [Route("/user/current")]
        public async Task current()
        {
            var json = System.IO.File.ReadAllText("MockData/current.json");
            await Response.WriteAsync(json);
            Response.ContentType = "application/json; charset=utf-8";
            await Response.CompleteAsync();
        }
    }
        
  
}