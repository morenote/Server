
using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Logic.Entity
{
	/// <summary>
	/// 专辑
	/// </summary>
	[Table("album")]
	public class Album : BaseEntity
	{

		[Column("user_id")]
		public long? UserId { get; set; }
		[Column("name")]
		public string Name { get; set; }// album name
		[Column("type")]
		public int Type { get; set; }// type, the default is image: 0
		[Column("seq")]
		public int SEQ { get; set; }
		[Column("created_time")]
		public DateTime CreatedTime { get; set; }


	}
}
