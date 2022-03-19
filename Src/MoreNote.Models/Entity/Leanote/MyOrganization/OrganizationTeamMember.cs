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
    public class OrganizationTeamMember
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("team_id")]
        public long TeamId { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }
    }
}