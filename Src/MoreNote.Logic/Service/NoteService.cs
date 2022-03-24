using Microsoft.EntityFrameworkCore;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service.Segmenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Z.EntityFramework.Plus;

namespace MoreNote.Logic.Service
{
    public class NoteService
    {
        private DataContext dataContext;

        public ConfigService ConfigService { get; set; }
        public BlogService BlogService { get; set; }
        public NoteImageService NoteImageService { get; set; }
        public AttachService AttachService { get; set; }
        public CommonService CommonService { get; set; }
        public UserService UserService { get; set; }
        public InitServices InitServices { get; set; }

        public NotebookService NotebookService { get; set; }

        public TagService TagService { get; set; }
        public NoteContentService NoteContentService { get; set; }

        public ShareService ShareService { get; set; }
        private JiebaSegmenterService jieba { get; set; }

        public NoteService(DataContext dataContext, JiebaSegmenterService jieba)
        {
            this.dataContext = dataContext;
            this.jieba = jieba;
        }

        public bool AddNote(Note note)
        {
            var result = dataContext.Note.Add(note);
            return dataContext.SaveChanges() > 0;
        }

        // fileService调用
        // 不能是已经删除了的, life bug, 客户端删除后, 竟然还能在web上打开
        //我也是？？？？
        public Note GetNoteById(long? NoteId)
        {
            var result = dataContext.Note
                .Where(b => b.NoteId == NoteId).FirstOrDefault();
            return result;
        }

        public Note SelectNoteByTag(String Tag)
        {
            var result = dataContext.Note
                 .Where(b => b.Tags.Contains(Tag)).FirstOrDefault();

            return result;
        }

        public NoteAndContent[] GetNoteAndContent(bool isBlog, int pageIndex)
        {
            var result = (from _note in dataContext.Note
                          join _content in dataContext.NoteContent on _note.NoteId equals _content.NoteId
                          where _note.IsBlog == isBlog && _content.IsBlog == true && _content.IsHistory == false
                          select new NoteAndContent
                          {
                              note = _note,
                              noteContent = _content
                          }).OrderByDescending(b => b.note.PublicTime).Skip((pageIndex - 1) * 10).Take(10).OrderByDescending(b => b.note.PublicTime).ToArray();
            return result;
        }

        public NoteAndContent[] GetNoteAndContentForBlog(int pageIndex, long? userId)
        {
            var result = (from _note in dataContext.Note
                          join _content in dataContext.NoteContent on _note.NoteId equals _content.NoteId
                          where _note.IsBlog == true && _content.IsHistory == false && _note.IsTrash == false && _note.IsDeleted == false && _note.UserId == userId
                          select new NoteAndContent
                          {
                              note = _note,
                              noteContent = _content
                          }).OrderByDescending(b => b.note.PublicTime).Skip((pageIndex - 1) * 10).Take(10).ToArray();
            return result;
        }

        public NoteAndContent[] SearchNoteAndContentForBlog(int pageIndex, long? userId, string keywords)
        {
            var query = jieba.GetSerachNpgsqlTsQuery(keywords);
            var result = (from _note in dataContext.Note
                          join _content in dataContext.NoteContent on _note.NoteId equals _content.NoteId
                          where _note.UserId == userId
                                && _note.TitleVector.Matches(query)
                                && _note.IsBlog == true
                                && _content.IsHistory == false
                                && _note.IsTrash == false
                                && _note.IsDeleted == false
                          select new NoteAndContent
                          {
                              note = _note,
                              noteContent = _content
                          }).OrderByDescending(b => b.note.PublicTime).Skip((pageIndex - 1) * 10).Take(10).ToArray();
            return result;
        }

        public NoteAndContent[] GetNoteAndContentByTag(int pageIndex, long? userId, string tag)
        {
            var result = (from _note in dataContext.Note
                          join _content in dataContext.NoteContent on _note.NoteId equals _content.NoteId
                          where _note.IsBlog == true && _content.IsHistory == false && _note.IsTrash == false && _note.IsDeleted == false && _note.UserId == userId && _note.Tags.Contains(tag)
                          select new NoteAndContent
                          {
                              note = _note,
                              noteContent = _content
                          }).OrderByDescending(b => b.note.Title).Skip((pageIndex - 1) * 10).Take(10).OrderByDescending(b => b.note.PublicTime).ToArray();
            return result;
        }

        public NoteAndContent[] GetNoteAndContentForBlogOfNoteBookId(int pageIndex, long? notebookId, long? userId)
        {
            var result = (from _note in dataContext.Note
                          join _content in dataContext.NoteContent on _note.NoteId equals _content.NoteId
                          where _note.IsBlog == true && _content.IsBlog == true && _content.IsHistory == false && _note.IsTrash == false && _note.IsDeleted == false && _note.NotebookId == notebookId && _note.UserId == userId
                          select new NoteAndContent
                          {
                              note = _note,
                              noteContent = _content
                          }).OrderByDescending(b => b.note.PublicTime).Skip((pageIndex - 1) * 10).Take(10).OrderByDescending(b => b.note.PublicTime).ToArray();
            return result;
        }

        public NoteAndContent[] GetNoteAndContentForBlogOfTag(int pageIndex, string tag)
        {
            var result = (from _note in dataContext.Note
                          join _content in dataContext.NoteContent on _note.NoteId equals _content.NoteId
                          where _note.IsBlog == true && _content.IsBlog == true && _content.IsHistory == false && _note.IsTrash == false && _note.IsDeleted == false && _note.Tags.Contains(tag)
                          select new NoteAndContent
                          {
                              note = _note,
                              noteContent = _content
                          }).OrderByDescending(b => b.note.PublicTime).Skip((pageIndex - 1) * 10).Take(10).OrderByDescending(b => b.note.PublicTime).ToArray();
            return result;
        }

        public NoteAndContent GetNoteAndContentForBlog(long? noteId)
        {
            {
                var result = (from _note in dataContext.Note
                              join _content in dataContext.NoteContent on _note.NoteId equals _content.NoteId
                              where _note.IsBlog == true && _note.NoteId == noteId && _content.IsHistory == false && _note.IsTrash == false && _note.IsDeleted == false
                              select new NoteAndContent
                              {
                                  note = _note,
                                  noteContent = _content
                              }).FirstOrDefault();
                return result;
            }
        }

