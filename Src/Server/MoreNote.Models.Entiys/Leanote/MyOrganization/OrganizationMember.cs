using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.MyOrganization
{

	[Table("organization_member")]
	public class OrganizationMember : BaseEntity
	{



		[Column("organization_id")]
		public long? OrganizationId { get; set; }

		[Column("role_id")]
		public long? RoleId { get; set; } // 角色ID

		[Column("user_id")]
		public long? UserId { get; set; }
	}
}