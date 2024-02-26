using Morenote.Models.Models.Entity;

using MoreNote.Models.Enums;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote
{
	/// <summary>
	/// 仓库成员
	/// </summary>
	/// 
	[Table("repository_member")]
	public class RepositoryMember : BaseEntity
	{



		[Column("role_id")]
		public long? RoleId { get; set; } // 角色ID

		[Column("respository_id")]
		public long? RespositoryId { get; set; } // 角色ID

		[Column("accessor_id")]
		public long? AccessorId { get; set; }   //用户ID或者团队Id
		/// <summary>
		/// 仓库访问者类型（成员类型）
		/// </summary>
		[Column("accessor_type")]
		public RepositoryMemberType RepositoryAccessorType { get; set; }//访问者类型

	}
}
