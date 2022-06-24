using github.hyfree.GM;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Morenote.Framework.Filter.Global;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;

using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Models.DTO.Leanote.ApiRequest;
using MoreNote.Models.DTO.Leanote.USBKey;
using MoreNote.Models.Enum;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Note/[action]")]
    [ServiceFilter(typeof(CheckTokenFilter))]
    // [ApiController]
    public class NoteAPIController : APIBaseController
    {
        private AttachService attachService;
        private NoteService noteService;
        private TokenSerivce tokenSerivce;
        private NoteContentService noteContentService;
        private NotebookService notebookService;
        private TrashService trashService;
        private IHttpContextAccessor accessor;
        private NoteRepositoryService noteRepositoryService;
        private EPassService ePassService;
        private GMService gMService;
        DataSignService dataSignService;
        public NoteAPIController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            NoteService noteService,
            NoteContentService noteContentService,
            NotebookService notebookService,
            NoteRepositoryService noteRepositoryService,
            TrashService trashService,
            EPassService ePass,
            GMService gMService,
            DataSignService dataSignService
           ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.attachService = attachService;
            this.noteService = noteService;
            this.tokenSerivce = tokenSerivce;
            this.noteContentService = noteContentService;
            this.trashService = trashService;
            this.accessor = accessor;
            this.notebookService = notebookService;
            this.noteRepositoryService = noteRepositoryService;
            this.ePassService = ePass;
            this.dataSignService = dataSignService;
            this.gMService = gMService;
        }

        //todo:获取同步的笔记
        //public JsonResult GetSyncNotes([ModelBinder(BinderType = typeof(Hex2LongModelBinder))]long? userId,int afterUsn,int maxEntry,string token)
        //{
        //    if (maxEntry==0) maxEntry=100;
        //    ApiNote[] apiNotes=NoteService.GetSyncNotes(userId,afterUsn,maxEntry);
        //    return Json(apiNotes,MyJsonConvert.GetOptions());
        //}
        public JsonResult GetSyncNotes(int afterUsn, int maxEntry, string token)
        {
            if (maxEntry == 0) maxEntry = 100;
            ApiNote[] apiNotes = noteService.GetSyncNotes(GetUserIdByToken(token), afterUsn, maxEntry);
            return Json(apiNotes, MyJsonConvert.GetLeanoteOptions());
        }

        //todo:得到笔记本下的笔记
        public IActionResult GetNotes(string notebookId, string token)
        {
            Note[] notes = noteService.ListNotes(GetUserIdByToken(token), notebookId.ToLongByHex(), false);
          
            return Json(notes, MyJsonConvert.GetLeanoteOptions());
        }

        //todo:得到trash
        public IActionResult GetTrashNotes(string token)
        {

            Note[] notes = noteService.ListTrashNotes(GetUserIdByToken(token),false, true);
          
            return Json(notes, MyJsonConvert.GetLeanoteOptions());
        }

        //todo:获取笔记
        public IActionResult GetNote(string token,string noteId)
        {
            var userId=GetUserIdByToken(token);
            var note = noteService.GetNote(userId,noteId.ToLongByHex());
            var apiNotes=   noteService.ToApiNotes(new Note[]{ note});
            return Json(apiNotes[0], MyJsonConvert.GetLeanoteOptions());
        }

        //todo:得到note和内容
        public IActionResult GetNoteAndContent(string token, string noteId)
        {
            User tokenUser = tokenSerivce.GetUserByToken(token);
            if (tokenUser == null)
            {
                return Json(new ApiRe() { Ok = false, Msg = "" }, MyJsonConvert.GetLeanoteOptions());
            }
            try
            {
                NoteAndContent noteAndContent = noteService.GetNoteAndContent(noteId.ToLongByHex(), tokenUser.UserId, false, false, false);

                ApiNote[] apiNotes = noteService.ToApiNotes(new Note[] { noteAndContent.note });
                ApiNote apiNote = apiNotes[0];
                apiNote.Content = noteService.FixContent(noteAndContent.noteContent.Content, noteAndContent.note.IsMarkdown);
                apiNote.Desc = noteAndContent.note.Desc;
                apiNote.Abstract = noteAndContent.noteContent.Abstract;
                if (noteAndContent == null)
                {
                    return Json(new ApiRe() { Ok = false, Msg = "" }, MyJsonConvert.GetLeanoteOptions());
                }
                else
                {
                    return Json(apiNote, MyJsonConvert.GetLeanoteOptions());
                }
            }
            catch (Exception ex)
            {
               return Json(new ApiRe() { Ok = false, Msg = ex.Message }, MyJsonConvert.GetLeanoteOptions());
            }
        }

        //todo:格式化URL

        //todo:得到内容
        public async Task<IActionResult> GetNoteContent(string token, string noteId)
        {

            ApiRe re = new ApiRe()
            {
                Ok = false,
                Msg = "GetNoteContent_is_error"
            };
            var user=GetUserByToken(token);
            if (user==null)
            {
                return LeanoteJson(re);
            }

            Note note = noteService.GetNote(noteId.ToLongByHex(), GetUserIdByToken(token));
            NoteContent noteContent = noteContentService.GetNoteContent(noteId.ToLongByHex(), GetUserIdByToken(token), false);
            if (noteContent == null || note == null)
            {
                return Json(re, MyJsonConvert.GetLeanoteOptions());
            }
            if (noteContent != null && !string.IsNullOrEmpty(noteContent.Content))
            {
                noteContent.Content = noteService.FixContent(noteContent.Content, note.IsMarkdown);
            }
            else
            {
                noteContent.Content = "<p>Content is IsNullOrEmpty<>";
            }
            if (noteContent.IsEncryption)
            {
                noteContent.Content =await this.cryptographyProvider.SM2Decrypt(noteContent.Content);

            }

            re.Ok = true;
            re.Data= noteContent;
            return LeanoteJson(re);
        }

        //todo:添加笔记
        public async Task<IActionResult> AddNote(ApiNote noteOrContent, string token)
        {
            var re=new ApiRe();

            var user=tokenSerivce.GetUserByToken(token);

            if (user==null)
            {
                return LeanoteJson(re);
            }


            //json 返回状态乱
          
            long? tokenUserId = GetUserIdByToken(token); ;
            long? myUserId = tokenUserId;
            if (noteOrContent == null || string.IsNullOrEmpty(noteOrContent.NotebookId))
            {
                return Json(new ApiRe() { Ok = false, Msg = "notebookIdNotExists" }, MyJsonConvert.GetSimpleOptions());
            }
            long? noteId = idGenerator.NextId();

            if (noteOrContent.Title == null)
            {
                noteOrContent.Title = "无标题";
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
                            var result = UploadImages("FileDatas[" + file.LocalFileId + "]", tokenUserId, noteId, file.IsAttach, out long? serverFileId, out string msg);
                            if (!result)
                            {
                                if (string.IsNullOrEmpty(msg))
                                {
                                    re.Msg = "fileUploadError";
                                }
                                else
                                {
                                    re.Msg = msg;
                                    return Json(re, MyJsonConvert.GetLeanoteOptions());
                                }
                            }
                            else
                            {
                                // 建立映射
                                file.FileId = serverFileId.ToHex24();
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
            else
            {
            }
            //-------------替换笔记内容中的文件ID
            FixPostNotecontent(ref noteOrContent);
            if (noteOrContent.Tags != null)
            {
                if (noteOrContent.Tags.Length > 0 && noteOrContent.Tags[0] == null)
                {
                    noteOrContent.Tags = Array.Empty<string>();
                    //noteOrContent.Tags= new string[] { ""};
                }
            }
            //-------------新增笔记对象
            Note note = new Note()
            {
                UserId = tokenUserId,
                NoteId = noteId,
                CreatedUserId = tokenUserId,
                UpdatedUserId = noteId,
                NotebookId = noteOrContent.NotebookId.ToLongByHex(),
                Title = noteOrContent.Title,
                Tags = noteOrContent.Tags,
                Desc = noteOrContent.Desc,
                IsBlog = noteOrContent.IsBlog.GetValueOrDefault(),
                IsMarkdown = noteOrContent.IsMarkdown.GetValueOrDefault(),
                AttachNum = attachNum,
                CreatedTime = noteOrContent.CreatedTime,
                UpdatedTime = noteOrContent.UpdatedTime,
                ContentId = idGenerator.NextId()
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
            if (noteOrContent.Desc == null)
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
          
            note = await noteService.AddNoteAndContent(note, noteContent, myUserId);
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
            noteOrContent.NoteId = noteId.ToHex24();
            noteOrContent.UserId = tokenUserId.ToHex24();
            noteOrContent.Title = note.Title;
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

            return Json(noteOrContent, MyJsonConvert.GetLeanoteOptions());
        }

        //todo:更新笔记
        public JsonResult UpdateNote(ApiNote noteOrContent, string token)
        {
            Note noteUpdate = new Note();
            var needUpdateNote = false;
            var re = new ReUpdate();
            long? tokenUserId = GetUserIdByToken(token);
            var noteId = noteOrContent.NoteId.ToLongByHex();
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
            var note = noteService.GetNote(noteId, tokenUserId);
            var noteContent = noteContentService.GetNoteContent(note.NoteId, tokenUserId, false);
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
                            var result = UploadImages("FileDatas[" + file.LocalFileId + "]", tokenUserId, noteId, file.IsAttach, out long? serverFileId, out string msg);
                            if (!result)
                            {
                                if (string.IsNullOrEmpty(msg))
                                {
                                    re.Msg = "fileUploadError";
                                }
                                if (!string.Equals(msg, "notImage", System.StringComparison.OrdinalIgnoreCase))
                                {
                                    return Json(re, MyJsonConvert.GetLeanoteOptions());
                                }
                            }
                            else
                            {
                                // 建立映射
                                file.FileId = serverFileId.ToHex24();
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
                attachService.UpdateOrDeleteAttachApiAsync(noteId, tokenUserId, noteOrContent.Files);
            }
            //-------------更新笔记内容
            var afterContentUsn = 0;
            var contentOk = false;
            var contentMsg = "";
            long? contentId = 0;
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
            contentOk = noteContentService.UpdateNoteContent(
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
                return Json(re, MyJsonConvert.GetLeanoteOptions());
            }

            //-------------更新笔记元数据
            int afterNoteUsn = 0;
            var noteOk = false;
            var noteMsg = "";

            noteOk = noteService.UpdateNote(
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
                return Json(re, MyJsonConvert.GetLeanoteOptions());
            }
            //处理结果
            //-------------API返回客户端信息
            note = noteService.GetNote(noteId, tokenUserId);
            // noteOrContent.NoteId = noteId.ToHex24();
            // noteOrContent.UserId = tokenUserId.ToHex24();
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
            noteOrContent.IsDeleted = false;
            noteOrContent.UserId = tokenUserId.ToHex24();
            return Json(noteOrContent, MyJsonConvert.GetLeanoteOptions());
        }

        //todo:删除trash
        public JsonResult DeleteTrash(string noteId, int usn, string token)
        {
            bool result = trashService.DeleteTrashApi(noteId.ToLongByHex(), GetUserIdByToken(token), usn, out string msg, out int afterUsn);
            if (result)
            {
                return Json(new ReUpdate()
                {
                    Ok = true,
                    Msg = "",
                    Usn = afterUsn
                }, MyJsonConvert.GetLeanoteOptions());
            }
            else
            {
                return Json(new ReUpdate()
                {
                    Ok = false,
                    Msg = msg,
                    Usn = afterUsn
                }, MyJsonConvert.GetLeanoteOptions());
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
                    if (file.FileId != null && file.LocalFileId != null)
                    {
                        if (!file.IsAttach)
                        {
                            //处理图片链接
                            Regex regex = new Regex(@"(https*://[^/]*?/api/file/getImage\?fileId=)" + file.LocalFileId);
                            if (regex.IsMatch(noteOrContent.Content))
                            {
                                noteOrContent.Content = regex.Replace(noteOrContent.Content, "${1}" + file.FileId);
                            }
                        }
                        else
                        {
                            //处理附件链接
                            Regex regex = new Regex(@"(https*://[^/]*?/api/file/getAttach\?fileId=)" + file.LocalFileId);
                            if (regex.IsMatch(noteOrContent.Content))
                            {
                                noteOrContent.Content = regex.Replace(noteOrContent.Content, "${1}" + file.FileId);
                            }
                        }
                    }
                }
            }
        }


        public IActionResult GetNotChildrenByNotebookId(string token,string notebookId)
        {
            var apiRe = new ApiRe();

            var user = tokenSerivce.GetUserByToken(token);

            if (user != null)
            {
                //var repository = noteRepositoryService.GetNotesRepository(repositoryId.ToLongByHex());

                //var memerRole = noteRepositoryService.GetRepositoryMemberRole(repositoryId.ToLongByHex());

                var book = notebookService.GetNotebookById(notebookId.ToLongByHex());
                if (book == null)
                {
                    return LeanoteJson(apiRe);

                }
                //检查用户是否对仓库具有读权限
                if (noteRepositoryService.Verify(book.NotesRepositoryId, user.UserId, RepositoryAuthorityEnum.Read))
                {
                    var notes = noteService.GetNotChildrenByNotebookId(notebookId.ToLongByHex());
                    apiRe.Ok = true;
                    apiRe.Data = notes;
                }
            }
            return LeanoteJson(apiRe);
        }
        public async Task<IActionResult> CreateNote(string token,string noteTitle,string notebookId,bool isMarkdown,string dataSignJson)
        {
            if (string.IsNullOrEmpty(noteTitle))
            {
                noteTitle ="未命名";
            }
            var re=new ApiRe(); 
            var user=tokenSerivce.GetUserByToken(token);
            var notebook=notebookService.GetNotebookById(notebookId.ToLongByHex());

            if (user==null || notebook==null)
            {
                return LeanoteJson(re);
            }
            //验证签名
            var dataSign = DataSignDTO.FromJSON(dataSignJson);
            var verify = await this.ePassService.VerifyDataSign(dataSign);
            if (!verify)
            {
                return LeanoteJson(re);
            }
            verify = dataSign.SignData.Operate.Equals("/api/Note/CreateNote");
            if (!verify)
            {
                re.Msg = "Operate is not Equals ";
                return LeanoteJson(re);
            }
            //签名存证
            this.dataSignService.AddDataSign(dataSign, "CreateNote");
            

            var repositoryId=notebook.NotesRepositoryId;
            verify=  noteRepositoryService.Verify(repositoryId, user.UserId, RepositoryAuthorityEnum.Write);
            if (!verify)
            {
                return LeanoteJson(re);
            }
            var noteId = idGenerator.NextId();
            var noteContentId= idGenerator.NextId();
            var content=isMarkdown? "欢迎使用markdown文档 power by vditor": "欢迎使用富文本文档 power by textbus";
            var usn = noteRepositoryService.IncrUsn(repositoryId);

            NoteContent noteContent=new NoteContent()
            {
                NoteContentId= noteContentId,
                Abstract= content,
                Content= content,
                
                UserId=user.UserId,
                NoteId=noteId,
                CreatedTime=DateTime.Now,
                UpdatedTime=DateTime.Now,
                UpdatedUserId=user.UserId

            };
           await  noteContentService.AddNoteContent(noteContent);

            var note=new Note()
            {
                NotebookId=notebook.NotebookId,
                NoteId=noteId,
                ContentId=noteContentId,
                Title= noteTitle,
                UrlTitle=noteTitle,
                NotesRepositoryId=repositoryId,
                IsMarkdown=isMarkdown,
                CreatedTime =DateTime.Now,
                UserId=user.UserId,
                CreatedUserId=user.UserId,
                Desc=string.Empty,
                Usn=usn,
                Tags=Array.Empty<string>()

            };
            noteService.AddNote(note);
            re.Ok=true;
            re.Data=note;
           return LeanoteJson(re);


        }

        public IActionResult UpdateNoteTitle(string token, string noteId, string noteTitle)
        {
            var user = tokenSerivce.GetUserByToken(token);
            var re = new ApiRe();
            if (user == null)
            {
                return LeanoteJson(re);
            }
            var note = noteService.GetNote(noteId.ToLongByHex(), user.UserId);
            var verify = noteRepositoryService.Verify(note.NotesRepositoryId, user.UserId, RepositoryAuthorityEnum.Write);
            if (!verify)
            {
                return LeanoteJson(re);
            }
            noteService.UpdateNoteTitle(note.NoteId, noteTitle);
            re.Ok=true;
            re.Data=note;
            return LeanoteJson(re);

        }

        public async Task<IActionResult> UpdateNoteTitleAndContent(string token,string noteId,string noteTitle,string content, string dataSignJson,string digitalEnvelopeJson)
        {

            var user=tokenSerivce.GetUserByToken(token);
            var re=new ApiRe();
            if (user==null)
            {
                return LeanoteJson(re);

            }
            DigitalEnvelope digitalEnvelope=null;
            //数字信封
            if (this.config.SecurityConfig.ForceDigitalEnvelope)
            {

                digitalEnvelope = DigitalEnvelope.FromJSON(digitalEnvelopeJson);
                var data = digitalEnvelope.GetPayLoadValue(this.gMService, this.config.SecurityConfig.PrivateKey);
                if (data == null)
                {
                    throw new Exception("数字信封解密失败");
                }
                //赋予解密的数字信封
                content = data;
            }
            //验证签名
            var dataSign = DataSignDTO.FromJSON(dataSignJson);
            var verify = await this.ePassService.VerifyDataSign(dataSign);
            if (!verify)
            {
                return LeanoteJson(re);
            }
            verify = dataSign.SignData.Operate.Equals("/api/Note/UpdateNoteTitleAndContent");
            if (!verify)
            {
                re.Msg = "Operate is not Equals ";
                return LeanoteJson(re);
            }
            //签名存证
            this.dataSignService.AddDataSign(dataSign, "UpdateNoteTitleAndContent");

            //-------------校验参数合法性
            if (user == null)
            {
                re.Msg = "NOlogin";
                re.Ok = false;
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
           
            // 先判断USN的问题, 因为很可能添加完附件后, 会有USN冲突, 这时附件就添错了
            var note = noteService.GetNote(noteId.ToLongByHex(), user.UserId);
             verify=noteRepositoryService.Verify(note.NotesRepositoryId,user.UserId,RepositoryAuthorityEnum.Write);
            if (!verify)
            {
                return LeanoteJson(re);
            }

            if (note == null || note.NoteId == 0)
            {
                re.Msg = "notExists";
                re.Ok = false;
                return Json(re, MyJsonConvert.GetSimpleOptions());
            }
            var des=MyHtmlHelper.SubHTMLToRaw(content, 200);

            var noteContentId = idGenerator.NextId();

            NoteContent noteContent = new NoteContent()
            {
                NoteContentId = noteContentId,
                Abstract = content,
                Content = content,

                UserId = user.UserId,
                NoteId = note.NoteId,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                UpdatedUserId = user.UserId
            };

            await noteContentService.UpdateNoteContent(note.NoteId,noteContent);
            noteService.UpdateNoteTitle(note.NoteId,noteTitle);
           
            var usn=  noteRepositoryService.IncrUsn(note.NotesRepositoryId);
            noteService.UpdateUsn(note.NoteId,usn);
            re.Ok=true;
            re.Data=note;
            if (this.config.SecurityConfig.ForceDigitalEnvelope)
            {

                var key = digitalEnvelope.getSM4Key(this.gMService, this.config.SecurityConfig.PrivateKey);
                var json = note.ToJson();

                var payLoad = new PayLoadDTO();
                payLoad.SetData(json);

                var payLoadJson= payLoad.ToJson();

                var jsonHex = Common.Utils.HexUtil.ByteArrayToString(Encoding.UTF8.GetBytes(payLoadJson));

                var enc = gMService.SM4_Encrypt_CBC(jsonHex,key, "00000000000000000000000000000000", true);
                re.Data = enc;
                re.Encryption = true;
            }

            return LeanoteJson(re);

        }

        public async Task<IActionResult> DeleteNote(string token,string noteRepositoryId, string noteId,string dataSignJson)
        {
            var user = tokenSerivce.GetUserByToken(token);
            var re = new ApiRe();
            if (user == null)
            {
                return LeanoteJson(re);
            }

            //验证签名
            var dataSign = DataSignDTO.FromJSON(dataSignJson);
            var verify = await this.ePassService.VerifyDataSign(dataSign);
            if (!verify)
            {
                return LeanoteJson(re);
            }

       
            verify = dataSign.SignData.Operate.Equals("/api/Note/DeleteNote");
            if (!verify)
            {
                re.Msg = "Operate is not Equals ";
                return LeanoteJson(re);
            }
            //签名存证
            this.dataSignService.AddDataSign(dataSign, "DeleteNote");



            var note = noteService.GetNoteById(noteId.ToLongByHex());
           
            var repositoryId = note.NotesRepositoryId;
            if (repositoryId!=noteRepositoryId.ToLongByHex())
            {
                return LeanoteJson(re);
            }
            verify = noteRepositoryService.Verify(repositoryId, user.UserId, RepositoryAuthorityEnum.Write);
            if (!verify)
            {
                return LeanoteJson(re);
            }
            var usn = noteRepositoryService.IncrUsn(repositoryId);
            var noteDelte= noteService.DeleteNote(noteId.ToLongByHex(), usn);
            re.Ok = true;
            re.Data= noteDelte;
          


            return LeanoteJson(re);
            
        }


        /// <summary>
        /// 搜索仓库中的笔记
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
      
        public IActionResult SearchRepositoryNote(string token, string noteRepositoryId,string key,int index)
        {
             var re=new ApiRe();
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
            var user = tokenSerivce.GetUserByToken(token);
        
            if (user == null)
            {
                return LeanoteJson(re);
            }
            var repositor = this.noteRepositoryService.GetNotesRepository(noteRepositoryId.ToLongByHex());
            
            if (!repositor.Visible)
            {
                var verify = noteRepositoryService.Verify(repositor.Id, user.UserId, RepositoryAuthorityEnum.Read);
                if (!verify)
                {
                    return LeanoteJson(re);
                }
            }
           

            var notes1 = noteService.SearchRepositoryIdNoteByTitleVector(key, repositor.Id, GetPage(), pageSize);

            var notes2 = noteService.SearchRepositoryNoteByContentVector(key, repositor.Id, GetPage(), pageSize);

            var result = merge(notes1, notes2);
            re.Ok=true;
            re.Data= result;
            return LeanoteJson(re);
        }

        /// <summary>
        /// 拷贝笔记(不推荐使用)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="noteId"></param>
        /// <param name="parentNotebookId"></param>
        /// <returns></returns>
        public IActionResult Copy(string token,string  noteId,string targetParentNotebookId)
        {

            var user = tokenSerivce.GetUserByToken(token);
            var re = new ApiRe();
            if (user == null)
            {
                return LeanoteJson(re);
            }

            var note = noteService.GetNoteById(noteId.ToLongByHex());

            var repositoryId = note.NotesRepositoryId;

            var targetParentNotebook= notebookService.GetNotebookById(targetParentNotebookId.ToLongByHex());
            //目标文件夹必必须位于同一个仓库中
            if (targetParentNotebook.NotesRepositoryId!=repositoryId)
            {
                return LeanoteJson(re);
            }
            //操作者必须拥有写权限
            var verify = noteRepositoryService.Verify(repositoryId, user.UserId, RepositoryAuthorityEnum.Write);
            if (!verify)
            {
                return LeanoteJson(re);
            }
            //usn
            var usn = noteRepositoryService.IncrUsn(repositoryId);

            var noteContext=   noteContentService.GetValidNoteContent(note.NoteId);


            var cloneNoteId = idGenerator.NextId();
            var cloneNoteContentId = idGenerator.NextId();
            var cloneContent = noteContext.Content;

            //添加新文件
            this.noteService.AddNote(repositoryId,targetParentNotebook.NotebookId,cloneNoteId,cloneNoteContentId,user.UserId,note.Title,cloneContent,note.IsMarkdown,usn);

            var cloneNote=this.noteService.GetNote(cloneNoteId);

            re.Ok=true;
            re.Data=cloneNote;

            return LeanoteJson(re);

        }


        private Note[] merge(Note[] notes1, Note[] notes2)
        {
            Dictionary<long?, Note> result = new Dictionary<long?, Note>(notes1.Length + notes2.Length);
            if (notes1 != null)
            {
                foreach (var item in notes1)
                {
                    if (!result.ContainsKey(item.NoteId))
                    {
                        result.Add(item.NoteId, item);
                    }
                }
            }
            if (notes2 != null)
            {
                foreach (var item in notes2)
                {
                    if (!result.ContainsKey(item.NoteId))
                    {
                        result.Add(item.NoteId, item);
                    }
                }
            }

            return result.Values.ToArray();
        }
    }
}