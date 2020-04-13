using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreNote.Logic.Entity
{
    public class WebReportInfo
    {
        [Key]
        public long ReportId { get; set; }
        public string WebSiteName { get; set; }
        public string WebSiteURL { get; set; } // UserId回复ToUserId 
        public string URL { get; set; } // 有害内容链接 
        public string Describe { get; set; } // 描述 理由
        public string TraceId { get; set; }//追踪ID
        public string CommentId { get; set; } // 对某条评论进行回复 
    }
}
