using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Enums
{
    public enum OrganizationAuthorityEnum
    {
        Read = 101,
        Write = 102,

        ManagementMember =201,

        AddRepository = 301,
        DelteRepository = 302,
        EditeRepository= 303,
        ManagementRepository = 304,


        ManagementOrganizationProperties = 401,

        Approval = 501,
        

    }
}
