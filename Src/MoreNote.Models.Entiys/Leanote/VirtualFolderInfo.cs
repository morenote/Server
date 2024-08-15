using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote
{
	[Table("virtual_folder_info")]
	public class VirtualFolderInfo : BaseEntity
	{
		[Column("name")]
		public string? Name { get; set; }
		[Column("modify_date")]
		public DateTime? ModifyDate { get; set; }

		[Column("parent_id")]
		public long? ParentId { get; set; }
		[Column("repository_id")]
		public long? RepositoryId { get; set; }

		[Column("is_delete")]
		public bool? IsDelete { get; set; }

	}
}
