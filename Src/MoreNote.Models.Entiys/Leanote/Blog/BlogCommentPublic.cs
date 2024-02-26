using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Blog
{
	[Table("blog_comment_public")]
	public class BlogCommentPublic : BaseEntity
	{

		[Column("blog_comment")]
		BlogComment BlogComment { get; set; }
		[Column("is_i_like_it")]
		public bool IsILikeIt { get; set; }
	}
}
