using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers
{
    public class NoteBookController : BaseController
    {
        private NotebookService notebookService;
        public NoteBookController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor, NotebookService notebookService) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.notebookService= notebookService;
            ;


        }
        public IActionResult Index()
        {
            return View();
        }   
        public IActionResult GetNotebooks()
        {
            long userid = 1208692382644703232;
            Notebook[] noteBoooks = notebookService.GetNoteBookTree(userid);
            string json = JsonSerializer.Serialize(noteBoooks, MyJsonConvert.GetOptions());
            return Content(json);
        }
    }
}