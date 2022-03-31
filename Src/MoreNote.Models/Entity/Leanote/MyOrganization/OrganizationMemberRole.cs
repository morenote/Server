﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.MyOrganization
{

    [Table("organization_member_role")]
    public class OrganizationMemberRole
    {
        [Key]
        [Column("id")]
        public long? Id { get; set; }

      
        [Column("organization_id")]
        public long? OrganizationId { get; set; }


        [Column("role_name")]
        public string? RoleName { get; set; }
    }
}