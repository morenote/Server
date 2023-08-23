using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Morenote.Models.Models.Entity;

namespace MoreNote.Models.Entity.Leanote.Management.Email
{
    [Table("email_log")]
    public class EmailLog : BaseEntity
    {

        [Column("email")]
        public string Email { get; set; } // 发送者 
        [Column("subject")]
        public string Subject { get; set; } // 主题 
        [Column("body")]
        public string Body { get; set; } // 内容 
        [Column("msg")]
        public string Msg { get; set; } // 发送失败信息 
        [Column("ok")]
        public bool Ok { get; set; } // 发送是否成功 
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

    }
}
