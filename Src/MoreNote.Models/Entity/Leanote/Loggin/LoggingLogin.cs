using Microsoft.EntityFrameworkCore;
using MoreNote.CryptographyProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Loggin
{
    /// <summary>
    /// 登录日志
    /// </summary>
    [Table("logging_login"), Index(nameof(Id), nameof(UserId), nameof(LoginDateTime), nameof(LoginMethod), nameof(IsLoginSuccess),nameof(Ip))]
    public class LoggingLogin
    {
        [Key]
        [Column("id")]
        public long? Id { get; set; }//登录事件id
        [Column("user_id")]
        public long? UserId { get; set; }//登录用户
        [Column("login_datetime")]
        public DateTime LoginDateTime { get; set; }//登录时间
        [Column("login_method")]
        public string? LoginMethod { get; set; }//登录方式
        [Column("is_login_success")]
        public bool IsLoginSuccess { get; set; }//是否登录成功
        [Column("error_meesage")]
        public string? ErrorMessage { get; set; }//失败原因
        [Column("ip")]
        public string? Ip { get; set; }//登陆者IP地址
        [Column("browser_request_header")]
        public string? BrowserRequestHeader { get; set; }//登录浏览器请求头
        [Column("hmac")]
        public string? Hmac { get; set; }//计算Hmac

        [NotMapped]
        public bool Verify { get; set; }
        public string ToStringNoMac()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Id=" + this.Id);
            stringBuilder.Append("UserId=" + this.UserId);
            stringBuilder.Append("DateTime=" + this.LoginDateTime);
            stringBuilder.Append("LoginMethod=" + this.LoginMethod);
            stringBuilder.Append("IsLoginSuccess=" + this.IsLoginSuccess);
            stringBuilder.Append("FailureMessage=" + this.ErrorMessage);
            stringBuilder.Append("Ip=" + this.Ip);
            stringBuilder.Append("Ip=" + this.Ip);
            stringBuilder.Append("BrowserRequestHeader=" + this.BrowserRequestHeader);
            return stringBuilder.ToString();
        }

        public async Task<LoggingLogin> AddMac(ICryptographyProvider cryptographyProvider)
        {
            var bytes = Encoding.UTF8.GetBytes(this.ToStringNoMac());
            var base64 = Convert.ToBase64String(bytes);
            this.Hmac = await cryptographyProvider.Hmac(base64);
            return this;
        }
        
        public async Task<bool> VerifyHmac(ICryptographyProvider cryptographyProvider)
        {
            if (string.IsNullOrEmpty(this.Hmac))
            {
                return false;
            }
            var bytes = Encoding.UTF8.GetBytes(this.ToStringNoMac());
            var base64 = Convert.ToBase64String(bytes);
            return await cryptographyProvider.VerifyHmac(base64, this.Hmac);
        }
    }
}