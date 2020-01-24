using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public  class NoteService
    {
   

   
        public static bool AddNote(Note note)
        {
            using (var db = new DataContext())
            {
                var result = db.Note.Add(note);

                return db.SaveChanges() > 0;
            }
        }

        public static List<Note> ListNotes(long userId,
            long notebookId,
            bool isTrash,
            int pageNumber,
            int pageSize,
            string sortField,
            bool isAsc,
            bool isBlog)
        {
            using (var db = new DataContext())
            {
                var result = db.Note
                    .Where(b => b.NotebookId==notebookId&& b.UserId == userId);
                return result.ToList<Note>();
            }
        }

        public static Note GetNoteById(long NoteId)
        {
            using (var db = new DataContext())
            {
                var result = db.Note
                    .Where(b => b.NoteId == NoteId).FirstOrDefault();
                return result;
            }
        }
        public static Note SelectNoteByTag(String Tag)
        {
            using (var db = new DataContext())
            {
                var result = db.Note
                    .Where(b => b.Tags.Contains(Tag)).FirstOrDefault();
               
                return result;
            }
        }
        public static NoteAndContent[] GetNoteAndContent(bool isBlog)
        {
            using (var db = new DataContext())
            {
                var result = (from _note in db.Note
                              join _content in db.NoteContent on _note.ContentId equals _content.NoteId
                              where _note.IsBlog == isBlog
                              select new NoteAndContent
                              {
                                  note = _note,
                                  noteContent = _content
                              }).ToArray();
                return result;
            }
        }
        public static NoteAndContent GetNoteAndContent(long noteId)
        {
            using (var db = new DataContext())
            {
                var result = (from _note in db.Note
                              join _content in db.NoteContent on _note.ContentId equals _content.NoteId
                              where _note.NoteId==noteId
                              select new NoteAndContent
                              {
                                  note = _note,
                                  noteContent = _content
                              }).FirstOrDefault();
                return result;
            }
        }
        public static ApiNote[] GeSyncNotes(long userid, int afterUsn, int maxEntry)
        {
            using (var db = new DataContext())
            {
                var result = db.Note.
                    Where(b => b.UserId == userid && b.Usn > afterUsn).Take(maxEntry);
                return ToApiNotes(result.ToArray());
            }
        }
        public static ApiNote[] ToApiNotes(Note[] notes)
        {
            if (notes!=null&&notes.Length>0)
            {
                ApiNote[] apiNotes = new ApiNote[notes.Length];
                for (int i = 0; i < notes.Length; i++)
                {
                    apiNotes[i] = ToApiNote(ref notes[i], null);

                }
                return apiNotes;
            }
            else
            {
                return null;
            }
           
        }
        public static ApiNote ToApiNote (ref Note note,NoteFile[] files)
        {
            ApiNote apiNote = new ApiNote()
            {
                NoteId = note.NoteId,
              
		        NotebookId= note.NotebookId,
		        UserId= note.UserId,
		        Title= note.Title,
		        Tags= note.Tags,
		        IsMarkdown= note.IsMarkdown,
		        IsBlog= note.IsBlog,
		        IsTrash= note.IsTrash,
		        IsDeleted= note.IsDeleted,
		        Usn= note.Usn,
		        CreatedTime= note.CreatedTime,
		        UpdatedTime= note.UpdatedTime,
		        PublicTime= note.PublicTime,
		        Files= files

            };
            return apiNote;
        }


        // getDirtyNotes, 把note的图片, 附件信息都发送给客户端
        // 客户端保存到本地, 再获取图片, 附件

        // 得到所有图片, 附件信息
        // 查images表, attachs表
        // [待测]
        public NoteFile getFiles(long noteId)
        {
            return null;
        }

    }
}
