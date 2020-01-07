using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    // 随机token
    // 验证邮箱
    // 找回密码
    public class Token
    {
        [Key]
        public long TokenId { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        //使用token来授权第三方应用使用你的笔记数据
        //当使用API登录时，总是假定你的登录方式时token登录
        //token的规格是16位随机字符串 yk1I-W8o8-FwC0-O2vO
        //登录第三方应用时，密码使用token
        public string TokenStr { get; set; }
        public string tokenTag { get; set; }//token标签
        public int Type { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
