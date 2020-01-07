using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    public class EmailLog
    {
        [Key]
        public long LogId { get; set;}
        public string Email { get; set; } // 发送者 
        public string Subject { get; set; } // 主题 
        public string Body { get; set; } // 内容 
        public string Msg { get; set; } // 发送失败信息 
        public bool Ok { get; set; } // 发送是否成功 
        public DateTime CreatedTime { get; set; }

    }
}
