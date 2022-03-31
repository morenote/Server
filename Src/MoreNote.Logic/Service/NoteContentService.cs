using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service.Segmenter;
using System;
using System.Collections.Generic;
using System.Linq;
using Z.EntityFramework.Plus;

namespace MoreNote.Logic.Service
{
    public class NoteContentService
    {
        private DataContext dataContext;
        public NoteImageService NoteImageService { get; set; }//属性注入
        public NoteService NoteService { get; set; }//属性注入
        JiebaSegmenterService jieba;

        public NoteContentService(DataContext dataContext,JiebaSegmenterService jiebaSegmenter)
        {
            this.dataContext = dataContext;
            this.jieba=jiebaSegmenter;
        }

        public List<NoteContent> ListNoteContent()
        {
            var result = dataContext.NoteContent
                      .Where(b => b.IsBlog == true && b.IsHistory == false);
            return result.ToList<NoteContent>();
        }

        public List<NoteContent> ListNoteContent(bool IsHistory)
        {
            var result = dataContext.NoteContent
                       .Where(b => b.IsBlog == true && b.IsHistory == IsHistory);
            return result.ToList<NoteContent>();
        }

        public NoteContent SelectNoteContent(long? noteId)
        {
            var result = dataContext.NoteContent
                      .Where(b => b.NoteId == noteId && b.IsHistory == false).FirstOrDefault();
            return result;
        }

        public bool InsertNoteContent(NoteContent noteContent)
        {
            var result = dataContext.NoteContent.Add(noteContent);

            return dataContext.SaveChanges() > 0;
        }

        public NoteContent GetNoteContent(long? noteId, long? userId, bool IsHistory)
        {
            var result = dataContext.NoteContent.Where(b => b.UserId == userId && b.NoteId == noteId && b.IsHistory == IsHistory);
            return result == null ? null : result.FirstOrDefault();
        }

        [Obsolete("不推荐使用,使用GetValidNoteContent替代")]
        public NoteContent GetNoteContent(long? noteId, long? userId)
        {
            var result = dataContext.NoteContent.Where(b => b.UserId == userId && b.NoteId == noteId);
            return result == null ? null : result.FirstOrDefault();
        }

        public NoteContent GetValidNoteContent(long? noteId, long? userId)
        {
            var result = dataContext.NoteContent.Where(b => b.UserId == userId && b.NoteId == noteId && b.IsHistory == false);
            return result == null ? null : result.FirstOrDefault();
        }

        // 添加笔记本内容
        // [ok]
        public NoteContent AddNoteContent(NoteContent noteContent)
        {
            noteContent.CreatedTime = Tools.FixUrlTime(noteContent.CreatedTime);
            noteContent.UpdatedTime = Tools.FixUrlTime(noteContent.UpdatedTime);
            noteContent.UpdatedUserId = noteContent.UserId;
            UpdataVector(ref  noteContent);
            InsertNoteContent(noteContent);
            // 更新笔记图片
            NoteImageService.UpdateNoteImages(noteContent.UserId, noteContent.NoteId, "", noteContent.Content);
            return noteContent;
        }
        /// <summary>
        /// 更新全文检索向量
        /// </summary>
        /// <param name="noteContent"></param>
        private void UpdataVector(ref NoteContent noteContent)
        {
            noteContent.ContentVector=jieba.GetNpgsqlTsVector(noteContent.Content);
        }

        // 修改笔记本内容
        // [ok] TODO perm未测
        // hasBeforeUpdateNote 之前是否更新过note其它信息, 如果有更新, usn不用更新
        // TODO abstract这里生成0

        public bool UpdateNoteContent(long? updateUserId, long? noteId, string content, string abstractStr, bool hasBeforeUpdateNote, int usn, DateTime updateTime
       )
        {
            var note = NoteService.GetNoteById(noteId);
            if (note == null || note.NoteId == null)
            {
                return false;
            }
            var userId = note.UserId;
            if (userId != updateUserId)
            {
                // throw new Exception("不支持共享笔记");
                return false;
            }
            var updatedTime = DateTime.Now;
            var noteContext = dataContext.NoteContent
                          .Where(b => b.NoteId == noteId && b.IsHistory == false)
                          .FirstOrDefault();
            if (noteContext != null)
            {
                noteContext.IsHistory = true;
                dataContext.SaveChanges();
            }

            var insertNoteConext = new NoteContent()
            {
                NoteContentId = SnowFlakeNet.GenerateSnowFlakeID(),
                NoteId = noteId,
                UserId = userId,
                IsBlog = noteContext == null ? noteContext.IsBlog : false,
                Content = content,
                Abstract = abstractStr,
                CreatedTime = noteContext == null ? noteContext.CreatedTime : DateTime.Now,
                UpdatedTime = updatedTime,
                UpdatedUserId = userId,
                IsHistory = false
            };
            UpdataVector(ref insertNoteConext);
            InsertNoteContent(insertNoteConext);
            //todo: 需要完成函数UpdateNoteContent
            return true;
        }

