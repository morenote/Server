using MoreNote.Logic.Database;
using MoreNote.Logic.Service.MyOrganization;
using MoreNote.Models.Entity.Leanote;
using MoreNote.Models.Enum;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.MyRepository
{
    public class NoteRepositoryService
    {
        private DataContext dataContext;
        private OrganizationTeamService organizationTeamService;

        public NoteRepositoryService(DataContext dataContext, OrganizationTeamService organizationTeamService)
        {
            this.dataContext = dataContext;
            this.organizationTeamService = organizationTeamService;
        }

        public NotesRepository GetNotesRepository(long? Id)
        {
            return dataContext.NotesRepository.Where(b => b.Id == Id).FirstOrDefault();
        }

        public List<NotesRepository> GetNoteRepositoryList(long? userId)
        {
            var list = dataContext.NotesRepository.Where(b => b.OwnerId == userId).ToList<NotesRepository>();
            return list;
        }

        public RepositoryMemberRole GetRepositoryMemberRole(long? userId, long? respositoryId)
        {
            var members = GetRepositoryMember(respositoryId);

            foreach (var member in members)
            {
                if (member.RepositoryAccessorType == RepositoryAccessorType.Personal && member.AccessorId == userId)
                {
                    return GetRepositoryMemberRole(member.RoleId);
                }
                else
                {
                    if (organizationTeamService.ExistUser(member.AccessorId, userId))
                    {
                        return GetRepositoryMemberRole(member.RoleId);
                    }
                }
            }

            return null;
        }

        public RepositoryMemberRole GetRepositoryMemberRole(long? roleId)
        {
            var role = dataContext.RepositoryMemberRole.Where(b => b.Id == roleId).FirstOrDefault();
            return role;
        }

        public List<RepositoryMember> GetRepositoryMember(long? respositoryId)
        {
            var members = dataContext.RepositoryMember.Where(b => b.RespositoryId == respositoryId).ToList<RepositoryMember>();
            return members;
        }
    }
}