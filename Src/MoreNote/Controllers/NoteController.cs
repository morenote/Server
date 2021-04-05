using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System.Collections.Generic;
using System.Text.Json;

namespace MoreNote.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin,User")]
    public class NoteController : BaseController
    {
        private NotebookService notebookService;
        private NoteContentService noteContentService;
        private NoteService noteService;
        private ShareService shareService;

        public NoteController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            ,ShareService shareService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor, NotebookService notebookService, NoteService noteService, NoteContentService noteContentService) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.notebookService = notebookService;
            this.noteService = noteService;
            this.noteContentService = noteContentService;
        }

        [Route("Note/{action=Editor}/{noteId?}/")]
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        public IActionResult Editor(string noteId)
        {
            this.SetLocale();//设置区域化信息
            var userInfo = this.GetUserAndBlogUrl();//得到用户信息+博客主页
            //判断用户ID是否已经登录
            var user = this.GetUserBySession();
            var userId=user.UserId;
    
            Notebook[] noteBoooks = notebookService.GetNoteBookTree(user.UserId);

            //是否已经开放注册功能
            ViewBag.openRegister = configFileService.GetWebConfig().SecurityConfig.OpenRegister;
            // 已登录了, 那么得到所有信息
            var notebooks=notebookService.GetNotebooks(userId);
           

            // 还需要按时间排序(DESC)得到notes
            List<Note> notes=new List<Note>();
            NoteContent noteContent=null;
               




            Dictionary<string, string> js = new Dictionary<string, string>();
          
            


            string json = JsonSerializer.Serialize(noteBoooks, MyJsonConvert.GetOptions());
            //json  = System.IO.File.ReadAllText(@"E:\Project\JSON\notebook\GetNotebooks.json");
            //js.Add("notebooks", json);
            ViewBag.notebooks = JsonSerializer.Serialize(noteBoooks, MyJsonConvert.GetOptions());
            SetLocale();
            ViewBag.js = js;
            ViewBag.userInfo = user;
            //页面的值
            ViewBag.isAdmin=configFileService.GetWebConfig().SecurityConfig.AdminUsername.Equals(user.Username);
            
            ViewBag.userInfo=user;
            ViewBag.notebooks=noteBoooks;
            



            //return View("Note-dev");
            return View();
        }

        public IActionResult GetNoteContent(string noteId)
        {
            long? noteNumber = noteId.ToLongByHex();
            long? userNumber = GetUserIdBySession();
            NoteContent noteContent = noteContentService.GetValidNoteContent(noteId.ToLongByHex(), GetUserIdBySession());
            return Json(noteContent, MyJsonConvert.GetOptions());
        }

        public JsonResult ListNotes(string notebookId)
        {
            Note[] notes = noteService.ListNotes(GetUserIdBySession(), notebookId.ToLongByHex(), false);
            //string json = JsonSerializer.Serialize(notes, MyJsonConvert.GetOptions());
            return Json(notes, MyJsonConvert.GetOptions());
        }
    }
}