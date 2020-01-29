using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{

    public class NoteContentService
    {
        public static List<NoteContent> ListNoteContent()
        {
            using (var db = new DataContext())
            {
                var result = db.NoteContent
                    .Where(b => b.IsBlog == true);
                return result.ToList<NoteContent>();
            }
        }
        public static NoteContent SelectNoteContent(long noteId)
        {
            using (var db = new DataContext())
            {
                var result = db.NoteContent
                    .Where(b => b.NoteId.Equals(noteId) ).FirstOrDefault();
                return result;
            }

        }
        public static bool InsertNoteContent(NoteContent noetContent)
        {
            using (var db = new DataContext())
            {
                var result = db.NoteContent.Add(noetContent);

                return db.SaveChanges() > 0;
            }
        }
        public static NoteContent GetNoteContent(long noteId,long userId)
        {
            using (var db = new DataContext())
            {
                var result = db.NoteContent
                    .Where(b => b.UserId==userId&&b.NoteId==noteId);
                return result==null?null:result.FirstOrDefault();
            }
        }
        // 添加笔记本内容
        // [ok]
        public static NoteContent AddNoteContent(NoteContent noteContent)
        {
            throw new Exception();
        }

        // 修改笔记本内容
        // [ok] TODO perm未测
        // hasBeforeUpdateNote 之前是否更新过note其它信息, 如果有更新, usn不用更新
        // TODO abstract这里生成
        public static bool UpdateNoteContent(long updateUserId,long noteId,string content,string abstractStr,bool hasBeforeUpdateNote,int usn,DateTime updateTime)
        {
            throw new Exception();
        }
   

    }
}
