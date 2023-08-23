using Morenote.Models.Models.Entity;

using MoreNote.Models.Enums;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.MyRepository
{

    [Table("repository_member_role_mapping")]
    public class RepositoryMemberRoleMapping: BaseEntity
    {

        

        [Column("role_id")]
        public long? RoleId { get; set; }

        [Column("authority")]
        public  RepositoryAuthorityEnum RepositoryAuthorityEnum { get;set;}


    }
}
