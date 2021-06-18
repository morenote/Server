using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System.Collections.Generic;
using System.Linq;
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
        private TagService tagService;
        IWebHostEnvironment env;
        public NoteController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            ,ShareService shareService
            , ConfigFileService configFileService
            ,IWebHostEnvironment env
            ,TagService tagService
            , IHttpContextAccessor accessor, NotebookService notebookService, NoteService noteService, NoteContentService noteContentService) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.notebookService = notebookService;
            this.noteService = noteService;
            this.noteContentService = noteContentService;
            this.env=env;
            this.tagService=tagService;
        }

        [Route("Note/{action=Editor}/{noteIdHex?}/")]
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        public IActionResult Editor(string noteIdHex)
        {
            this.SetLocale();//设置区域化信息
            var userInfo = this.GetUserAndBlogUrl();//得到用户信息+博客主页
            //判断用户ID是否已经登录
            var user = this.GetUserBySession();
            var userId=user.UserId;
    
            Notebook[] noteBoooks = notebookService.GetNoteBookTree(user.UserId);

            //是否已经开放注册功能
            ViewBag.openRegister = configFileService.WebConfig.SecurityConfig.OpenRegister;
            // 已登录了, 那么得到所有信息
            var notebooks=notebookService.GetNotebooks(userId);
            
           
           //获得共享的笔记


            // 还需要按时间排序(DESC)得到notes
            List<Note> notes=new List<Note>();
            NoteContent noteContent=null;

            if (!notebooks.IsNullOrNothing())
            {
                // noteId是否存在
		        // 是否传入了正确的noteId
                var   hasRightNoteId = false;

                long? noteId=noteIdHex.ToLongByHex();
                if (noteIdHex!=null)
                {
                    //说明ID本身是有效的
                    var note=noteService.GetNoteById(noteId);
                    if (note!=null)
                    {
                        var noteOwner=note.UserId;
                        noteContent=noteContentService.GetNoteContent(noteId,noteOwner,false);
                       
                        hasRightNoteId=true;
                        ViewBag.curNoteId=noteId;
                        ViewBag.curNotebookId=note.NotebookId.ToHex24();

                        // 打开的是共享的笔记, 那么判断是否是共享给我的默认笔记
                        if (noteOwner!=GetUserIdBySession())
                        {
                            if (shareService.HasReadPerm(noteOwner,GetUserIdBySession(),noteId))
                            {
                                ViewBag.curSharedNoteNotebookId=note.NotebookId.ToHex24();
                                ViewBag.curSharedUserId=noteOwner;
                            }
                            else
                            {
                                hasRightNoteId = false;
                            }

                        }
                        else
                        {
                            
                             notes=  noteService.ListNotes(this.GetUserIdBySession(),note.NotebookId,false,GetPage(),50, defaultSortField, false,false);
                            // 如果指定了某笔记, 则该笔记放在首位
         
                             notes.Insert(0, note);

                        }



                    }
                    //获得最近的笔记
                    int count2 = 0;
                    var latestNotes = noteService.ListNotes(userId, null, false, GetPage(), 50, defaultSortField, false, false);


                }
                // 没有传入笔记
                // 那么得到最新笔记
                if (!hasRightNoteId)
                {
                    notes=noteService.ListNotes(userId,null,false,GetPage(),50,defaultSortField,false,false);
                    if (notes.Any())
                    {
                        noteContent=noteContentService.GetValidNoteContent(notes[0].NoteId,userId);
                        ViewBag.curNoteId=notes[0].NoteId;

                    }

                }

            }

            ViewBag.isAdmin=user.Username.Equals(config.SecurityConfig.AdminUsername);
            ViewBag.IsDevelopment = config.APPConfig.Dev;


            ViewBag.userInfo = userInfo;
            ViewBag.notebooks= notebooks;
            //ViewBag.shareNotebooks= shareNotebooks;


            ViewBag.notes= notes;
            ViewBag.noteContentJson= noteContent;
            ViewBag.noteContent = noteContent.Content;

            ViewBag.tags=tagService.GetTags(userId);
            ViewBag.config=config;

            Dictionary<string, string> js = new Dictionary<string, string>();
         

            string json = JsonSerializer.Serialize(noteBoooks, MyJsonConvert.GetOptions());
            //json  = System.IO.File.ReadAllText(@"E:\Project\JSON\notebook\GetNotebooks.json");
            //js.Add("notebooks", json);
            //ViewBag.notebooks = JsonSerializer.Serialize(notebooks, MyJsonConvert.GetOptions());
            SetLocale();
            ViewBag.js = js;
           
            //页面的值
            ViewBag.isAdmin=configFileService.WebConfig.SecurityConfig.AdminUsername.Equals(user.Username);
            
            ViewBag.userInfo=user;
           
            
            ViewBag.OpenRegister=config.SecurityConfig.OpenRegister;

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