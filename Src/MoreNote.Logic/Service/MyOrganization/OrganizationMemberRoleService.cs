using MoreNote.Logic.Database;
using MoreNote.Models.Entity.Leanote.MyOrganization;
using MoreNote.Models.Entity.Leanote.MyRepository;
using MoreNote.Models.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.MyOrganization
{
    public class OrganizationMemberRoleService
    {
        private DataContext dataContext;

        public OrganizationMemberRoleService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public OrganizationMemberRole GetOrganizationMemberRole(long? roleId)
        {
            var role = dataContext.OrganizationMemberRole.Where(b => b.Id == roleId).FirstOrDefault();
            return role;
        }
     

        public HashSet<OrganizationAuthorityEnum> GetOrganizationAuthoritySet(long? roleId)
        {
            var set=new HashSet<OrganizationAuthorityEnum>();

            var list=dataContext.OrganizationMemberRoleMapping.Where(x => x.RoleId == roleId).ToList<OrganizationMemberRoleMapping>();

            foreach (var item in list)
            {
                if (!set.Contains(item.AuthorityEnum))
                {
                    set.Add(item.AuthorityEnum);
                }
               
            }
            return set;

        }
    }
}