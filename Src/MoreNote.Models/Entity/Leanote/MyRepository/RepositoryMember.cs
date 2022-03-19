using MoreNote.Models.Enum;

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
    /// 仓库成员
    /// </summary>
    /// 
    [Table("repository_member")]
    public class RepositoryMember
    {

        [Key]
        [Column("id")]
        public long? Id { get; set; } // ID

        [Column("role_id")]
        public long? RoleId { get; set; } // 角色ID

        [Column("respository_id")]
        public long? RespositoryId { get; set; } // 角色ID

        [Column("accessor_id")]
        public long? AccessorId { get; set; }   //用户ID或者团队Id

        [Column("accessor_type")]
        public RepositoryAccessorType RepositoryAccessorType { get; set; }//访问者类型

    }
}
