using Morenote.Models.Models.Entity;

using MoreNote.Models.Enums;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote
{
    /// <summary>
    /// 仓库访问权限角色
    /// </summary>
    [Table("repository_member_Role")]
    public class RepositoryMemberRole: BaseEntity
    {

       

        /// <summary>
        /// 角色名称
        /// </summary>
        [Column("role_name")]
        public string? RoleName { set; get; }


        [Column("repository_id")]
        public long? RepositoryId { get; set; } // 仓库id

    }
}
