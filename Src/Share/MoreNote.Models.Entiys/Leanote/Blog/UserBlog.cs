using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Blog
{
	// 每个用户一份博客设置信息
	[Table("user_blog")]
	public class UserBlog : BaseEntity
	{

		[Column("user_id")]
		public long? UserId { get; set; } // 谁的 
		[Column("logo")]
		public string? Logo { get; set; } // 
		[Column("title")]
		public string? Title { get; set; } // 标题 
		[Column("sub_title")]
		public string? SubTitle { get; set; } // 副标题 
		[Column("about_me")]
		public string? AboutMe { get; set; } // 关于我, 
		[Column("can_comment")]

		public bool CanComment { get; set; } // 是否可以评论 
		[Column("comment_type")]

		public string? CommentType { get; set; } // default 
		[Column("disqus_id")]
		public string? DisqusId { get; set; } // default 


		[Column("style")]
		public string? Style { get; set; } // 风格 
		[Column("css")]
		public string? Css { get; set; } // 自定义css 
		[Column("theme_id")]

		public long? ThemeId { get; set; } // 主题Id 
		[Column("theme_path")]
		public string? ThemePath { get; set; } // 储存值, 从Theme中获取, 相对路径 public/ 

		[Column("cate_ids")]
		public string[]? CateIds { get; set; } // 分类Id, 排序好的
		[Column("singles")]
		public string[]? Singles { get; set; } //单页, 排序好的, map包含: ["Title"], ["SingleId"]
		[Column("per_page_size")]
		public int PerPageSize { get; set; } //  
		[Column("sort_field")]
		public string? SortField { get; set; } // 排序字段 
		[Column("is_asc")]
		public bool IsAsc { get; set; } // // 排序类型, 降序, 升序, 默认是false, 表示降序, 
		[Column("sub_domain")]
		public string? SubDomain { get; set; } // 二级域名 
		[Column("domain")]
		public string? Domain { get; set; } // 自定义域名 
	}
}
