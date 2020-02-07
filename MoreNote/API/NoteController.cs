using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System;

namespace MoreNote.API
{
    [Route("api/[controller]/[action]")]
    // [ApiController]
    public class NoteController : ApiBaseController
    {
        public NoteController(IHttpContextAccessor accessor) : base(accessor)
        {
        }

        //todo:获取同步的笔记
        //public JsonResult GetSyncNotes([ModelBinder(BinderType = typeof(Hex2LongModelBinder))]long userId,int afterUsn,int maxEntry,string token)
        //{
        //    if (maxEntry==0) maxEntry=100;
        //    ApiNote[] apiNotes=NoteService.GetSyncNotes(userId,afterUsn,maxEntry);
        //    return Json(apiNotes,MyJsonConvert.GetOptions());
        //}
        public JsonResult GetSyncNotes(int afterUsn, int maxEntry, string token)
        {
            if (maxEntry == 0) maxEntry = 100;
            ApiNote[] apiNotes = NoteService.GetSyncNotes(getUserIdByToken(token), afterUsn, maxEntry);
            return Json(apiNotes, MyJsonConvert.GetOptions());
        }
        //todo:得到笔记本下的笔记
        public IActionResult GetNotes(string notebookId, string token)
        {
            Note[] notes = NoteService.ListNotes(getUserIdByToken(token), MyConvert.HexToLong(notebookId), false);
            long myNotebookId = MyConvert.HexToLong(notebookId);
            return null;
        }
        //todo:得到trash
        public IActionResult GetTrashNotes()
        {
            return null;
        }
        //todo:获取笔记
        public IActionResult GetNote()
        {
            return null;
        }
        //todo:得到note和内容
        public IActionResult GetNoteAndContent(string token, string noteId)
        {
            NoteAndContent noteAndContent = NoteService.GetNoteAndContent(MyConvert.HexToLong(noteId), getUserIdByToken());

            if (noteAndContent == null)
            {
                return Json(new ApiRe() { Ok = false, Msg = "" }, MyJsonConvert.GetOptions());
            }
            else
            {
                return Json(new ApiRe()
                {
                    Ok = false,
                    Msg = "GetNoteAndContent_is_error"
                }); ;
            }

        }
        //todo:格式化URL

