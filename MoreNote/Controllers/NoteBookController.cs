using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers
{
    public class NoteBookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }   
        public IActionResult GetNotebooks()
        {
            long userid = 1208692382644703232;
            List<Notebook> noteBoooks= NotebookService.GetNoteBookTree(userid);
            string json = JsonSerializer.Serialize(noteBoooks, MyJsonConvert.GetOptions());
            return Content(json);
        }
    }
}