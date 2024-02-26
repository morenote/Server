using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Management
{
	/// <summary>
	/// 用户建议
	/// </summary>
	[Table("suggestion")]
	public class Suggestion : BaseEntity
	{

		[Column("user_id")]
		public long? UserId { get; set; }
		[Column("addr")]
		public string Addr { get; set; }

	}
}
