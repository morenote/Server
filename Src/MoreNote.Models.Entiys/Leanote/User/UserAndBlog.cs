using MoreNote.Models.Entity.Leanote.Blog;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.User
{
	// 用户与博客信息结合, 公开
	[Table("user_and_blog")]
	public class UserAndBlog
	{
		[Key]
		[Column("user_id")]
		public long? UserId { get; set; }// 必须要设置bson:"_id" 不然mgo不会认为是主键

		[Column("email")]
		public string Email { get; set; } // 全是小写

		[Column("username")]
		public string Username { get; set; }// 不区分大小写, 全是小写

		[Column("logo")]
		public string Logo { get; set; }

		[Column("blog_title")]
		public string BlogTitle { get; set; }// 博客标题

		[Column("blog_logo")]
		public string BlogLogo { get; set; }// 博客Logo

		[Column("blog_url")]
		public string BlogUrl { get; set; } // 博客链接, 主页

		public BlogUrls BlogUrls { get; set; }
	}
}
