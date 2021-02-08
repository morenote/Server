using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Value;
using System.Collections.Generic;
using System.Text.Json;

namespace MoreNote.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class NoteController : BaseController
    {
        private NotebookService notebookService;
        private NoteContentService noteContentService;
        private NoteService noteService;
        public NoteController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor, NotebookService notebookService, NoteService noteService, NoteContentService noteContentService) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {

            this.notebookService = notebookService;
            this.noteService = noteService;
            this.noteContentService = noteContentService;
        }
      
        public IActionResult Note()
        {
            ViewBag.msg = LanguageResource.GetMsg();
            ViewBag.member = LanguageResource.GetMember();
            ViewBag.markdown = LanguageResource.GetMarkdown();
            ViewBag.blog = LanguageResource.GetBlog();
            Dictionary<string, string> js = new Dictionary<string, string>();
            User user = GetUserBySession();
            Notebook[] noteBoooks = notebookService.GetNoteBookTree(user.UserId);
            string json = JsonSerializer.Serialize(noteBoooks, MyJsonConvert.GetOptions());
            //json  = System.IO.File.ReadAllText(@"E:\Project\JSON\notebook\GetNotebooks.json");
            //js.Add("notebooks", json);
            ViewBag.notebooks= JsonSerializer.Serialize(noteBoooks, MyJsonConvert.GetOptions());
            ViewBag.msg = LanguageResource.GetMsg();
            ViewBag.js = js;
            ViewBag.userInfo = user;
            //return View("Note-dev");
            return View();

        }
        public IActionResult getNoteContent(string noteId)
        {
            long noteNumber = noteId.ToLongByHex();
            long userNumber = GetUserIdBySession();
            NoteContent noteContent = noteContentService.GetValidNoteContent(noteId.ToLongByHex(), GetUserIdBySession());
            return Json(noteContent, MyJsonConvert.GetOptions());
        }
        public JsonResult ListNotes(string notebookId)
        {
            Note[] notes = noteService.ListNotes(GetUserIdBySession(), notebookId.ToLongByHex(), false);
            //string json = JsonSerializer.Serialize(notes, MyJsonConvert.GetOptions());
            return Json(notes,MyJsonConvert.GetOptions());

        }

    }
}