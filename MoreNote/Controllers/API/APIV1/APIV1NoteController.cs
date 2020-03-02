using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System;
using System.Text.RegularExpressions;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Note/[action]")]
    // [ApiController]
    public class ApiV1NoteController : ApiV1BaseController
    {
        public ApiV1NoteController(IHttpContextAccessor accessor) : base(accessor)
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
            User tokenUser = TokenSerivce.GetUserByToken(token);
            if (tokenUser == null)
            {
                return Json(new ApiRe() { Ok = false, Msg = "" }, MyJsonConvert.GetOptions());
            }
            NoteAndContent noteAndContent = NoteService.GetNoteAndContent(MyConvert.HexToLong(noteId), tokenUser.UserId, false,false,false);
            ApiNote[] apiNotes = NoteService.ToApiNotes(new Note[] { noteAndContent.note });
            ApiNote apiNote = apiNotes[0];
            apiNote.Content =NoteService.FixContent(noteAndContent.noteContent.Content, noteAndContent.note.IsMarkdown);
            apiNote.Desc = noteAndContent.note.Desc;
            apiNote.Abstract = noteAndContent.noteContent.Abstract;
            if (noteAndContent == null)
            {
                return Json(new ApiRe() { Ok = false, Msg = "" }, MyJsonConvert.GetOptions());
            }
            else
            {
                return Json(apiNote, MyJsonConvert.GetOptions());
            }

        }
        //todo:格式化URL

        //todo:得到内容
        public IActionResult GetNoteContent(string token, string noteId)
        {
            ApiRe falseRe = new ApiRe()
            {
                Ok = false,
                Msg = "GetNoteContent_is_error"
            };
            Note note = NoteService.GetNote(MyConvert.HexToLong(noteId), getUserIdByToken(token));
            NoteContent noteContent = NoteContentService.GetNoteContent(MyConvert.HexToLong(noteId), getUserIdByToken(token));
            if (noteContent==null||note==null)
            {
             
                return Json(falseRe, MyJsonConvert.GetOptions());

            }
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
           
            return Json(apiNote, MyJsonConvert.GetOptions());

        }
        //todo:添加笔记
        public JsonResult AddNote(ApiNote noteOrContent, string token)
        {

            var x = _accessor.HttpContext.Request.Form.Files;
            var z = x["FileDatas[5e36bafc26f2af1a79000000]"];
            //json 返回状态好乱呀 /(ㄒoㄒ)/~~
            Re re = Re.NewRe();
            long tokenUserId = getUserIdByToken(token); ;
            long myUserId = tokenUserId;
            if (noteOrContent == null || string.IsNullOrEmpty(noteOrContent.NotebookId))
            {
                return Json(new ApiRe() { Ok = false, Msg = "notebookIdNotExists" }, MyJsonConvert.GetSimpleOptions());
            }
            long noteId = SnowFlake_Net.GenerateSnowFlakeID();
          
       
            if (noteOrContent.Title==null)
            {
                noteOrContent.Title="无标题";
            }

            // TODO 先上传图片/附件, 如果不成功, 则返回false
            //-------------新增文件和附件内容
            int attachNum = 0;
            if (noteOrContent.Files != null && noteOrContent.Files.Length > 0)
            {
                for (int i = 0; i < noteOrContent.Files.Length; i++)
                {
                    var file = noteOrContent.Files[i];
                    if (file.HasBody)
                    {
                        if (!string.IsNullOrEmpty(file.LocalFileId))
                        {
                            var result = upload("FileDatas[" + file.LocalFileId + "]", tokenUserId, noteId, file.IsAttach, out long serverFileId, out string msg);
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
            else{
                
            }
            //-------------替换笔记内容中的文件ID
            FixPostNotecontent(ref noteOrContent);
            if (noteOrContent.Tags!=null)
            {
                if (noteOrContent.Tags.Length>0&&noteOrContent.Tags[0]==null)
                {
                    //noteOrContent.Tags= Array.Empty<string>();
                    noteOrContent.Tags= new string[] { ""};
                }

            }
            //-------------新增笔记对象
            Note note = new Note()
            {
                UserId = tokenUserId,
                NoteId = noteId,
                CreatedUserId=noteId,
                UpdatedUserId=noteId,
                NotebookId = MyConvert.HexToLong(noteOrContent.NotebookId),
                Title = noteOrContent.Title,
                Tags = noteOrContent.Tags,
                Desc = noteOrContent.Desc,
                IsBlog = noteOrContent.IsBlog.GetValueOrDefault(),
                IsMarkdown = noteOrContent.IsMarkdown.GetValueOrDefault(),
                AttachNum = attachNum,
                CreatedTime = noteOrContent.CreatedTime,
                UpdatedTime = noteOrContent.UpdatedTime,
                ContentId = SnowFlake_Net.GenerateSnowFlakeID()
            };

            //-------------新增笔记内容对象
            NoteContent noteContent = new NoteContent()
            {
                NoteContentId = note.ContentId,
                NoteId = noteId,
                UserId = tokenUserId,
                IsBlog = note.IsBlog,
                Content = noteOrContent.Content,
                Abstract = noteOrContent.Abstract,
                CreatedTime = noteOrContent.CreatedTime,
                UpdatedTime = noteOrContent.UpdatedTime,
                IsHistory = false

            };
            //-------------得到Desc, abstract
            if (string.IsNullOrEmpty(noteOrContent.Abstract))
            {

                if (noteOrContent.IsMarkdown.GetValueOrDefault())
                {
                   // note.Desc = MyHtmlHelper.SubMarkDownToRaw(noteOrContent.Content, 200);
                    noteContent.Abstract = MyHtmlHelper.SubMarkDownToRaw(noteOrContent.Content, 200);

                }
                else
                {
                    //note.Desc = MyHtmlHelper.SubHTMLToRaw(noteOrContent.Content, 200);
                    noteContent.Abstract = MyHtmlHelper.SubHTMLToRaw(noteOrContent.Content, 200);
                }
               
            }
            else
            {
                note.Desc = MyHtmlHelper.SubHTMLToRaw(noteOrContent.Abstract, 200);
            }
            if (noteOrContent.Desc==null)
            {
                if (noteOrContent.IsMarkdown.GetValueOrDefault())
                {
                    note.Desc = MyHtmlHelper.SubMarkDownToRaw(noteOrContent.Content, 200);
                    

                }
                else
                {
                    note.Desc = MyHtmlHelper.SubHTMLToRaw(noteOrContent.Content, 200);
                   
                }
            }
            else
            {
                note.Desc = noteOrContent.Desc;
            }
            
            note = NoteService.AddNoteAndContent(note, noteContent, myUserId);
            //-------------将笔记与笔记内容保存到数据库
            if (note == null || note.NoteId == 0)
            {
                return Json(new ApiRe()
                {
                    Ok = false,
                    Msg = "AddNoteAndContent_is_error"
                });
            }
            //-------------API返回客户端信息
            noteOrContent.NoteId = noteId.ToString("x");
            noteOrContent.UserId = tokenUserId.ToString("x");
            noteOrContent. Title = note.Title;
            noteOrContent.Tags = note.Tags;
            noteOrContent.IsMarkdown = note.IsMarkdown;
            noteOrContent.IsBlog = note.IsBlog;
            noteOrContent.IsTrash = note.IsTrash;
            noteOrContent.IsDeleted = note.IsDeleted;
            noteOrContent.IsTrash = note.IsTrash;
            noteOrContent.IsTrash = note.IsTrash;
            noteOrContent.Usn = note.Usn;
            noteOrContent.CreatedTime = note.CreatedTime;
            noteOrContent.UpdatedTime = note.UpdatedTime;
            noteOrContent.PublicTime = note.PublicTime;
            //Files = files

            //------------- 删除API中不需要返回的内容
            noteOrContent.Content = "";
            noteOrContent.Abstract = "";
            //	apiNote := info.NoteToApiNote(note, noteOrContent.Files)

            return Json(noteOrContent, MyJsonConvert.GetOptions());
        }
        //todo:更新笔记
        public JsonResult UpdateNote(ApiNote noteOrContent, string token)
        {
            Note noteUpdate = new Note();
            var needUpdateNote = false;
            var re = new ReUpdate();
            long tokenUserId = getUserIdByToken(token);
            var noteId = MyConvert.HexToLong(noteOrContent.NoteId);
            //-------------校验参数合法性
            if (tokenUserId == 0)
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
            var note = NoteService.GetNote(noteId, tokenUserId);
            var noteContent = NoteContentService.GetNoteContent(note.NoteId, tokenUserId,false);
            if (note == null || note.NoteId == 0)
            {
                re.Msg = "notExists";
                re.Ok = false;
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            //判断服务器版本与客户端版本是否一致
            if (note.Usn != noteOrContent.Usn)
            {
                re.Msg = "conflict";
                re.Ok = false;
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            //-------------更新文件和附件内容
            if (noteOrContent.Files != null && noteOrContent.Files.Length > 0)
            {
                for (int i = 0; i < noteOrContent.Files.Length; i++)
                {
                    var file = noteOrContent.Files[i];
                    if (file.HasBody)
                    {
                        if (!string.IsNullOrEmpty(file.LocalFileId))
                        {
                            var result = upload("FileDatas[" + file.LocalFileId + "]", tokenUserId, noteId, file.IsAttach, out long serverFileId, out string msg);
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
            //更新用户元数据
            //int usn = UserService.IncrUsn(tokenUserId);

            // 移到外面来, 删除最后一个file时也要处理, 不然总删不掉
            // 附件问题, 根据Files, 有些要删除的, 只留下这些
            if (noteOrContent.Files != null)
            {
                AttachService.UpdateOrDeleteAttachApi(noteId, tokenUserId, noteOrContent.Files);
            }
            //-------------更新笔记内容
            var afterContentUsn = 0;
            var contentOk = false;
            var contentMsg = "";
            long contentId = 0;
            if (noteOrContent.Content != null)
            {
                // 把fileId替换下
                FixPostNotecontent(ref noteOrContent);
                // 如果传了Abstract就用之
                if (noteOrContent.Abstract != null)
                {
                    noteOrContent.Abstract = MyHtmlHelper.SubHTMLToRaw(noteOrContent.Abstract, 200);
                }
                else
                {
                    noteOrContent.Abstract = MyHtmlHelper.SubHTMLToRaw(noteOrContent.Content, 200);
                }
            }
            else
            {
                noteOrContent.Abstract = MyHtmlHelper.SubHTMLToRaw(noteContent.Content, 200);
            }
            //上传noteContent的变更
            contentOk = NoteContentService.UpdateNoteContent(
                 noteOrContent,
                 out contentMsg,
                 out contentId
                 );
            //返回处理结果
            if (!contentOk)
            {
                re.Ok = false;
                re.Msg = contentMsg;
                re.Usn = afterContentUsn;
                return Json(re, MyJsonConvert.GetOptions());
            }

            //-------------更新笔记元数据
            int afterNoteUsn = 0;
            var noteOk = false;
            var noteMsg = "";

            noteOk = NoteService.UpdateNote(
           ref noteOrContent,
           tokenUserId,
           contentId,
           true,
           true,
           out noteMsg,
           out afterNoteUsn
               );
            if (!noteOk)
            {
                re.Ok = false;
                re.Msg = noteMsg;
                return Json(re, MyJsonConvert.GetOptions());
            }
            //处理结果
            //-------------API返回客户端信息
            note = NoteService.GetNote(noteId, tokenUserId);
           // noteOrContent.NoteId = noteId.ToString("x");
           // noteOrContent.UserId = tokenUserId.ToString("x");
          //  noteOrContent.Title = note.Title;
           // noteOrContent.Tags = note.Tags;
           // noteOrContent.IsMarkdown = note.IsMarkdown;
           // noteOrContent.IsBlog = note.IsBlog;
            //noteOrContent.IsTrash = note.IsTrash;
            //noteOrContent.IsDeleted = note.IsDeleted;
            //noteOrContent.IsTrash = note.IsTrash;
           
            //noteOrContent.Usn = note.Usn;
            //noteOrContent.CreatedTime = note.CreatedTime;
            //noteOrContent.UpdatedTime = note.UpdatedTime;
            //noteOrContent.PublicTime = note.PublicTime;

            noteOrContent.Content = "";
            noteOrContent.Usn = afterNoteUsn;
            noteOrContent.UpdatedTime = DateTime.Now;
            noteOrContent.IsDeleted=false;
            noteOrContent.UserId = tokenUserId.ToString("x");
            return Json(noteOrContent, MyJsonConvert.GetOptions());
        }
        //todo:删除trash
        public JsonResult DeleteTrash(string noteId, int usn, string token)
        {
            bool result = TrashService.DeleteTrashApi(MyConvert.HexToLong(noteId), getUserIdByToken(token), usn, out string msg, out int afterUsn);
            if (result)
            {
                return Json(new ReUpdate()
                {
                    Ok=true,
                    Msg="",
                    Usn=afterUsn
                },MyJsonConvert.GetOptions());
            }
            else
            {
                return Json(new ReUpdate()
                {
                    Ok = false,
                    Msg = msg,
                    Usn = afterUsn
                }, MyJsonConvert.GetOptions());
            }
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
        public void FixPostNotecontent(ref ApiNote noteOrContent)
        {
            //todo 这里需要完成fixPostNotecontent
            if (noteOrContent == null || string.IsNullOrEmpty(noteOrContent.Content))
            {
                return;
            }
            APINoteFile[] files = noteOrContent.Files;
            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    if (file.FileId!=null&&file.LocalFileId!=null)
                    {
                        if (!file.IsAttach)
                        {
                            //处理图片链接
                            Regex regex = new Regex(@"(https*://[^/]*?/api/file/getImage\?fileId=)" + file.LocalFileId);
                            if (regex.IsMatch(noteOrContent.Content))
                            {
                                noteOrContent.Content = regex.Replace(noteOrContent.Content, "${1}" + file.FileId+"aaaaaaaa");
                            }
                        }
                        else
                        {
                            //处理附件链接
                            Regex regex = new Regex(@"(https*://[^/]*?/api/file/getAttach\?fileId=)" + file.LocalFileId);
                            if (regex.IsMatch(noteOrContent.Content))
                            {
                                noteOrContent.Content = regex.Replace(noteOrContent.Content, "${1}" + file.FileId+ "aaaaaaaa");
                            }
                        }

                    }
                }

            }
        }
    }
}