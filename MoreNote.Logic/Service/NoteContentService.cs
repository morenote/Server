using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using Z.EntityFramework.Plus;

namespace MoreNote.Logic.Service
{
    public class NoteContentService
    {
        private DependencyInjectionService dependencyInjectionService;

        public NoteContentService(DependencyInjectionService dependencyInjectionService, DataContext dataContext)
        {
            this.dependencyInjectionService = dependencyInjectionService;
        }

        public List<NoteContent> ListNoteContent()
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.NoteContent
                          .Where(b => b.IsBlog == true && b.IsHistory == false);
                return result.ToList<NoteContent>();
            }
        }

        public List<NoteContent> ListNoteContent(bool IsHistory)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.NoteContent
                           .Where(b => b.IsBlog == true && b.IsHistory == IsHistory);
                return result.ToList<NoteContent>();
            }
        }

        public NoteContent SelectNoteContent(long noteId)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.NoteContent
                          .Where(b => b.NoteId == noteId && b.IsHistory == false).FirstOrDefault();
                return result;
            }
        }

        public bool InsertNoteContent(NoteContent noteContent)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.NoteContent.Add(noteContent);

                return dataContext.SaveChanges() > 0;
            }
        }

        public NoteContent GetNoteContent(long noteId, long userId, bool IsHistory)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.NoteContent.Where(b => b.UserId == userId && b.NoteId == noteId && b.IsHistory == IsHistory);
                return result == null ? null : result.FirstOrDefault();
            }
        }

        [Obsolete("不推荐使用,使用GetValidNoteContent替代")]
        public NoteContent GetNoteContent(long noteId, long userId)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.NoteContent.Where(b => b.UserId == userId && b.NoteId == noteId);
                return result == null ? null : result.FirstOrDefault();
            }
        }

        public NoteContent GetValidNoteContent(long noteId, long userId)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.NoteContent.Where(b => b.UserId == userId && b.NoteId == noteId && b.IsHistory == false);
                return result == null ? null : result.FirstOrDefault();
            }
        }

        // 添加笔记本内容
        // [ok]
        public NoteContent AddNoteContent(NoteContent noteContent)
        {
            NoteImageService noteImageService = dependencyInjectionService.GetNoteImageService();
            noteContent.CreatedTime = Tools.FixUrlTime(noteContent.CreatedTime);
            noteContent.UpdatedTime = Tools.FixUrlTime(noteContent.UpdatedTime);
            noteContent.UpdatedUserId = noteContent.UserId;
            InsertNoteContent(noteContent);
            // 更新笔记图片
            noteImageService.UpdateNoteImages(noteContent.UserId, noteContent.NoteId, "", noteContent.Content);
            return noteContent;
        }

        // 修改笔记本内容
        // [ok] TODO perm未测
        // hasBeforeUpdateNote 之前是否更新过note其它信息, 如果有更新, usn不用更新
        // TODO abstract这里生成
        public bool UpdateNoteContent(long updateUserId, long noteId, string content, string abstractStr, bool hasBeforeUpdateNote, int usn, DateTime updateTime,
            out string msg, out int afterContentUsn)
        {
            //todo: 需要完成函数UpdateNoteContent
            throw new Exception();
        }

        public bool UpdateNoteContent(ApiNote apiNote, out string msg, out long contentId)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
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
        }

        public bool DeleteByIdAndUserId(long noteId, long userId, bool Including_the_history)
        {
         using(var dataContext = dependencyInjectionService.GetDataContext())
		{
		  if (Including_the_history)
            {
                dataContext.NoteContent.Where(b => b.NoteId == noteId && b.UserId == userId).Delete();
            }
            else
            {
                dataContext.NoteContent.Where(b => b.NoteId == noteId && b.UserId == userId && b.IsHistory == false).Delete();
            }
		
		}
          

            return true;
        }

        public bool Delete_HistoryByNoteIdAndUserId(long noteId, long userId)
        {
            throw new Exception("此方法需要实现");
        }
    }
}