        public NoteAndContent GetNoteAndContent(long? noteId)
        {
            var result = (from _note in dataContext.Note
                          join _content in dataContext.NoteContent on _note.NoteId equals _content.NoteId
                          where _note.NoteId == noteId && _content.IsHistory == false
                          select new NoteAndContent
                          {
                              note = _note,
                              noteContent = _content
                          }).FirstOrDefault();
            return result;
        }

        public bool SetAccessPassword(long? userId, long? noteId, string password)
        {
            var note = dataContext.Note.Where(b => b.UserId == userId && b.NoteId == noteId).FirstOrDefault();
            if (note == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                note.AccessPassword = null;
            }
            else
            {
                note.AccessPassword = SHAEncryptHelper.Hash256Encrypt(password + userId + noteId);
            }

            dataContext.SaveChanges();
            return true;
        }

        public bool VerifyAccessPassword(long? userId, long? noteId, string password, string hash)
        {
            var passwordhash = SHAEncryptHelper.Hash256Encrypt(password + userId + noteId);

            if (passwordhash.Equals(hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 增加文章的阅读数量
        /// </summary>
        /// <param name="noteId"></param>
        public void AddReadNum(long? noteId)
        {
            var result = dataContext.Note.Where(b => b.NoteId == noteId);
            if (result != null)
            {
                var note = result.FirstOrDefault();
                if (note != null)
                {
                    note.ReadNum++;
                }
            }
            dataContext.SaveChanges();
        }

        public bool SetDeleteStatus(long? noteID, long? userId)
        {
            var result = dataContext.Note.Where(b => b.NoteId == noteID && b.UserId == userId);
            if (result == null)
            {
                return false;
            }
            var note = result.FirstOrDefault();
            note.IsTrash = true;
            note.IsDeleted = true;
            var afterUsn = UserService.IncrUsn(userId);
            note.Usn = afterUsn;
            return dataContext.SaveChanges() > 0;
        }

        public bool SetDeleteStatus(long? noteID, long? userId, out int afterUsn)
        {
            var result = dataContext.Note.Where(b => b.NoteId == noteID && b.UserId == userId);
            if (result == null)
            {
                afterUsn = 0;
                return false;
            }
            var note = result.FirstOrDefault();
            note.IsTrash = true;
            note.IsDeleted = true;
            afterUsn = UserService.IncrUsn(userId);
            note.Usn = afterUsn;
            return dataContext.SaveChanges() > 0;
        }

        public Note GetNote(long? noteId, long? userId, bool IsTrash, bool isDelete)
        {
            var result = dataContext.Note.Where(b => b.UserId == userId && b.NoteId == noteId);
            return result == null ? null : result.FirstOrDefault();
        }

        public Note GetNote(long? noteId, long? userId)
        {
            var result = dataContext.Note.Where(b => b.UserId == userId && b.NoteId == noteId);

            return result == null ? null : result.FirstOrDefault();
        }

        // 得到blog, blogService用
        // 不要传userId, 因为是公开的
        public Note GetBlogNote(long? noteId)
        {
            throw new Exception();
        }

        /// <summary>
        /// 获得笔记和内容
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="userId"></param>
        /// <param name="isTrach">是否垃圾</param>
        /// <param name="isDelete">是否删除</param>
        /// <param name="IsHistory">是否历史记录</param>
        /// <returns></returns>
        public NoteAndContent GetNoteAndContent(long? noteId, long? userId, bool isTrach, bool isDelete, bool IsHistory)
        {
            Note note = GetNote(noteId, userId, isTrach, isDelete);
            NoteContent noteContent = NoteContentService.GetNoteContent(noteId, userId, IsHistory);
            return new NoteAndContent()
            {
                note = note,
                noteContent = noteContent
            };
        }

        public Note GetNoteBySrc(string src, string userId)
        {
            throw new Exception();
        }

        public void GetNoteAndContentBySrc(string url, long? userId, out long? noteId, out NoteAndContentSep noteAndContent)
        {
            throw new Exception();
        }

        // 获取同步的笔记
        // > afterUsn的笔记
        public ApiNote[] GetSyncNotes(long? userid, int afterUsn, int maxEntry)
        {
            var result = dataContext.Note.
                        Where(b => b.UserId == userid && b.Usn > afterUsn).OrderBy(b => b.Usn).Take(maxEntry);
            return ToApiNotes(result.ToArray());
        }

        // note与apiNote的转换
        public ApiNote[] ToApiNotes(Note[] notes)
        {
            // 2, 得到所有图片, 附件信息
            // 查images表, attachs表

            if (notes != null && notes.Length > 0)
            {
                ApiNote[] apiNotes = new ApiNote[notes.Length];

                //得到所有文件

                long?[] noteIds = new long?[notes.Length];
                for (int i = 0; i < notes.Length; i++)
                {
                    noteIds[i] = notes[i].NoteId;
                }

                Dictionary<long?, List<APINoteFile>> noteFilesMap = getFiles(noteIds);
                for (int i = 0; i < notes.Length; i++)
                {
                    APINoteFile[] aPINoteFiles = noteFilesMap.ContainsKey(notes[i].NoteId) ? noteFilesMap[notes[i].NoteId].ToArray() : null;
                    apiNotes[i] = ToApiNote(ref notes[i], aPINoteFiles);
                }

                return apiNotes;
            }
            else
            {
                return new ApiNote[0];
            }
        }

        // note与apiNote的转换
        public ApiNote ToApiNote(ref Note note, APINoteFile[] files)
        {
            ApiNote apiNote = new ApiNote()
            {
                NoteId = note.NoteId.ToHex24(),
                NotebookId = note.NotebookId.ToHex24(),
                UserId = note.UserId.ToHex24(),
                Title = note.Title,
                Desc = note.Desc,
                Tags = note.Tags,

                IsMarkdown = note.IsMarkdown,
                IsBlog = note.IsBlog,
                IsTrash = note.IsTrash,
                IsDeleted = note.IsDeleted,
                Usn = note.Usn,
                CreatedTime = note.CreatedTime,
                UpdatedTime = note.UpdatedTime,
                PublicTime = note.PublicTime,
                Files = (files == null ? Array.Empty<APINoteFile>() : files)
            };
            return apiNote;
        }

        // getDirtyNotes, 把note的图片, 附件信息都发送给客户端
        // 客户端保存到本地, 再获取图片, 附件

        /// <summary>
        /// 得到所有图片, 附件信息
        /// 查images表, attachs表
        /// [待测]
        /// </summary>
        /// <param name="noteIds"></param>
        /// <returns></returns>
        public Dictionary<long?, List<APINoteFile>> getFiles(long?[] noteIds)
        {
            var noteImages = NoteImageService.getImagesByNoteIds(noteIds);
            var noteAttachs = AttachService.GetAttachsByNoteIds(noteIds);
            Dictionary<long?, List<APINoteFile>> noteFilesMap = new Dictionary<long?, List<APINoteFile>>(100);

            //if (noteImages != null && noteImages.Length > 0)
            //{
            //    foreach (var item in noteImages)
            //    {
            //        NoteFile noteFile=new NoteFile()
            //        {
            //            FileId=item.FileId,
            //            Type=item.Type
            //        };
            //        if (noteFilesMap.ContainsKey(item.note))
            //        {
            //            noteFilesMap[item.NoteId].Add(noteFile);
            //        }
            //        else
            //        {
            //            noteFilesMap.Add(item.NoteId,new List<NoteFile>());
            //            noteFilesMap[item.NoteId].Add(noteFile);

            //        }

            //    }
            //}
            foreach (var noteId in noteIds)
            {
                APINoteFile apiNoteFile = new APINoteFile();

                //images 图像
                //if (noteImages != null && noteImages.ContainsKey(noteId))
                //{
                //    if (!noteFilesMap.ContainsKey(noteId))
                //    {
                //        noteFilesMap.Add(noteId, new List<APINoteFile>());
                //    }
                //    foreach (NoteFile image in noteImages[noteId])
                //    {
                //        apiNoteFile = new APINoteFile()
                //        {
                //            FileId = image.FileId.ToHex24(),
                //            Type = image.Type
                //        };
                //        noteFilesMap[noteId].Add(apiNoteFile);
                //    }
                //}
                //attach 附件
                if (noteAttachs != null && noteAttachs.ContainsKey(noteId))
                {
                    if (!noteFilesMap.ContainsKey(noteId))
                    {
                        noteFilesMap.Add(noteId, new List<APINoteFile>());
                    }
                    foreach (AttachInfo attach in noteAttachs[noteId])
                    {
                        apiNoteFile = new APINoteFile()
                        {
                            FileId = attach.AttachId.ToHex24(),
                            Type = attach.Type,
                            Title = attach.Title,
                            IsAttach = true
                        };
                        noteFilesMap[noteId].Add(apiNoteFile);
                    }
                }
            }
            return noteFilesMap;
        }

        // getDirtyNotes, 把note的图片, 附件信息都发送给客户端
        // 客户端保存到本地, 再获取图片, 附件

        // 得到所有图片, 附件信息
        // 查images表, attachs表
        // [待测]

        public NoteFile[] getFiles(long? noteId)
        {
            var noteImages = NoteImageService.getImagesByNoteId(noteId);
            var noteAttachs = AttachService.GetAttachsByNoteId(noteId);

            Dictionary<long, NoteFile[]> noteFilesMap = new Dictionary<long, NoteFile[]>(100);

            throw new Exception();
        }

        // 列出note, 排序规则, 还有分页
        // CreatedTime, UpdatedTime, title 来排序
        public Note[] ListNotes(long? userId, long? notebookId, bool isTrash
        )
        {
            var result = dataContext.Note
                .Where(b => b.NotebookId == notebookId && b.UserId == userId && b.IsTrash == isTrash);
            return result.ToArray();
        }

        public Note[] ListNotes(long? userId, long? notebookId, bool isDeleted, bool isTrash)
        {
            var result = dataContext.Note
                .Where(b => b.NotebookId == notebookId && b.UserId == userId && b.IsDeleted == isDeleted && b.IsTrash == isTrash);
            return result.ToArray();
        }
        public Note[] ListTrashNotes(long? userId, bool isDeleted, bool isTrash)
        {
            var result = dataContext.Note
                .Where(b => b.UserId == userId && b.IsDeleted == isDeleted && b.IsTrash == true);
            return result.ToArray();
        }


        // 列出note, 排序规则, 还有分页
        // CreatedTime, UpdatedTime, title 来排序
        public List<Note> ListNotes(long? userId,
          long? notebookId,
          bool isTrash,
          int pageNumber,
          int pageSize,
          string sortField,
          bool isAsc,
          bool? isBlog)
        {
            if (notebookId == null)
            {
                throw new ArgumentException("notebookId不能为空");
            }
            int skipNum;
            string sortFieldR;

            CommonService.parsePageAndSort(pageNumber, pageSize, sortField, isAsc, out skipNum, out sortFieldR);

            //todo:不支持排序
            List<Note> result = null;
            switch (sortField)
            {
                case "UpdatedTime":
                    result = dataContext.Note
           .Where(b => b.UserId == userId && b.NotebookId == notebookId && b.IsTrash == isTrash && b.IsDeleted == false).OrderBy(s => s.UpdatedTime).Skip(skipNum).Take(pageSize).ToList<Note>();
                    break;

                case "PublicTime":
                    result = dataContext.Note
           .Where(b => b.UserId == userId && b.NotebookId == notebookId && b.IsTrash == isTrash && b.IsDeleted == false).OrderBy(s => s.PublicTime).Skip(skipNum).Take(pageSize).ToList<Note>();
                    break;

                case "CreatedTime":
                    result = dataContext.Note
           .Where(b => b.UserId == userId && b.NotebookId == notebookId && b.IsTrash == isTrash && b.IsDeleted == false).OrderBy(s => s.CreatedTime).Skip(skipNum).Take(pageSize).ToList<Note>();
                    break;

                case "Title":
                    result = dataContext.Note
                .Where(b => b.UserId == userId && b.NotebookId == notebookId && b.IsTrash == isTrash && b.IsDeleted == false).OrderBy(s => s.Title).Skip(skipNum).Take(pageSize).ToList<Note>();
                    break;

                default:
                    result = dataContext.Note
           .Where(b => b.UserId == userId && b.NotebookId == notebookId && b.IsTrash == isTrash && b.IsDeleted == false).OrderBy(s => s.UpdatedTime).Skip(skipNum).Take(pageSize).ToList<Note>();
                    break;
            }

            if (isBlog != null)
            {
                result = (from note in result
                          where note.IsBlog == isBlog
                          select note).ToList<Note>();
            }

            return result.ToList<Note>();
        }

        // 列出note, 排序规则, 还有分页
        // CreatedTime, UpdatedTime, title 来排序
        public List<Note> ListNotes(long? userId,

          bool isTrash,
          int pageNumber,
          int pageSize,
          string sortField,
          bool isAsc,
          bool? isBlog)
        {
            int skipNum;
            string sortFieldR;

            CommonService.parsePageAndSort(pageNumber, pageSize, sortField, isAsc, out skipNum, out sortFieldR);

            //todo:不支持排序
            List<Note> result = null;
            switch (sortField)
            {
                case "UpdatedTime":
                    result = dataContext.Note
           .Where(b => b.UserId == userId && b.IsTrash == isTrash && b.IsDeleted == false).OrderBy(s => s.UpdatedTime).Skip(skipNum).Take(pageSize).ToList<Note>();
                    break;

                case "PublicTime":
                    result = dataContext.Note
           .Where(b => b.UserId == userId && b.IsTrash == isTrash && b.IsDeleted == false).OrderBy(s => s.PublicTime).Skip(skipNum).Take(pageSize).ToList<Note>();
                    break;

                case "CreatedTime":
                    result = dataContext.Note
           .Where(b => b.UserId == userId && b.IsTrash == isTrash && b.IsDeleted == false).OrderBy(s => s.CreatedTime).Skip(skipNum).Take(pageSize).ToList<Note>();
                    break;

                case "Title":
                    result = dataContext.Note
                .Where(b => b.UserId == userId && b.IsTrash == isTrash && b.IsDeleted == false).OrderBy(s => s.Title).Skip(skipNum).Take(pageSize).ToList<Note>();
                    break;

                default:
                    result = dataContext.Note
           .Where(b => b.UserId == userId && b.IsTrash == isTrash && b.IsDeleted == false).OrderBy(s => s.UpdatedTime).Skip(skipNum).Take(pageSize).ToList<Note>();
                    break;
            }

            if (isBlog != null)
            {
                result = (from note in result
                          where note.IsBlog == isBlog
                          select note).ToList<Note>();
            }

            return result.ToList<Note>();
        }

        public List<Note> GetNotChildrenByNotebookId(long? notebookId)
        {
            var result=dataContext.Note.Where(b => b.NotebookId == notebookId).OrderBy(x=>x.Title).ToList<Note>();
            return result;
        }

        // 通过noteIds来查询
        // ShareService调用
        public Note[] ListNotesByNoteIdsWithPageSort(long[] noteIds, long? userId, int pageNumber, int pageSize, string sortField, bool isAsc, bool isBlog)
        {
            throw new Exception();
        }

        // shareService调用
        public Note[] ListNotesByNoteIds(long[] noteIds)
        {
            throw new Exception();
        }

        // blog需要
        public Note[] ListNoteContentsByNoteIds(long[] noteIds)
        {
            throw new Exception();
        }

        // 只得到abstract, 不需要content
        public Note[] ListNoteAbstractsByNoteIds(long[] noteIds)
        {
            throw new Exception();
        }

        public NoteContent[] ListNoteContentByNoteIds(long? noteIds)
        {
            throw new Exception();
        }

        // 添加笔记
        // 首先要判断Notebook是否是Blog, 是的话设为blog
        // [ok]
        public Note AddNote(Note note, bool fromAPI)
        {
            if (note.NoteId == null)
            {
                note.NoteId = SnowFlakeNet.GenerateSnowFlakeID();
            }

            // 关于创建时间, 可能是客户端发来, 此时判断时间是否有
            note.CreatedTime = Tools.FixUrlTime(note.CreatedTime);
            note.UpdatedTime = Tools.FixUrlTime(note.UpdatedTime);
            if (note.UrlTitle == null)
            {
                note.UrlTitle = note.NoteId.ToHex();
            }
            //note.UrlTitle = InitServices.GetUrTitle(note.UserId, note.Title, "note", note.NoteId);
            note.Usn = UserService.IncrUsn(note.UserId);
            long? notebookId = note.NotebookId;

            // api会传IsBlog, web不会传
            if (!fromAPI)
            {
                note.IsBlog = NotebookService.IsBlog(notebookId);
            }

            //	if note.IsBlog {
            note.PublicTime = note.UpdatedTime;
            UpdateVector(ref note);

            AddNote(note);
            // tag1
            if (note.Tags != null)
            {
                TagService.AddTags(note.UserId, note.Tags);
            }
            // recount notebooks' notes number
            NotebookService.ReCountNotebookNumberNotes(notebookId);
            return note;
        }

        // 添加共享d笔记
        public Note AddSharedNote(Note note, long? muUserId)
        {
            throw new Exception();
        }

        // 添加笔记和内容
        // 这里使用 info.NoteAndContent 接收?
        public Note AddNoteAndContentForController(Note note, NoteContent noteContent, long? updatedUserId)
        {
            //todo:实现AddNoteAndContentForController
            return AddNoteAndContent(note, noteContent, updatedUserId);
        }

        public Note AddNoteAndContent(Note note, NoteContent noteContent, long? myUserId)
        {
            if (note.NoteId == null)
            {
                note.NoteId = SnowFlakeNet.GenerateSnowFlakeID();
            }
            if (noteContent.NoteContentId == null)
            {
                noteContent.NoteContentId = SnowFlakeNet.GenerateSnowFlakeID();
            }
            noteContent.NoteId = note.NoteId;
            if (note.UserId != null && note.UserId != myUserId)
            {
                //todo:  支持共享笔记
                //note = AddSharedNote(note, myUserId);
            }
            else
            {
                note = AddNote(note, false);
            }

            if (note.NoteId != null)
            {
                NoteContentService.AddNoteContent(noteContent);
            }
            return note;
        }

        public Note AddNoteAndContentApi(Note note, NoteContent noteContent, long? myUserId)
        {
            throw new Exception();
        }

        // 修改笔记
        // 这里没有判断usn
        public bool UpdateNote(long? updateUserId, long? noteId, Note needUpdate, int usn)
        {
            string message;
            int afterUsn;
            return UpdateNote(updateUserId, noteId, needUpdate, usn, out afterUsn, out message);
        }

        public bool UpdateNote(long? updateUserId, long? noteId, Note needUpdate, int usn, out int afterUsn, out string msg)
        {
            var oldNote = GetNoteById(noteId);

            //updateUser 必须是笔记的原主人

            //todo:需要完成函数NoteService.UpdateNote

            if (oldNote == null)
            {
                msg = "notExists";
                afterUsn = 0;
                return false;
            }
            var userId = oldNote.UserId;
            if (oldNote.UserId != updateUserId)
            {
                //todo:当前版本仅支持个人使用 不支持多租户共享编辑或分享笔记
                msg = "noAuth";
                afterUsn = 0;
                return false;
            }
            // 是否已自定义
            if (oldNote.IsBlog && oldNote.HasSelfDefined)
            {
                needUpdate.ImgSrc = "";
                needUpdate.Desc = "";
            }
            needUpdate.UserId = updateUserId;

            // 可以将时间传过来
            needUpdate.UpdatedTime = DateTime.Now;

            afterUsn = UserService.IncrUsn(updateUserId);

            needUpdate.Usn = afterUsn;

            var needRecountTags = false;

            // 是否修改了isBlog
            // 也要修改noteContents的IsBlog
            if (needUpdate.IsBlog != oldNote.IsBlog)
            {
                NoteContentService.UpdateNoteContentIsBlog(noteId, oldNote.UserId, needUpdate.IsBlog);
                if (!oldNote.IsBlog)
                {
                    needUpdate.PublicTime = needUpdate.UpdatedTime;
                }
                needRecountTags = true;
            }
            var newNote = dataContext.Note.Where(b => b.NoteId == noteId && b.UserId == userId).FirstOrDefault();
            newNote.Usn = afterUsn;
            // 添加tag2
            // TODO 这个tag去掉, 添加tag另外添加, 不要这个
            if (!needUpdate.Tags.IsNullOrNothing())
            {
                newNote.Tags = needUpdate.Tags;
                TagService.AddTags(userId, needUpdate.Tags);
                if (oldNote.IsBlog)
                {
                    BlogService.ReCountBlogTags(userId);
                }
            }

            if (needUpdate.Desc.IsValid())
            {
                newNote.Desc = needUpdate.Desc;
            }
            if (needUpdate.Title.IsValid())
            {
                newNote.Title = needUpdate.Title;
            }
            if (needUpdate.ImgSrc.IsValid())
            {
                newNote.ImgSrc = needUpdate.ImgSrc;
            }
            UpdateVector(ref newNote);
            dataContext.SaveChanges();
            // 重新获取之
            oldNote = GetNoteById(noteId);
            var notebookId = needUpdate.NotebookId;
            if (notebookId != null)
            {
                NotebookService.ReCountNotebookNumberNotes(oldNote.NotebookId);
                NotebookService.ReCountNotebookNumberNotes(notebookId);
            }

            msg = string.Empty;
            afterUsn = usn;
            return true;
        }

        private static bool UpdateNote(Note note)
        {
            {
                //   var change= dataContext.Note.Where(b=>b.NoteId==note.NoteId).Update(x=>note);
                return false;
            }
        }

        /// <summary>
        /// 更新笔记 元数据
        /// </summary>
        /// <param name="apiNote"></param>
        /// <returns></returns>
        public bool UpdateNote(ref ApiNote apiNote, long? updateUser, long? contentId, bool verifyUsn, bool verifyOwner,
            out string msg, out int afterUsn)
        {
            var noteId = apiNote.NoteId.ToLongByHex();

            afterUsn = 0;
            if (apiNote == null)
            {
                msg = "apiNote_is_null";
                return false;
            }

            // var noteId = MyConvert.HexToLong(apiNote.NoteId);
            if (noteId == 0)
            {
                msg = "noteId_is_note_long_Number";
                return false;
            }

            var result = dataContext.Note.Where(b => b.NoteId == noteId && b.UserId == updateUser);
            if (result == null)
            {
                msg = "inexistence";
                return false;
            }
            var note = result.FirstOrDefault();

            if (verifyUsn)
            {
                if (note.Usn != apiNote.Usn)
                {
                    msg = "Verify_Usn_Failure";
                    return false;
                }
            }
            if (verifyOwner)
            {
                if (note.UserId != updateUser)
                {
                    msg = "Verify_updateUser_Failure";
                    return false;
                }
            }
            if (apiNote.Desc != null)
            {
                note.Desc = apiNote.Desc;
            }

            if (apiNote.Title != null)
            {
                note.Title = apiNote.Title;
            }
            if (apiNote.IsTrash != null)
            {
                note.IsTrash = apiNote.IsTrash.GetValueOrDefault();
            }
            if (apiNote.IsBlog != null)
            {
                if (note.IsBlog == false && apiNote.IsBlog == true)
                {
                    note.PublicTime = DateTime.Now;
                }
                note.IsBlog = apiNote.IsBlog.GetValueOrDefault(false);
            }
            if (apiNote.Tags != null)
            {
                note.Tags = apiNote.Tags;
                if (apiNote.Tags.Length == 0 || apiNote.Tags[0] == null)
                {
                    note.Tags = Array.Empty<string>();
                }

                TagService.AddTags(note.UserId, note.Tags);
                BlogService.ReCountBlogTags(note.UserId);
            }
            if (apiNote.NotebookId != null)
            {
                var noteBookId = apiNote.NotebookId.ToLongByHex();
                if (note.NotebookId == 0)
                {
                    msg = "NotebookId_Is_Illegal";
                    return false;
                }
                if (note.NotebookId != noteBookId)
                {
                    // 如果修改了notebookId, 则更新notebookId'count
                    // 两方的notebook也要修改
                    NotebookService.ReCountNotebookNumberNotes(note.NotebookId);
                    NotebookService.ReCountNotebookNumberNotes(noteBookId);
                    note.NotebookId = noteBookId;
                }
            }
            if (apiNote.Content != null)
            {
                note.ContentId = contentId;
                if (apiNote.Abstract == null)
                {
                    if (apiNote.IsMarkdown.GetValueOrDefault(note.IsMarkdown))
                    {
                        note.Desc = MyHtmlHelper.SubMarkDownToRaw(apiNote.Content, 200);
                    }
                    else
                    {
                        note.Desc = MyHtmlHelper.SubHTMLToRaw(apiNote.Content, 200);
                    }

                    //  note.Desc = MyHtmlHelper.SubStringHTMLToRaw(apiNote.Content, 200);
                }
                else
                {
                    note.Desc = MyHtmlHelper.SubHTMLToRaw(apiNote.Abstract, 200);

                    //note.Desc = MyHtmlHelper.SubStringHTMLToRaw(apiNote.Abstract, 200);
                }
            }
            if (apiNote.UpdatedTime != null)
            {
                note.UpdatedTime = Tools.FixUrlTime(apiNote.UpdatedTime);
            }
            else
            {
                note.UpdatedTime = DateTime.Now;
            }
            if (note.IsBlog && note.HasSelfDefined)
            {
                note.ImgSrc = null;
                note.Desc = null;
            }
            if (apiNote.IsTrash != null)
            {
                note.IsTrash = apiNote.IsTrash.GetValueOrDefault(false);
                NotebookService.ReCountNotebookNumberNotes(note.NotebookId);
            }
            if (apiNote.IsMarkdown != null)
            {
                note.IsMarkdown = apiNote.IsMarkdown.GetValueOrDefault();
            }
            note.UpdatedUserId = updateUser;

            UpdateVector(ref note);
            //更新用户元数据乐观锁
            afterUsn = UserService.IncrUsn(note.UserId);

            //更新笔记元数据乐观锁
            note.Usn = afterUsn;
            dataContext.SaveChanges();
            msg = "success";
            return true;
        }

        private void UpdateVector(ref Note note)
        {
            note.TitleVector = jieba.GetNpgsqlTsVector(note.Title);
        }

        // 附件修改, 增加noteIncr
        public int IncrNoteUsn(long? noteId, long? userId)
        {
            var afterUsn = UserService.IncrUsn(userId);
            dataContext.Note.Where(b => b.NoteId == noteId && b.UserId == userId).Update(c => new Note() { UpdatedTime = DateTime.Now, Usn = afterUsn });
            dataContext.SaveChanges();
            return afterUsn;
        }

        // 这里要判断权限, 如果userId != updatedUserId, 那么需要判断权限
        // [ok] TODO perm还没测 [del]
        public bool UpdateNoteTitle(long? userId, long? updateUserId, long? noteId, string title)
        {
            throw new Exception();
        }

        // ?????
        // 这种方式太恶心, 改动很大
        // 通过content修改笔记的imageIds列表
        // src="http://localhost:9000/file/outputImage?fileId=541ae75499c37b6b79000005&noteId=541ae63c19807a4bb9000000"
        //哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈歇一会哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈
        public bool updateNoteImages(long? noteId, string content)
        {
            throw new Exception();
        }

        // 更新tags
        // [ok] [del]
        public bool UpdateTags(long? noteId, long? userId, string[] tags)
        {
            throw new Exception();
        }

        /// <summary>
        /// 设置笔记成为博客和置顶
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="noteId"></param>
        /// <param name="isBlog"></param>
        /// <param name="isTop"></param>
        /// <returns></returns>
        public bool ToBlog(long? userId, long? noteId, bool isBlog, bool isTop)
        {
            //如果置顶，必须设置成博客
            if (isTop)
            {
                isBlog = true;
            }
            //如果不是博客，就无需置顶
            if (!isBlog)
            {
                isTop = false;
            }
            var note = dataContext.Note.Where(b => b.UserId == userId && b.NoteId == noteId).FirstOrDefault();
            if (note == null)
            {
                return false;
            }
            note.IsBlog = isBlog;
            note.IsTop = isTop;

            if (isBlog)
            {
                note.PublicTime = DateTime.Now;
            }
            else
            {
                note.HasSelfDefined = false;
            }
            note.Usn = UserService.IncrUsn(userId);
            var ok = dataContext.SaveChanges() > 0;
            // 重新计算tags
            NoteContentService.UpdateNoteContentIsBlog(noteId, userId, isBlog);
            BlogService.ReCountBlogTags(userId);
            return ok;
        }

        // 移动note【未完全实现】
        // trash, 正常的都可以用
        // 1. 要检查下notebookId是否是自己的
        // 2. 要判断之前是否是blog, 如果不是, 那么notebook是否是blog?
        public Note MoveNote(long? userId, long? noteId, long? notebookId)
        {
            //先判断笔记是否是自己的或者共享的
            var note = dataContext.Note.Where(b => b.UserId == userId && b.NoteId == noteId).FirstOrDefault();
            var preNotebookId = note.NotebookId;
            if (note == null)
            {
                return null;
            }
            //更新笔记的笔记本id
            note.NotebookId = notebookId;
            //更新blog状态
            this.updateToNotebookBlog(noteId, notebookId, userId);
            // recount notebooks' notes number
            NotebookService.ReCountNotebookNumberNotes(notebookId);
            // 之前不是trash才统计, trash本不在统计中的
            if (!note.IsTrash && notebookId != preNotebookId)
            {
                NotebookService.ReCountNotebookNumberNotes(preNotebookId);
            }
            return note;
        }

        // 如果自己的blog状态是true, 不用改变,
        // 否则, 如果notebookId的blog是true, 则改为true之
        // 返回blog状态
        // move, copy时用
        public bool updateToNotebookBlog(long? userId, long? notebookId, long? noteId)
        {
            if (IsBlog(noteId))
            {
                return true;
            }
            if (NotebookService.IsBlog(notebookId))
            {
                var note = dataContext.Note.Where(b => b.NoteId == noteId).FirstOrDefault();
                if (note == null)
                {
                    return true;
                }
                note.IsBlog = true;
                note.PublicTime = DateTime.Now;
                dataContext.SaveChanges();
                return true;
            }
            return false;
        }

        // 判断是否是blog
        public bool IsBlog(long? noteId)
        {
            var note = dataContext.Note.Where(b => b.NoteId == noteId).FirstOrDefault();
            if (note == null)
            {
                return false;
            }
            return note.IsBlog;
        }

        // 复制note
        // 正常的可以用
        // 先查, 再新建
        // 要检查下notebookId是否是自己的
        public Note CopyNote(long? noteId, long? userId)
        {
            throw new Exception();
        }

        // 复制别人的共享笔记给我
        // 将别人可用的图片转为我的图片, 复制图片
        public Note CopySharedNote(long? noteId, long? fromUserId, long? myUserId)
        {
            throw new Exception();
        }

        // 通过noteId得到notebookId
        // shareService call
        // [ok]
        public long? GetNotebookId(long? noteId)
        {
            throw new Exception();
        }

        //------------------
        // 搜索Note, 博客使用了
        public Note[] SearchNote(string key, long? userId, int pageNumber, int pageSize, string sortField, bool isAsc, bool isBlog)
        {
            JiebaSegmenterService jiebaSegmenterService = new JiebaSegmenterService();
            var query = jiebaSegmenterService.GetSerachNpgsqlTsQuery(key);
            var list = dataContext.Note.Where(b => b.UserId == userId
                                     && b.IsTrash == false
                                     && b.IsDeleted == false
                                     && b.IsBlog == isBlog
                                     && b.TitleVector.Matches(query)).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList().OrderBy(b => b.Title);
            return list.ToArray();
        }
        /// <summary>
        /// 通过标题向量向量搜索笔记
        /// </summary>
        /// <param name="key"></param>
        /// <param name="userId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Note[] SearchNoteByTitleVector(string key, long? userId, int pageNumber, int pageSize)
        {
            JiebaSegmenterService jiebaSegmenterService = new JiebaSegmenterService();
            var query = jiebaSegmenterService.GetSerachNpgsqlTsQuery(key);
            var list = dataContext.Note.Where(b => b.UserId == userId
                                     && b.IsTrash == false
                                     && b.IsDeleted == false
                                     && b.TitleVector.Matches(query)).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList().OrderBy(b => b.Title);
            return list.ToArray();
        }
        /// <summary>
        /// 通过标题向量和内容向量搜索笔记
        /// </summary>
        /// <param name="key"></param>
        /// <param name="userId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Note[] SearchNoteByTitleVectorAndContentVector(string key, long? userId, int pageNumber, int pageSize)
        {
            JiebaSegmenterService jiebaSegmenterService = new JiebaSegmenterService();
            var query = jiebaSegmenterService.GetSerachNpgsqlTsQuery(key);

            var result = (from _note in dataContext.Note
                          join _content in dataContext.NoteContent on _note.NoteId equals _content.NoteId
                          where _content.IsHistory == false && _note.IsTrash == false && _note.IsDeleted == false && _note.UserId == userId
                          && (_note.TitleVector.Matches(query) || _content.ContentVector.Matches(query))
                          select _note).OrderByDescending(b => b.PublicTime).Skip((pageNumber - 1) * 10).Take(pageSize).ToArray();
            return result;
        }
        /// <summary>
        /// 通过内容全文检索向量搜索笔记
        /// </summary>
        /// <param name="key"></param>
        /// <param name="userId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Note[] SearchNoteByContentVector(string key, long? userId, int pageNumber, int pageSize)
        {
            JiebaSegmenterService jiebaSegmenterService = new JiebaSegmenterService();
            var query = jiebaSegmenterService.GetSerachNpgsqlTsQuery(key);

            var result = (from _note in dataContext.Note
                          join _content in dataContext.NoteContent on _note.NoteId equals _content.NoteId
                          where _content.IsHistory == false && _note.IsTrash == false && _note.IsDeleted == false && _note.UserId == userId
                          && (_content.ContentVector.Matches(query))
                          select _note).OrderByDescending(b => b.PublicTime).Skip((pageNumber - 1) * 10).Take(pageSize).ToArray();
            return result;
        }

        // 搜索noteContents, 补集pageSize个
        public Note[] searchNoteFromContent(Note[] notes, long? userId, string key, int pagSize, string sortField, bool isBlog)
        {
            throw new Exception();
        }

        //----------------
        // tag搜索
        public Note[] SearchNoteByTags(string[] tags, long? userId, int pageNumber, int pageSize, string sortField, bool isAsc)
        {
            throw new Exception();
        }

        public Note[] SearchNoteByTag(string tag, long? userId, int pageNumber, int pageSize)
        {
            var list = dataContext.Note.Where(b => b.UserId == userId
                                    && b.IsTrash == false
                                    && b.IsDeleted == false
                                    && b.Tags.Contains(tag)).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList().OrderBy(b => b.Title);
            return list.ToArray();
        }

        //------------
        // 统计
        public int CountNote(long? userId)
        {
            var result = dataContext.Note
                .Where(b => b.UserId == userId && b.IsDeleted == false && b.IsTrash == false).Count();
            return result;
        }

        public int CountBlog(long? usrId)
        {
            var result = dataContext.Note
                .Where(b => b.UserId == usrId && b.IsDeleted == false && b.IsTrash == false && b.IsBlog == true).Count();
            return result;
        }

        // 通过标签来查询
        public int CountNoteByTag(long? userId, string tag)
        {
            var result = dataContext.NoteTag
                .Where(b => b.UserId == userId && b.Tag.Equals(tag)).Count();
            return result;
        }

        // 删除tag
        // 返回所有note的Usn
        public Dictionary<string, int> UpdateNoteToDeleteTag(long? userId, string targetTag)
        {
            throw new Exception();
        }

        // api
        [Obsolete]

        // 得到笔记的内容, 此时将笔记内的链接转成标准的Leanote Url
        // 将笔记的图片, 附件链接转换成 site.url/file/getImage?fileId=xxx,  site.url/file/getAttach?fileId=xxxx
        public string FixContentBad(string content, bool isMarkdown)
        {
            throw new Exception("废弃");
        }

        public string FixContent(string content, bool isMarkdown)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }
            var baseUrl = ConfigService.config.APPConfig.SiteUrl;
            return FixContent(content, isMarkdown, baseUrl);
        }

        // 得到笔记的内容, 此时将笔记内的链接转成标准的Leanote Url
        // 将笔记的图片, 附件链接转换成 site.url/file/getImage?fileId=xxx,  site.url/file/getAttach?fileId=xxxx
        // 性能更好, 5倍的差距
        public string FixContent(string content, bool isMarkdown, string baseUrl)
        {
            //开发是不可能开发的，只能靠复制粘贴这个样子
            //todo:需要实现FixContent
            // string baseUrl = ConfigService.GetSiteUrl();
            string baseUrlPattern = baseUrl;

            // 避免https的url
            if (baseUrl.Substring(0, 8).Equals("https://"))
            {
                baseUrlPattern = baseUrl.Replace(@"https://", @"https*://");
            }
            else
            {
                baseUrlPattern = baseUrl.Replace("http://", "https*://");
            }
            baseUrlPattern = "(?:" + baseUrlPattern + ")*";

            var patterns = new Dictionary<string, string>[]
            {
                new Dictionary<string, string>()
                {
                    { "src","src"},{"middle","/api/file/getImage"},{"param","fileId"},{ "to","getImage?fileId="}
                },
                  new Dictionary<string, string>()
                {
                    { "src","src"},{"middle","/file/outputImage"},{"param","fileId"},{ "to","getImage?fileId="}
                },
                new Dictionary<string, string>()
                {
                    { "src","href"},{ "middle","/attach/download"},{ "param","attachId"},{"to","getAttach?fileId=" }
                },
                new Dictionary<string, string>()
                {
                    { "src","href"},{ "middle","/api/file/getAtach"},{ "param","fileId"},{"to","getAttach?fileId=" }
                }
            };
            foreach (var eachPattern in patterns)
            {
                if (!isMarkdown)
                {
                    Regex reg = null;
                    Regex reg2 = null;

                    // 富文本处理

                    // <img src="http://leanote.com/file/outputImage?fileId=5503537b38f4111dcb0000d1">
                    // <a href="http://leanote.com/attach/download?attachId=5504243a38f4111dcb00017d"></a>

                    if (eachPattern["src"].Equals("src"))
                    {
                        reg = new Regex("<img(?:[^>]+?)(?:" + eachPattern["src"] + "=['\"]*" + baseUrlPattern + eachPattern["middle"] + "\\?" + eachPattern["param"] + "=(?:[a-z0-9A-Z]{24})[\"']*)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        reg2 = new Regex("<img(?:[^>]+?)(" + eachPattern["src"] + "=['\"]*" + baseUrlPattern + eachPattern["middle"] + "\\?" + eachPattern["param"] + "=([a-z0-9A-Z]{24})[\"']*)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        reg = new Regex("<a(?:[^>]+?)(?:" + eachPattern["src"] + "=['\"]*" + baseUrlPattern + eachPattern["middle"] + "\\?" + eachPattern["param"] + "=(?:[a-z0-9A-Z]{24})[\"']*)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        reg2 = new Regex("<a(?:[^>]+?)(" + eachPattern["src"] + "=['\"]*" + baseUrlPattern + eachPattern["middle"] + "\\?" + eachPattern["param"] + "=([a-z0-9A-Z]{24})[\"']*)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    }
                    content = reg.Replace(content, (math) =>
                    {
                        //Console.WriteLine("markdown表达式1=" + math.Value);
                        if (math.Success)
                        {
                            var eachFind = reg2.Match(math.Value);
                            Console.WriteLine(eachFind.Groups.Count);
                            Console.WriteLine(eachFind.Groups[0]);
                            Console.WriteLine(eachFind.Groups[1]);
                            Console.WriteLine(eachFind.Groups[2]);
                            var src = eachPattern["src"] + "=\"" + baseUrl + "/api/file/" + eachPattern["to"] + eachFind.Groups[2] + "\"";
                            var output = math.Value.Replace(eachFind.Groups[1].Value, src);

                            return output;
                        }
                        return math.Value;
                    });
                }
                else
                {
                    var pre = "!";                       // 默认图片
                    if (eachPattern["src"].Equals("href"))
                    { // 是attach
                        pre = "";
                    }
                    // markdown处理
                    // ![](http://leanote.com/file/outputImage?fileId=5503537b38f4111dcb0000d1)
                    // [selection 2.html](http://leanote.com/attach/download?attachId=5504262638f4111dcb00017f)
                    // [all.tar.gz](http://leanote.com/attach/downloadAll?noteId=5503b57d59f81b4eb4000000)
                    Regex regImageMarkdown = new Regex($@"{pre}\[(?:[^]]*?)\]\({baseUrlPattern}{eachPattern["middle"]}\?{eachPattern["param"]}=(?:[a-z0-9A-Z]{{24}})\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Regex regImageMarkdown2 = new Regex($@"{pre}\[([^]]*?)\]\({baseUrlPattern}{eachPattern["middle"]}\?{eachPattern["param"] }=([a-z0-9A-Z]{{24}})\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Console.WriteLine($@"{pre}\[(?:[^]]*?)\]\({baseUrlPattern}{eachPattern["middle"]}\?{eachPattern["param"]}=(?:[a-z0-9A-Z]{24})\)");
                    content = regImageMarkdown.Replace(content, (math) =>
                    {
                        //Console.WriteLine("markdown表达式1=" + math.Value);
                        if (regImageMarkdown2.IsMatch(math.Value))
                        {
                            var eachFind = regImageMarkdown2.Match(math.Value);
                            Console.WriteLine(eachFind.Groups.Count);
                            Console.WriteLine(eachFind.Groups[0]);
                            Console.WriteLine(eachFind.Groups[1]);
                            Console.WriteLine(eachFind.Groups[2]);
                            return pre + "[" + eachFind.Groups[1] + "](" + baseUrl + "/api/file/" + eachPattern["to"] + eachFind.Groups[2] + ")";
                        }
                        return math.Value;
                    });
                }
            }
            return content;
        }
    }
}