using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Blog
{
	[Table("user_blog_base")]
	public class UserBlogBase : BaseEntity
	{

		[Column("logo")]
		public string Logo { get; set; } // logo 
		[Column("title")]
		public string Title { get; set; } // 标题 
		[Column("sub_title")]
		public string SubTitle { get; set; } // 副标题 

	}
}
