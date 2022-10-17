using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote
{
    [Table("virtual_file_info")]
    public class VirtualFileInfo: BaseEntity
    {
        [Column("name")]
        public string? Name { get;set;}
        [Column("modify_date")]
        public DateTime? ModifyDate { get;set;}
        [Column("size")]
        public long? Size { get;set;}

        [Column("parent_id")]
        public long? ParentId { get;set;}
        [Column("repository_id")]
        public long? RepositoryId { get;set;}

        [Column("is_delete")]
        public bool? IsDelete { get; set; }
    }
}
