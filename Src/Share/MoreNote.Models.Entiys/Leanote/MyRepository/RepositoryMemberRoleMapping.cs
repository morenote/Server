using Morenote.Models.Models.Entity;

using MoreNote.Models.Enums;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.MyRepository
{

	[Table("repository_member_role_mapping")]
	public class RepositoryMemberRoleMapping : BaseEntity
	{



		[Column("role_id")]
		public long? RoleId { get; set; }

		[Column("authority")]
		public RepositoryAuthorityEnum RepositoryAuthorityEnum { get; set; }


	}
}
