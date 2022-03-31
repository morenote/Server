﻿using MoreNote.Common.Utils;
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
        private RepositoryMemberRoleService memberRoleService;
        private ConfigFileService ConfigFileService;
        public NoteRepositoryService(DataContext dataContext,
            OrganizationTeamService organizationTeamService,
            ConfigFileService configFileService,
            RepositoryMemberRoleService repositoryMemberRoleService)
        {
            this.dataContext = dataContext;
            this.organizationTeamService = organizationTeamService;
            this.memberRoleService = repositoryMemberRoleService;
            this.ConfigFileService = configFileService;
        }

        public NotesRepository GetNotesRepository(long? Id)
        {
            return dataContext.NotesRepository.Where(b => b.Id == Id&&b.IsDelete==false).FirstOrDefault();
        }

        public List<NotesRepository> GetNoteRepositoryList(long? userId)
        {
            var list = dataContext.NotesRepository.Where(b => b.OwnerId == userId&& b.IsDelete==false).ToList<NotesRepository>();
            return list;
        }
        /// <summary>
        /// 获取用户的仓库角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="respositoryId"></param>
        /// <returns></returns>
        public RepositoryMemberRole GetRepositoryMemberRole(long? userId, long? respositoryId)
        {
            var members = GetRepositoryMember(respositoryId);

            foreach (var member in members)
            {
                if (member.RepositoryAccessorType == RepositoryMemberType.Personal && member.AccessorId == userId)
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
        /// <summary>
        /// 获取仓库的全部成员
        /// </summary>
        /// <param name="respositoryId"></param>
        /// <returns></returns>
        public List<RepositoryMember> GetRepositoryMember(long? respositoryId)
        {
            var members = dataContext.RepositoryMember.Where(b => b.RespositoryId == respositoryId).ToList<RepositoryMember>();
            return members;
        }
        /// <summary>
        /// 获得用户对该仓库的全部权限
        /// </summary>
        /// <param name="respositoryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public HashSet<RepositoryAuthorityEnum> GetRepositoryAuthoritySet(long? respositoryId, long? userId)
        {
            var memerRole = GetRepositoryMemberRole(respositoryId);
            if (memerRole==null)
            {
                return new HashSet<RepositoryAuthorityEnum>();

            }
            var set = this.memberRoleService.GetRepositoryAuthoritySet(memerRole.Id);
            return set;
        }

        private void AddNoteRepository(NotesRepository notesRepository)
        {
           dataContext.NotesRepository.Add(notesRepository);    
           dataContext.SaveChanges();
        }

        public NotesRepository CreateNoteRepository(NotesRepository notesRepository)
        {
        
            var addNoteRepositoryService = new NotesRepository()
            {
                Id = SnowFlakeNet.GenerateSnowFlakeID(),
                Name = notesRepository.Name,
                Description = notesRepository.Description,
                License = notesRepository.License,
                RepositoryOwnerType = notesRepository.RepositoryOwnerType,
                OwnerId = notesRepository.OwnerId,
                Visible = notesRepository.Visible,
                CreateTime = DateTime.Now,
                Avatar=this.GetDefaultAvatar()
                
            };
            this.AddNoteRepository(addNoteRepositoryService);
            return addNoteRepositoryService;
        }
        public void DeleteNoteRepository(long? respositoryId)
        {
            dataContext.NotesRepository.Where(b=>b.Id==respositoryId).UpdateFromQuery(x=>new NotesRepository() { IsDelete=true}); 
            dataContext.SaveChanges();
        }

        public bool ExistNoteRepositoryByName(long? ownerId, string name)
        {
            return dataContext.NotesRepository.Where(x => x.Name == name &&x.OwnerId==ownerId && x.IsDelete==false).Any();
        }

        private string GetDefaultAvatar()
        {
            var url=   this.ConfigFileService.WebConfig.APPConfig.SiteUrl;
            var avatar = url+ @"/images/avatar/antd.png";
            return avatar;
        }
        /// <summary>
        ///  检验某个用户是否对仓库具有某种权限
        /// </summary>
        /// <param name="respositoryId"></param>
        /// <param name="userId"></param>
        /// <param name="repositoryAuthorityEnum"></param>
        /// <returns></returns>
        public bool Verify(long? respositoryId, long? userId, RepositoryAuthorityEnum repositoryAuthorityEnum)
        {
            var respository=GetNotesRepository(respositoryId);
            if (respository == null)
            {
                return false;
            }
            if (respository.OwnerId == userId)
            {
                return true;//拥有者 拥有任意权限
            }

            var set = GetRepositoryAuthoritySet(respositoryId, userId);
            if (set == null)
            {
                return false;
            }
            return set.Contains(repositoryAuthorityEnum);
        }

        public bool Verify(long? respositoryId, long? userId, HashSet<RepositoryAuthorityEnum> repositoryAuthorityEnumList)
        {

            var respository = GetNotesRepository(respositoryId);
            if (respository.OwnerId == userId)
            {
                return true;//拥有者 拥有任意权限
            }


            var set = GetRepositoryAuthoritySet(respositoryId, userId);
            if (set == null)
            {
                return false;
            }
            foreach (var item in repositoryAuthorityEnumList)
            {
                if (!set.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public int IncrUsn(long? repositoryId)
        {
            var repository = dataContext.NotesRepository
               .Where(b => b.Id == repositoryId).FirstOrDefault();
            repository.Usn += 1;
            dataContext.SaveChanges();
            
            return repository.Usn;
        }
    }
}