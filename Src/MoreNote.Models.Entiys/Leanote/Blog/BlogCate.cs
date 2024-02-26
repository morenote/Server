using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Blog
{
	[Table("cate")]
	public class BlogCate : BaseEntity
	{

		[Column("parent_cate_id")]
		public string ParentCateId { get; set; }
		[Column("title")]
		public string Title { get; set; }
		[Column("url_title")]
		public string UrlTitle { get; set; }
		[Column("children")]
		public BlogCate[] Children { get; set; }
	}
}
