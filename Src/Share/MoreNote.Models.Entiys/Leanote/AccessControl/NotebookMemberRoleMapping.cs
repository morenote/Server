using Morenote.Models.Models.Entity;

using MoreNote.Models.Enums;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.AccessControl
{

	[Table("notebook_member_role_mapping")]
	public class NotebookMemberRoleMapping : BaseEntity
	{
		[Column("role_id")]
		public long? RoleId { get; set; }

		[Column("authority")]
		public NotebookAuthorityEnum NotebookAuthorityEnum { get; set; }


	}
}
