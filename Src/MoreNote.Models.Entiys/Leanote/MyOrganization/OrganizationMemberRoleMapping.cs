using Morenote.Models.Models.Entity;

using MoreNote.Models.Enums;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.MyOrganization
{

    [Table("organization_member_role_mapping")]
    public class OrganizationMemberRoleMapping:BaseEntity
    {
        

        [Column("role_id")]
        public long? RoleId { get; set; }

        [Column("authority")]
        public OrganizationAuthorityEnum AuthorityEnum { get; set; }
    }
}