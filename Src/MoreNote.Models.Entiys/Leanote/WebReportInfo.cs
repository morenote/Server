using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Logic.Entity
{
	[Table("web_report_info")]
	public class WebReportInfo : BaseEntity
	{

		[Column("web_site_name")]
		public string WebSiteName { get; set; }
		[Column("web_site_url")]
		public string WebSiteURL { get; set; } // UserId回复ToUserId 
		[Column("url")]
		public string URL { get; set; } // 有害内容链接 
		[Column("describe")]
		public string Describe { get; set; } // 描述 理由
		[Column("trace_id")]
		public string TraceId { get; set; }//追踪ID
		[Column("comment_id")]
		public string CommentId { get; set; } // 对某条评论进行回复 
	}
}
