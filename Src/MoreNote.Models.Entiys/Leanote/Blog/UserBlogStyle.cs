using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Blog
{
	[Table("user_blog_style")]
	public class UserBlogStyle : BaseEntity
	{

		[Column("style")]
		public string Style { get; set; } // 风格 
		[Column("css")]
		public string Css { get; set; } // 自定义css 

	}

}