        public bool UpdateNoteContent(ApiNote apiNote, out string msg, out long? contentId)
        {
            //更新 将其他笔记刷新
            var noteId = apiNote.NoteId.ToLongByHex();
            var note = dataContext.Note.Where(b => b.NoteId == noteId).First();
            var noteContent = dataContext.NoteContent.Where(b => b.NoteId == noteId && b.IsHistory == false).FirstOrDefault();
            //如果笔记内容发生变化，生成新的笔记内容
            if (apiNote.Content != null)
            {
                //新增笔记内容，需要将上一个笔记设置为历史笔记
                dataContext.NoteContent.Where(b => b.NoteId == noteId && b.IsHistory == false).Update(x => new NoteContent() { IsHistory = true });
                contentId = SnowFlakeNet.GenerateSnowFlakeID();
                NoteContent contentNew = new NoteContent()
                {
                    NoteContentId = contentId,
                    NoteId = noteContent.NoteId,
                    UserId = noteContent.UserId,
                    IsBlog = noteContent.IsBlog,
                    Content = noteContent.Content,
                    Abstract = noteContent.Abstract,
                    CreatedTime = noteContent.CreatedTime,
                    UpdatedTime = noteContent.UpdatedTime,
                    UpdatedUserId = noteContent.UpdatedUserId,
                    IsHistory = noteContent.IsHistory,
                };
                contentNew.IsHistory = false;

                if (apiNote.IsBlog != null)
                {
                    contentNew.IsBlog = apiNote.IsBlog.GetValueOrDefault();
                }
                if (apiNote.Abstract != null)
                {
                    contentNew.Abstract = apiNote.Abstract;
                }
                if (apiNote.Content != null)
                {
                    contentNew.Content = apiNote.Content;
                }
                if (apiNote.UpdatedTime != null)
                {
                    contentNew.UpdatedTime = apiNote.UpdatedTime;
                }
                UpdataVector(ref contentNew);
                dataContext.NoteContent.Add(contentNew);
                msg = "";
                return dataContext.SaveChanges() > 0;
            }
            else
            {   //没有新增笔记内容，那么仅仅修改原始笔记内容就可以了
                contentId = noteContent.NoteContentId;
                if (apiNote.IsBlog != null)
                {
                    noteContent.IsBlog = apiNote.IsBlog.GetValueOrDefault();
                }
                if (apiNote.Abstract != null)
                {
                    noteContent.Abstract = apiNote.Abstract;
                }

                if (apiNote.UpdatedTime != null)
                {
                    noteContent.UpdatedTime = apiNote.UpdatedTime;
                }
            }
            msg = "";
            dataContext.SaveChanges();
            return true;
        }
        public void UpdateNoteContent(long? noteId,  NoteContent noteContent)
        {
            dataContext.NoteContent.Where(b => b.NoteId == noteId && b.IsHistory == false).Update(x => new NoteContent() { IsHistory = true });
            AddNoteContent(noteContent);
        }


        public List<NoteContent> ListNoteContentByNoteIds(long?[] noteIds)
        {
            List<NoteContent> noteContents = new List<NoteContent>(10);
            foreach (var noteId in noteIds)
            {
                var noteContent = dataContext.NoteContent.Where(b => b.NoteId == noteId && b.IsHistory == false).FirstOrDefault();
                noteContents?.Add(noteContent);
            }
            return noteContents;
        }

        public Dictionary<long?, NoteContent> DictionaryNoteContentByNoteIds(long?[] noteIds)
        {
            var noteContents = new Dictionary<long?, NoteContent>(10);
            foreach (var noteId in noteIds)
            {
                var noteContent = dataContext.NoteContent.Where(b => b.NoteId == noteId && b.IsHistory == false).FirstOrDefault();
                noteContents?.Add(noteId, noteContent);
            }
            return noteContents;
        }

        public bool DeleteByIdAndUserId(long? noteId, long? userId, bool Including_the_history)
        {
            if (Including_the_history)
            {
                dataContext.NoteContent.Where(b => b.NoteId == noteId && b.UserId == userId).Delete();
            }
            else
            {
                dataContext.NoteContent.Where(b => b.NoteId == noteId && b.UserId == userId && b.IsHistory == false).Delete();
            }

            return true;
        }

        public bool Delete_HistoryByNoteIdAndUserId(long? noteId, long? userId)
        {
            throw new Exception("此方法需要实现");
        }

        // 当设置/取消了笔记为博客
        public bool UpdateNoteContentIsBlog(long? noteId, long? userid, bool isBlog)
        {
            var noteContent = dataContext.NoteContent.Where(b => b.NoteId == noteId && b.UserId == userid).FirstOrDefault();
            if (noteContent == null)
            {
                return false;
            }
            noteContent.IsBlog = isBlog;
            return dataContext.SaveChanges() > 0;
        }
    }
}