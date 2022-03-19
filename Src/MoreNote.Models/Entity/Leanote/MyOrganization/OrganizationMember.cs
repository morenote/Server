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
    public class OrganizationMember
    {
        [Key]
        [Column("id")]
        public long? Id { get; set; }

      
        [Column("organization_id")]
        public long? OrganizationId { get; set; }


        [Column("user_id")]
        public long? UserId { get; set; }
    }
}