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

    [Table("organization_member")]
    public class OrganizationMember: BaseEntity
    {
      

      
        [Column("organization_id")]
        public long? OrganizationId { get; set; }

        [Column("role_id")]
        public long? RoleId { get; set; } // 角色ID

        [Column("user_id")]
        public long? UserId { get; set; }
    }
}