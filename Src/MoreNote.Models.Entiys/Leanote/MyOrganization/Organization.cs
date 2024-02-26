using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.MyOrganization
{
	[Table("organization")]
	public class Organization : BaseEntity
	{


		[Column("name")]
		public string? Name { get; set; }

		[Column("owner_id")]
		public long? OwnerId { get; set; }
	}
}