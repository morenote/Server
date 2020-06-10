using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Value;
using System.Collections.Generic;
using System.Text.Json;

namespace MoreNote.Controllers
{
    public class NoteController : BaseController
    {
        public NoteController(IHttpContextAccessor accessor) : base(accessor)
        {


        }
      
        public IActionResult Note()
        {
            ViewBag.msg = LanguageResource.GetMsg();
            Dictionary<string, string> js = new Dictionary<string, string>();
            User user = GetUserBySession();
            Notebook[] noteBoooks = NotebookService.GetNoteBookTree(user.UserId);
            string json = JsonSerializer.Serialize(noteBoooks, MyJsonConvert.GetOptions());
            //json  = System.IO.File.ReadAllText(@"E:\Project\JSON\notebook\GetNotebooks.json");
            //js.Add("notebooks", json);
            ViewBag.notebooks= JsonSerializer.Serialize(noteBoooks, MyJsonConvert.GetOptions());
            ViewBag.js = js;
            return View("Note-dev");
          
        }
        public IActionResult getNoteContent(string noteId)
        {
            long noteNumber = MyConvert.HexToLong(noteId);
            long userNumber = GetUserIdBySession();
            NoteContent noteContent = NoteContentService.GetNoteContent(MyConvert.HexToLong(noteId), GetUserIdBySession());
            return Json(noteContent, MyJsonConvert.GetOptions());
        }
        public JsonResult ListNotes(string notebookId)
        {
            Note[] notes = NoteService.ListNotes(GetUserIdBySession(), MyConvert.HexToLong(notebookId), false);
            //string json = JsonSerializer.Serialize(notes, MyJsonConvert.GetOptions());
            return Json(notes,MyJsonConvert.GetOptions());

        }

    }
}