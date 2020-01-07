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
    }
}
