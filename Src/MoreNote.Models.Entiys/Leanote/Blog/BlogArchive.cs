using MoreNote.Models.Entity.Leanote.Notes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Blog
{
    public class BlogArchive
    {
        public int Year { get; set; }
        //public ArchiveMonth[] MonthAchives { get; set; }
        public Note[] Posts { get; set; }
    }
}