        //todo:得到内容
        public IActionResult GetNoteContent(string token, string noteId)
        {
            Note note = NoteService.GetNote(MyConvert.HexToLong(noteId), getUserIdByToken(token));
            NoteContent noteContent = NoteContentService.GetNoteContent(MyConvert.HexToLong(noteId), getUserIdByToken(token));
            if (noteContent != null && !string.IsNullOrEmpty(noteContent.Content))
            {

                noteContent.Content = NoteService.FixContent(noteContent.Content, note.IsMarkdown);
            }
            ApiNoteContent apiNote = new ApiNoteContent()
            {
                NoteId = note.NoteId,
                UserId = note.UserId,
                Content = noteContent.Content
            };
            ApiRe falseRe = new ApiRe()
            {
                Ok = false,
                Msg = "GetNoteContent_is_error"
            };
            return Json(apiNote, MyJsonConvert.GetOptions());

        }
        //todo:添加笔记
        public JsonResult AddNote(ApiNote noteOrContent, string token)
        {

            var x = _accessor.HttpContext.Request.Form.Files;
            var z = x["FileDatas[5e36bafc26f2af1a79000000]"];
            //json 返回状态好乱呀 /(ㄒoㄒ)/~~
            Re re = Re.NewRe();
            long userId = getUserIdByToken(token); ;
            long myUserId = userId;
            if (noteOrContent == null || string.IsNullOrEmpty(noteOrContent.NotebookId))
            {
                return Json(new ApiRe() { Ok = false, Msg = "notebookIdNotExists" }, MyJsonConvert.GetSimpleOptions());
            }
            long noteId = MyConvert.HexToLong(noteOrContent.NoteId);
            if (noteId == 0)
            {
                noteId = SnowFlake_Net.GenerateSnowFlakeID();
            }
            // TODO 先上传图片/附件, 如果不成功, 则返回false
            //
            int attachNum = 0;

            fixPostNotecontent(ref noteOrContent);
            Note note = new Note()
            {
                UserId = userId,
                NoteId = noteId,
                NotebookId = MyConvert.HexToLong(noteOrContent.NotebookId),
                Title = noteOrContent.Title,
                Tags = noteOrContent.Tags,
                Desc = noteOrContent.Desc,
                IsBlog = noteOrContent.IsBlog.GetValueOrDefault(),
                IsMarkdown = noteOrContent.IsMarkdown,
                AttachNum = attachNum,
                CreatedTime = noteOrContent.CreatedTime,
                UpdatedTime = noteOrContent.UpdatedTime,
            };
            NoteContent noteContent = new NoteContent()
            {
                NoteContentId = SnowFlake_Net.GenerateSnowFlakeID(),
                NoteId = noteId,
                UserId = userId,
                IsBlog = note.IsBlog,
                Content = noteOrContent.Content,
                Abstract = noteOrContent.Abstract,
                CreatedTime = noteOrContent.CreatedTime,
                UpdatedTime = noteOrContent.UpdatedTime,
            };
            // 通过内容得到Desc, abstract
            if (string.IsNullOrEmpty(noteOrContent.Abstract))
            {
                note.Desc = MyHtmlHelper.SubStringHTMLToRaw(noteContent.Content, 200);
                noteContent.Abstract = MyHtmlHelper.SubStringHTML(noteContent.Content, 200, "");
            }
            else
            {
                note.Desc = MyHtmlHelper.SubStringHTMLToRaw(noteContent.Abstract, 200);
            }
            note = NoteService.AddNoteAndContent(note, noteContent, myUserId);

            if (note == null || note.NoteId == 0)
            {
                return Json(new ApiRe()
                {
                    Ok = false,
                    Msg = "AddNoteAndContent_is_error"
                });
            }
            if (noteOrContent.Files != null && noteOrContent.Files.Length > 0)
            {
                for (int i = 0; i < noteOrContent.Files.Length; i++)
                {
                    var file = noteOrContent.Files[i];
                    if (file.HasBody)
                    {
                        if (!string.IsNullOrEmpty(file.LocalFileId))
                        {
                            var result = upload("FileDatas[" + file.LocalFileId + "]", userId, noteId, file.IsAttach, out long serverFileId, out string msg);
                            if (!result)
                            {
                                if (string.IsNullOrEmpty(msg))
                                {
                                    re.Msg = "fileUploadError";
                                }
                                else
                                {
                                    re.Msg = msg;
                                    return Json(re, MyJsonConvert.GetOptions());
                                }
                            }
                            else
                            {
                                // 建立映射
                                file.FileId = serverFileId.ToString("x");
                                noteOrContent.Files[i] = file;
                                if (file.IsAttach)
                                {
                                    attachNum++;
                                }
                            }
                        }
                        else
                        {   //存在疑问
                            return Json(new ReUpdate()
                            {
                                Ok = false,
                                Msg = "LocalFileId_Is_NullOrEmpty",
                                Usn = 0
                            }, MyJsonConvert.GetSimpleOptions());
                        }
                    }
                }
            }
            // 移到外面来, 删除最后一个file时也要处理, 不然总删不掉
            // 附件问题, 根据Files, 有些要删除的, 只留下这些
            AttachService.UpdateOrDeleteAttachApi(noteId, userId, noteOrContent.Files);
            if (noteOrContent.Desc != null)
            {


            }
            //添加需要返回的
            noteOrContent.NoteId = note.NoteId.ToString("x");
            noteOrContent.Usn = note.Usn;
            noteOrContent.CreatedTime = note.CreatedTime;
            noteOrContent.UpdatedTime = note.UpdatedTime;
            noteOrContent.UserId = getUserIdByToken(token).ToString("x");
            noteOrContent.IsMarkdown = note.IsMarkdown;
            // 删除一些不要返回的, 删除Desc?
            noteOrContent.Content = "";
            noteOrContent.Abstract = "";
            //	apiNote := info.NoteToApiNote(note, noteOrContent.Files)

            return Json(noteOrContent, MyJsonConvert.GetOptions());
        }
        //todo:更新笔记
        public JsonResult UpdateNote(ApiNote noteOrContent, string token)
        {
            var noteUpdate = new Note();
            var needUpdateNote = false;
            var re = new ReUpdate();
            long userId = getUserIdByToken(token);
            var noteId = MyConvert.HexToLong(noteOrContent.NoteId);
            if (userId == 0)
            {
                re.Msg = "NOlogin";
                re.Ok = false;
                return Json(re, MyJsonConvert.GetSimpleOptions());

            }

            if (string.IsNullOrEmpty(noteOrContent.NoteId))
            {
                re.Msg = "noteIdNotExists";
                re.Ok = false;
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }

            if (noteOrContent.Usn < 1)
            {

                re.Msg = "usnNotExists";
                re.Ok = false;
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            // 先判断USN的问题, 因为很可能添加完附件后, 会有USN冲突, 这时附件就添错了
            var note = NoteService.GetNote(noteId, userId);
            if (note == null || note.NoteId == 0)
            {
                re.Msg = "notExists";
                re.Ok = false;
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            if (note.Usn != noteOrContent.Usn)
            {
                re.Msg = "conflict";
                re.Ok = false;
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            if (noteOrContent.Files != null && noteOrContent.Files.Length > 0)
            {
                for (int i = 0; i < noteOrContent.Files.Length; i++)
                {
                    var file = noteOrContent.Files[i];
                    if (file.HasBody)
                    {
                        if (!string.IsNullOrEmpty(file.LocalFileId))
                        {
                            var result = upload("FileDatas[" + file.LocalFileId + "]", userId, noteId, file.IsAttach, out long serverFileId, out string msg);
                            if (!result)
                            {
                                if (string.IsNullOrEmpty(msg))
                                {
                                    re.Msg = "fileUploadError";
                                }
                                if (!string.Equals(msg, "notImage", System.StringComparison.OrdinalIgnoreCase))
                                {
                                    return Json(re, MyJsonConvert.GetOptions());
                                }
                            }
                            else
                            {
                                // 建立映射
                                file.FileId = serverFileId.ToString("x");
                                noteOrContent.Files[i] = file;

                            }
                        }
                        else
                        {
                            return Json(new ReUpdate()
                            {
                                Ok = false,
                                Msg = "LocalFileId_Is_NullOrEmpty",
                                Usn = 0
                            }, MyJsonConvert.GetSimpleOptions());
                        }
                    }
                }
            }
            //DESC
            if (noteOrContent.Desc != null)
            {
                needUpdateNote = true;
                noteUpdate.Desc = noteOrContent.Desc;
            }
            if (noteOrContent.Title != null)
            {
                needUpdateNote = true;
                noteUpdate.Desc = noteOrContent.Title;
            }
            if (noteOrContent.IsTrash != null)
            {
                needUpdateNote = true;
                noteUpdate.IsTrash = noteOrContent.IsTrash.GetValueOrDefault();
            }
            if (noteOrContent.IsBlog != null)
            {
                needUpdateNote = true;
                noteUpdate.IsBlog = noteOrContent.IsBlog.GetValueOrDefault();
            }
            if (noteOrContent.Tags != null)
            {
                needUpdateNote = true;
                noteUpdate.Tags = noteOrContent.Tags;
            }
            if (noteOrContent.NotebookId != null)
            {
                needUpdateNote = true;
                long id = MyConvert.HexToLong(noteOrContent.NotebookId);
              
                noteUpdate.NotebookId = id;
            }
            if (noteOrContent.Content != null)
            {
                needUpdateNote = true;
                if (noteOrContent.Abstract == null)
                {
                    noteUpdate.Desc = MyHtmlHelper.SubStringHTMLToRaw(noteOrContent.Content, 200);

                }
                else
                {
                    noteUpdate.Desc = MyHtmlHelper.SubStringHTMLToRaw(noteOrContent.Abstract, 200);
                }
            }
            if (noteOrContent.UpdatedTime != null)
            {
                needUpdateNote = true;
                noteUpdate.UpdatedTime = noteOrContent.UpdatedTime;
            }


            int afterNoteUsn = 0;
            var noteOk = false;
            var noteMsg = "";
            if (needUpdateNote)
            {
                
                noteOk = NoteService.UpdateNote(userId, noteId, noteUpdate, noteOrContent.Usn, out afterNoteUsn, out noteMsg);
                if (!noteOk)
                {
                    re.Ok = false;
                    re.Msg = noteMsg;
                    return Json(re, MyJsonConvert.GetOptions());
                }
            }
            //-------------更新笔记内容
            var afterContentUsn = 0;
            var contentOk = false;
            var contentMsg = "";
            if (noteOrContent.Content != null)
            {
                // 把fileId替换下
                fixPostNotecontent(ref noteOrContent);
                // 如果传了Abstract就用之
                if (noteOrContent.Abstract != null)
                {
                    noteOrContent.Abstract = MyHtmlHelper.SubStringHTML(noteOrContent.Content, 200, "");
                }
                contentOk = NoteContentService.UpdateNoteContent(
                    userId,
                    noteId,
                    noteOrContent.Content,
                    noteOrContent.Abstract,
                    needUpdateNote,
                    noteOrContent.Usn,
                    noteOrContent.UpdatedTime,
                    out contentMsg,
                    out afterContentUsn);
                if (needUpdateNote)
                {
                    re.Ok = noteOk;
                    re.Msg = noteMsg;
                    re.Usn = afterNoteUsn;

                }
                else
                {
                    re.Ok = contentOk;
                    re.Msg = contentMsg;
                    re.Usn = afterContentUsn;
                }
                if (!re.Ok)
                {
                    return Json(re, MyJsonConvert.GetSimpleOptions());
                }
                noteOrContent.Content = "";
                noteOrContent.Usn = re.Usn;
                noteOrContent.UpdatedTime = DateTime.Now;
                noteOrContent.UserId=userId.ToString("x");
                return Json(noteOrContent,MyJsonConvert.GetOptions());

            }

            return null;
        }
        //todo:删除trash
        public IActionResult DeleteTrash(string noteId, int usn, string token)
        {
            bool result = TrashService.DeleteTrashApi(MyConvert.HexToLong(noteId), getUserIdByToken(token), usn, out string msg, out int afterUsn);
            return null;
        }
        //todo:导出成PDF
        public IActionResult ExportPdf()
        {
            return null;
        }
        // content里的image, attach链接是
        // https://leanote.com/api/file/getImage?fileId=xx
        // https://leanote.com/api/file/getAttach?fileId=xx
        // 将fileId=映射成ServerFileId, 这里的fileId可能是本地的FileId
        public void fixPostNotecontent(ref ApiNote noteOrContent)
        {
            //todo 这里需要完成fixPostNotecontent
            if (noteOrContent == null || string.IsNullOrEmpty(noteOrContent.Content))
            {
                return;
            }
            APINoteFile[] files = noteOrContent.Files;
            if (files != null && files.Length > 0)
            {

            }
        }
    }
}