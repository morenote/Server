using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Blog
{
	/// <summary>
	/// 博客主机绑定
	/// </summary>
	[Table("blog_hosting_bundle")]
	public class BlogHostingBundle : BaseEntity
	{
		[Column("host")]
		public string? Host { get; set; }//全是小写，主机地址，比如blog.morenote.top

		[Column("blog_id")]
		public long? BlogId { get; set; }//全是小写
	}
}
