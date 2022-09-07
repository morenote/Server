using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Morenote.Models.Models.Entity;

namespace MoreNote.Logic.Entity
{
    // 举报
    [Table("report_info")]
    public class ReportInfo: BaseEntity
    {
        
        [Column("note_id")]
        public long? NoteId { get; set; }
        [Column("user_id")]
        public long? UserId { get; set; } // UserId回复ToUserId 
        [Column("reason")]
        public string Reason { get; set; } // 评论内容 
        [Column("comment_id")]
        public int CommentId { get; set; } // 对某条评论进行回复 

    }
}
