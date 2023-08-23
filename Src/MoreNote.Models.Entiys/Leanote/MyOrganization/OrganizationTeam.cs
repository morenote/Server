using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.MyOrganization
{
    [Table("organization_team")]
    public class OrganizationTeam : BaseEntity
    {
        

        [Column("name")]
        public string? Name { get; set; }

        [Column("organization_id")]
        public long? OrganizationId { get; set; }

        [Column("role_id")]
        public long? RoleId { get; set; } // 角色ID

        [Column("description")]
        public string? Description { get; set; }//仓库摘要说明
    }
}