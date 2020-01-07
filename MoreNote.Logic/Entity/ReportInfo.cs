using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    // 举报
    public class ReportInfo
    {
        [Key]
        public long ReportId { get; set; }
        public long NoteId { get; set; }
        public long UserId { get; set; } // UserId回复ToUserId 
        public string Reason { get; set; } // 评论内容 
        public int CommentId { get; set; } // 对某条评论进行回复 

    }
}
