using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.ModelBinder;
using MoreNote.Common.Utils;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

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
        private IWebHostEnvironment env;
        private TrashService trashService;

        public NoteController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ShareService shareService
            , ConfigFileService configFileService
            , IWebHostEnvironment env
            , TagService tagService
            , TrashService trashService
            , IHttpContextAccessor accessor
            , NotebookService notebookService
            , NoteService noteService
            , NoteContentService noteContentService
            ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.notebookService = notebookService;
            this.noteService = noteService;
            this.noteContentService = noteContentService;
            this.env = env;
            this.tagService = tagService;
            this.trashService = trashService;
        }

      

        [Route("Note/GetNoteContent")]
        [HttpGet]
        public IActionResult GetNoteContent(string noteId)
        {
            long? noteNumber = noteId.ToLongByHex();
            long? userNumber = GetUserIdBySession();
            NoteContent noteContent = noteContentService.GetValidNoteContentByNoteId(noteId.ToLongByHex(), GetUserIdBySession());
            ApiRe falseRe = new ApiRe()
            {
                Ok = false,
                Msg = "GetNoteContent_is_error"
            };
            if (noteContent == null)
            {
                return Json(falseRe, MyJsonConvert.GetLeanoteOptions());
            }
            return Json(noteContent, MyJsonConvert.GetLeanoteOptions());
        }

        [Route("Note/ListNotes")]
        [HttpGet]
        public JsonResult ListNotes(string notebookId)
        {
            Note[] notes = noteService.ListNotes(GetUserIdBySession(), notebookId.ToLongByHex(), false, false);
            //string json = JsonSerializer.Serialize(notes, MyJsonConvert.GetOptions());
            return Json(notes, MyJsonConvert.GetLeanoteOptions());
        }

        /// <summary>
        /// 设置/取消Blog; 置顶
        /// </summary>
        /// <param name="noteIds">笔记的名称</param>
        /// <param name="isBlog">是否设置成博客</param>
        /// <param name="isTop">是否置顶</param>
        /// <returns></returns>
        [Route("Note/SetNote2Blog")]
        [HttpGet]
        public JsonResult SetNote2Blog(string[] noteIds, bool isBlog, bool isTop)
        {
            foreach (var item in noteIds)
            {
                noteService.ToBlog(GetUserIdBySession(), item.ToLongByHex(), isBlog, isTop);
            }
            return Json(true);
        }

        [Route("Note/SetAccessPassword")]
        [HttpGet]
        public JsonResult SetAccessPassword(string[] noteIds, string password)
        {
            foreach (var nodeId in noteIds)
            {
                noteService.SetAccessPassword(GetUserIdBySession(), nodeId.ToLongByHex(), password);
            }
            return Json(true);
        }

        // 这里不能用json, 要用post
        [Route("Note/UpdateNoteOrContent")]
        [HttpPost]
        public async Task<JsonResult> UpdateNoteOrContent([ModelBinder(BinderType = typeof(NoteOrContentModelBinder))] NoteOrContent noteOrContent)
        {
            var userid = GetUserIdBySession();
            var oldNote = noteService.GetNoteById(noteOrContent.NoteId);
            // 新添加note
            if (noteOrContent.IsNew.IsValidTrue() && oldNote == null)
            {
                var userId = GetUserIdBySession();
                // 为共享新建?
                if (noteOrContent.FromUserId != null)
                {
                    userId = noteOrContent.FromUserId;
                }

                //todo:IsBlog.Value 缺陷 空指针异常
                var note = new Note()
                {
                    UserId = userId,
                    Id = noteOrContent.NoteId,
                    NotebookId = noteOrContent.NotebookId,
                    Title = noteOrContent.Title,
                    Src = noteOrContent.Src,
                    Tags = noteOrContent.Tags.ToTagsArray(),
                    Desc = noteOrContent.Desc,
                    ImgSrc = noteOrContent.ImgSrc,
                    IsBlog = noteOrContent.IsBlog.GetValueOrDefault(),
                    IsMarkdown = noteOrContent.IsMarkdown.GetValueOrDefault()
                };
                var noteContent = new NoteContent()
                {
                    UserId = userId,
                    IsBlog = note.IsBlog,
                    Content = noteOrContent.Content,
                    Abstract = noteOrContent.Abstract
                };
                note =  noteService.AddNoteAndContentForController(note, noteContent, userid);
                return Json(note, MyJsonConvert.GetLeanoteOptions());
            }
            var noteUpdate = new Note();
            var needUpdateNote = false;

            if (noteOrContent.Desc.IsValid())
            {
                needUpdateNote = true;
                noteUpdate.Desc = noteOrContent.Desc;
            }
            if (noteOrContent.Title.IsValid())
            {
                needUpdateNote = true;
                noteUpdate.Title = noteOrContent.Title;
            }

            if (noteOrContent.ImgSrc.IsValid())
            {
                needUpdateNote = true;
                noteUpdate.ImgSrc = noteOrContent.ImgSrc;
            }
            if (noteOrContent.Tags.IsValid())
            {
                needUpdateNote = true;
                noteUpdate.Tags = noteOrContent.Tags.ToTagsArray();
            }

            // web端不控制
            if (needUpdateNote)
            {
                noteService.UpdateNote(userid, noteOrContent.NoteId, noteUpdate, -1);
            }

            if (noteOrContent.Content.IsValid())
            {
                noteContentService.UpdateNoteContent(userid, noteOrContent.NoteId, noteOrContent.Content, noteOrContent.Abstract, needUpdateNote, -1, DateTime.Now);
            }
            return Json(true);
        }

        [Route("Note/DeleteNote")]
        [HttpPost,HttpDelete]
        public JsonResult DeleteNote(string[] noteIds, bool isShared)
        {
            if (!isShared)
            {
                foreach (var item in noteIds)
                {
                    trashService.DeleteNote(item.ToLongByHex(), GetUserIdBySession());
                }
                return Json(true);
            }

            foreach (var item in noteIds)
            {
                trashService.DeleteSharedNote(item.ToLongByHex(), GetUserIdBySession());
            }
            return Json(true);
        }

        [Route("Note/moveNote")]
        [HttpPost]
        public IActionResult moveNote(string[] noteIds, string notebookId)
        {
            var userId = GetUserIdBySession();
            foreach (var noteId in noteIds)
            {
                noteService.MoveNote(userId, noteId.ToLongByHex(), notebookId.ToLongByHex());
            }
            return Json(true);
        }

        /// <summary>
        /// 搜索笔记
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Route("Note/SearchNote")]
        [HttpGet]
        public IActionResult SearchNote(string key)
        {
            /**
             * 默认：title搜索
             * 关键词&关键词：title搜索
             * 关键词|关键词：title搜索
             * title:仅搜索标题中的关键词的笔记
             * body：仅搜索文章中的关键词的笔记
             * tag:仅搜索tag列表的关键词的笔记
             * time>YYYMMDD 2021/10/24 搜索指定日期后的笔记
             * time<YYYMMDD 2021/10/24 搜索指定日期前的笔记
             * time<YYYMMDD 2021/10/24 搜索指定日期的笔记
             * file：搜索包含制定附件名称的笔记
             * */
            var userId = this.GetUserIdBySession();

            var notes1 = noteService.SearchNoteByTitleVector(key, userId, GetPage(), pageSize);

            var notes2 = noteService.SearchNoteByContentVector(key, userId, GetPage(), pageSize);

            var result=merge(notes1,notes2);
            return Json(result, MyJsonConvert.GetLeanoteOptions());
        }


        private Note[] merge(Note[] notes1, Note[] notes2)
        {
            Dictionary<long?, Note> result = new Dictionary<long?, Note>(notes1.Length + notes2.Length);
            if (notes1 != null)
            {
                foreach (var item in notes1)
                {
                    if (!result.ContainsKey(item.Id))
                    {
                        result.Add(item.Id, item);
                    }
                }
            }
            if (notes2 != null)
            {
                foreach (var item in notes2)
                {
                    if (!result.ContainsKey(item.Id))
                    {
                        result.Add(item.Id, item);
                    }
                }
            }

            return result.Values.ToArray();
        }

        /// <summary>
        /// 通过tags搜索
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Route("Note/SearchNoteByTags")]
        [HttpGet]
        public IActionResult SearchNoteByTags(string tags)
        {
            var query = Request.Query["tags[]"];
            var userId = this.GetUserIdBySession();
            var notes = noteService.SearchNoteByTag(query, userId, GetPage(), pageSize);
            return Json(notes, MyJsonConvert.GetLeanoteOptions());
        }
    }
}