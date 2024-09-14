using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.AccessControl
{
	/// <summary>
	/// 访问权限角色
	/// </summary>
	[Table("notebook_member_Role")]
	public class NotebookMemberRole : BaseEntity
	{



		/// <summary>
		/// 角色名称
		/// </summary>
		[Column("role_name")]
		public string? RoleName { set; get; }


		[Column("notebook_id")]
		public long? NotebookId { get; set; } // 仓库id

	}
}
