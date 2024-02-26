using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.User
{
	[Table("session")]
	public class Session : BaseEntity
	{

		[Column("session_id")]
		public long? SessionId { get; set; } // SessionId 
		[Column("login_times")]
		public int LoginTimes { get; set; } // 登录错误时间 
		[Column("captcha")]
		public string Captcha { get; set; } // 验证码 
		[Column("user_id")]
		public long? UserId { get; set; } // API时有值UserId 
		[Column("created_time")]
		public DateTime CreatedTime { get; set; }
		[Column("updated_time")]
		public DateTime UpdatedTime { get; set; } // 更新时间, 

	}
}
