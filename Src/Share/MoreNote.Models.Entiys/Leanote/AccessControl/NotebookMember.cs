using Morenote.Models.Models.Entity;

using MoreNote.Models.Enums;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.AccessControl
{
	/// <summary>
	/// 笔记本成员
	/// </summary>
	/// 
	[Table("notebook_member")]
	public class NotebookMember : BaseEntity
	{



		[Column("role_id")]
		public long? RoleId { get; set; } // 角色ID

		[Column("notebook_id")]
		public long? NotebookId { get; set; } // 角色ID

		[Column("accessor_id")]
		public long? AccessorId { get; set; }   //用户ID或者团队Id
		/// <summary>
		/// 访问者类型（成员类型）
		/// </summary>
		[Column("accessor_type")]
		public RepositoryMemberType RepositoryAccessorType { get; set; }//访问者类型

	}
}
