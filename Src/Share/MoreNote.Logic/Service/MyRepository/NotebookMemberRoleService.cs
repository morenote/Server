using MoreNote.Logic.Database;
using MoreNote.Models.Entity.Leanote.AccessControl;
using MoreNote.Models.Enums;

using System.Collections.Generic;
using System.Linq;

namespace MoreNote.Logic.Service.MyRepository
{
    public class NotebookMemberRoleService
    {
        private DataContext dataContext;

        public NotebookMemberRoleService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }


        public HashSet<NotebookAuthorityEnum> GetRepositoryAuthoritySet(long? roleId)
        {
            var set = new HashSet<NotebookAuthorityEnum>();

            var list = dataContext.RepositoryMemberRoleMapping.Where(x => x.RoleId == roleId).ToList();

            foreach (var item in list)
            {
                if (!set.Contains(item.NotebookAuthorityEnum))
                {
                    set.Add(item.NotebookAuthorityEnum);
                }

            }
            return set;

        }
    }
}