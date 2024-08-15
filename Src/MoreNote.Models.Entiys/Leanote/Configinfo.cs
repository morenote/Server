using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Logic.Entity
{
	// 配置, 每一个配置一行记录
	[Table("config")]
	public class Config : BaseEntity
	{

		[Column("user_id")]
		public long? UserId { get; set; }
		[Column("key")]
		public string Key { get; set; }
		[Column("value_str")]
		public string ValueStr { get; set; }



	}
}
