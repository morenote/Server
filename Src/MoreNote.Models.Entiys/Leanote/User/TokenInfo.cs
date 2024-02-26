using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.User
{
	// 随机token
	// 验证邮箱
	// 找回密码
	[Table("token")]
	public class Token : BaseEntity
	{

		[Column("user_id")]
		public long? UserId { get; set; }
		[Column("email")]
		public string? Email { get; set; }
		//使用token来授权第三方应用使用你的笔记数据
		//当使用API登录时，总是假定你的登录方式是使用token方式获得授权
		//token的规格是16位随机字符串 yk1I-W8o8-FwC0-O2vO 但是不保证后期版本升级后长度发生变化
		//登录第三方应用时，密码使用token
		//leanoteAPI允许用户只使用Token进行登录，那么应该保证token的唯一性
		[Column("token_str")]
		public string TokenStr { get; set; }
		[Column("token_tag")]
		public string? TokenTag { get; set; }//token标签
		[Column("token_type")]
		public int TokenType { get; set; }//token类型
		[Column("created_time")]
		public DateTime CreatedTime { get; set; }

	}
}
