using MoreNote.Logic.Database;

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
    }
}