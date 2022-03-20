using MoreNote.Logic.Database;
using MoreNote.Models.Entity.Leanote.MyOrganization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.MyOrganization
{
    public class OrganizationTeamService
    {
        private DataContext dataContext;

        public OrganizationTeamService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public OrganizationTeam GetOrganizationTeamsByTeamId(long teamId)
        {
            var team = dataContext.OrganizationTeam.Where(b => b.Id == teamId).FirstOrDefault();

            return team;
        }

        public List<OrganizationTeam> GetOrganizationTeamsByOrganizationId(long organizationId)
        {
            var teams = dataContext.OrganizationTeam.Where(b => b.OrganizationId == organizationId).ToList<OrganizationTeam>();

            return teams;
        }

        public List<OrganizationTeamMember> GetOrganizationTeamMembers(long teamId)
        {
            var members = dataContext.OrganizationTeamMember.Where(b => b.Id == teamId).ToList<OrganizationTeamMember>();

            return members;
        }
     



        public bool ExistUser(long? teamId, long? userId)
        {
            var result = dataContext.OrganizationTeamMember.Where(b => b.Id == teamId && b.UserId == userId).Any();

            return result;
        }
    }
}