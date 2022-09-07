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
    /// <summary>
    /// 团队中的成员
    /// </summary>
    [Table("organization_team_member")]
    public class OrganizationTeamMember:BaseEntity
    {
        [Key]
        [Column("id")]
        public long? Id { get; set; }

        [Column("team_id")]
        public long? TeamId { get; set; }

        [Column("user_id")]
        public long? UserId { get; set; }
    }
}