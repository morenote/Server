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
    public class OrganizationTeam
    {
        [Key]
        [Column("id")]
        public long? Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("organization_id")]
        public long? OrganizationId { get; set; }

        [Column("description")]
        public string? Description { get; set; }//仓库摘要说明
    }
}