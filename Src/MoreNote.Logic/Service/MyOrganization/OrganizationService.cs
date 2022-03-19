using MoreNote.Logic.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.MyOrganization
{
    public class OrganizationService
    {
        private DataContext dataContext;

        public OrganizationService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
    }
}