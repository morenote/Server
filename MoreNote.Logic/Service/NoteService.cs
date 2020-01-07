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

        public static Note SelectNote(long NoteId)
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
    }
}
