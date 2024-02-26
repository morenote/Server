using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Blog
{
	// 点赞记录
	[Table("blog_like")]
	public class BlogLike : BaseEntity
	{

		[Column("note_id")]
		public long? NoteId { get; set; }
		[Column("user_id")]
		public long? UserId { get; set; }
		[Column("created_time")]
		public DateTime CreatedTime { get; set; }

	}
}
