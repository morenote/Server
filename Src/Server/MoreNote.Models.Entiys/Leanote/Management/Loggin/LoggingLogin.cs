using Microsoft.EntityFrameworkCore;

using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Models.Entity.Leanote.Management.Loggin
{
	/// <summary>
	/// 登录日志
	/// </summary>
	[Table("logging_login"), Index(nameof(Id), nameof(UserId), nameof(LoginDateTime), nameof(LoginMethod), nameof(IsLoginSuccess), nameof(Ip))]
	public class LoggingLogin : BaseEntity
	{

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
			stringBuilder.Append("Id=" + Id);
			stringBuilder.Append("UserId=" + UserId);
			stringBuilder.Append("DateTime=" + LoginDateTime);
			stringBuilder.Append("LoginMethod=" + LoginMethod);
			stringBuilder.Append("IsLoginSuccess=" + IsLoginSuccess);
			stringBuilder.Append("FailureMessage=" + ErrorMessage);
			stringBuilder.Append("Ip=" + Ip);
			stringBuilder.Append("Ip=" + Ip);
			stringBuilder.Append("BrowserRequestHeader=" + BrowserRequestHeader);
			return stringBuilder.ToString();
		}


	}
}