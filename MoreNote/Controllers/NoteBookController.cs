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
        public NoteBookController(DependencyInjectionService dependencyInjectionService) : base( dependencyInjectionService)
        {
            this.notebookService=dependencyInjectionService.ServiceProvider.GetService(typeof(NotebookService))as NotebookService;
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