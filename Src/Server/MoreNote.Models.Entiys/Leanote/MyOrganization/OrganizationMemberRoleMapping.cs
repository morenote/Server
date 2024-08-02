using Morenote.Models.Models.Entity;

using MoreNote.Models.Enums;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.MyOrganization
{

	[Table("organization_member_role_mapping")]
	public class OrganizationMemberRoleMapping : BaseEntity
	{


		[Column("role_id")]
		public long? RoleId { get; set; }

		[Column("authority")]
		public OrganizationAuthorityEnum AuthorityEnum { get; set; }
	}
}