using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreNote.Logic.Service
{
    public class TagService
    {
        public static NoteTag[] GeSyncTags(long userId,int afterUsn,int maxEntry)
        {
            using (var db = new DataContext())
            {
                var result = db.NoteTag.
                    Where(b => b.UserId == userId && b.Usn > afterUsn).Take(maxEntry);
                return result.ToArray();
            }
        }
    }
}
