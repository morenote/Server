using MoreNote.Logic.Database;
using MoreNote.Models.Entity.Leanote.MyRepository;
using MoreNote.Models.Enum;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.MyRepository
{
    public class RepositoryMemberRoleService
    {
        private DataContext dataContext;

        public RepositoryMemberRoleService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }


        public HashSet<RepositoryAuthorityEnum> GetRepositoryAuthoritySet(long? roleId)
        {
            var set=new HashSet<RepositoryAuthorityEnum>();

            var list=dataContext.RepositoryMemberRoleMapping.Where(x => x.RoleId == roleId).ToList<RepositoryMemberRoleMapping>();

            foreach (var item in list)
            {
                if (!set.Contains(item.RepositoryAuthorityEnum))
                {
                    set.Add(item.RepositoryAuthorityEnum);
                }
               
            }
            return set;

        }
    }
}