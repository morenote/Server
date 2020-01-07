using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
   public class Session
    {
        [Key]
        public long Id { get; set; } // 没有意义 
        public long SessionId { get; set; } // SessionId 
        public int LoginTimes { get; set; } // 登录错误时间 
        public string Captcha { get; set; } // 验证码 
        public long UserId { get; set; } // API时有值UserId 
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; } // 更新时间, 

    }
}
