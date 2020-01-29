using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Value;

namespace MoreNote.Controllers
{
    public class NoteController : Controller
    {
        public IActionResult Note()
        {
            ViewBag.msg = LanguageResource.GetMsg();
            Dictionary<string,string> js = new Dictionary<string, string>();
            long userid = 1208692382644703232;
            Notebook[] noteBoooks = NotebookService.GetNoteBookTree(userid);
            string json = JsonSerializer.Serialize(noteBoooks, MyJsonConvert.GetOptions());
            //json  = System.IO.File.ReadAllText(@"E:\Project\JSON\notebook\GetNotebooks.json");
            js.Add("notebooks", json);
            ViewBag.js = js;
            return View();
        }
        public IActionResult getNoteContent(string noteId)
        {
            string a = System.IO.File.ReadAllText("TextFile.txt");
            var options= new System.Text.Json.JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            //NoteContent noteContent1 = JsonSerializer.Deserialize<NoteContent>(a, options);
            //NoteService.InsertNoteContent(noteContent1);
            NoteContent noteContent = NoteContentService.SelectNoteContent(123123);
            string json = JsonSerializer.Serialize(noteContent, options);

            // return Content(a);

            return Content(json);
        }
        public IActionResult ListNotes(string notebookId)
        {
            Note[] notes =    NoteService.ListNotes(1208692382644703232, 1208692382640508928, false, 1, 1, "defaultSortField", false, false);
            string json= JsonSerializer.Serialize(notes, MyJsonConvert.GetOptions());
            return Content(json);

        }




        }
}