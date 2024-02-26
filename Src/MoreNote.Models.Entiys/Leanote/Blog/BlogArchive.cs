using MoreNote.Models.Entity.Leanote.Notes;

namespace MoreNote.Models.Entity.Leanote.Blog
{
	public class BlogArchive
	{
		public int Year { get; set; }
		//public ArchiveMonth[] MonthAchives { get; set; }
		public Note[] Posts { get; set; }
	}
}
