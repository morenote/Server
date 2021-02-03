using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Logic.Entity
{
    [Table("web_report_info")]
    public class WebReportInfo
    {
        [Key]
        [Column("report_id")]
        public long ReportId { get; set; }
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
