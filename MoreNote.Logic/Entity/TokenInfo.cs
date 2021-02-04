using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Logic.Entity
{
    // 随机token
    // 验证邮箱
    // 找回密码
    [Table("token")]
    public class Token
    {
        [Key]
        [Column("token_id")]
        public long TokenId { get; set; }
        [Column("user_id")]
        public long UserId { get; set; }
        [Column("email")]
        public string Email { get; set; }
        //使用token来授权第三方应用使用你的笔记数据
        //当使用API登录时，总是假定你的登录方式时token登录
        //token的规格是16位随机字符串 yk1I-W8o8-FwC0-O2vO 但是不保证后期版本升级后长度发生变化
        //登录第三方应用时，密码使用token
        //leanoteAPI允许用户只使用Token进行登录，那么应该保证token的唯一性
        [Column("token_str")]
        public string TokenStr { get; set; }
        [Column("token_tag")]
        public string TokenTag { get; set; }//token标签
        [Column("token_type")]
        public int TokenType { get; set; }//token类型
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

    }
}
