using MoreNote.Common.Utils;
using MoreNote.Logic.Database;
using MoreNote.Logic.Service.MyOrganization;
using MoreNote.Models.Entity.Leanote;
using MoreNote.Models.Enum;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreNote.Logic.Service.DistributedIDGenerator;

namespace MoreNote.Logic.Service.MyRepository
{
    public class RepositoryService
    {
        private DataContext dataContext;
        private OrganizationTeamService organizationTeamService;
        private RepositoryMemberRoleService memberRoleService;
        private ConfigFileService ConfigFileService;
        private IDistributedIdGenerator idGenerator;
        public RepositoryService(DataContext dataContext,
            OrganizationTeamService organizationTeamService,
            ConfigFileService configFileService,
            RepositoryMemberRoleService repositoryMemberRoleService,
            IDistributedIdGenerator idGenerator)
        {
            this.idGenerator=idGenerator;
            this.dataContext = dataContext;
            this.organizationTeamService = organizationTeamService;
            this.memberRoleService = repositoryMemberRoleService;
            this.ConfigFileService = configFileService;
        }

        public Repository GetRepository(long? Id)
        {
            return dataContext.Repository.Where(b => b.Id == Id&&b.IsDelete==false ).FirstOrDefault();
        }

        public List<Repository> GetRepositoryList(long? userId, RepositoryType repositoryType)
        {
            var list = dataContext.Repository.Where(b => b.OwnerId == userId&& b.IsDelete==false && b.RepositoryType == repositoryType).ToList<Repository>();
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
        public HashSet<RepositoryAuthorityEnum> GetRepositoryAccessPermissions(long? respositoryId, long? userId)
        {
            var memerRole = GetRepositoryMemberRole(respositoryId);
            if (memerRole==null)
            {
                return new HashSet<RepositoryAuthorityEnum>();

            }
            var set = this.memberRoleService.GetRepositoryAuthoritySet(memerRole.Id);
            return set;
        }

        private void AddRepository(Repository repository)
        {
           dataContext.Repository.Add(repository);    
           dataContext.SaveChanges();
        }

        public Repository CreateRepository(Repository repository)
        {
        

            var addRepository = new Repository()
            {
                Id = idGenerator.NextId(),
                Name = repository.Name,
                Description = repository.Description,
                License = repository.License,
                RepositoryOwnerType = repository.RepositoryOwnerType,
                RepositoryType = repository.RepositoryType,
                OwnerId = repository.OwnerId,
                Visible = repository.Visible,
                CreateTime = DateTime.Now,
                Avatar= repository.Avatar,
                VirtualFileBasePath= repository.VirtualFileBasePath,
                VirtualFileAccessor = repository.VirtualFileAccessor

            };
            this.AddRepository(addRepository);
            return addRepository;
        }
        public void DeleteRepository(long? respositoryId)
        {
            dataContext.Repository.Where(b=>b.Id==respositoryId).UpdateFromQuery(x=>new Repository() { IsDelete=true}); 
            dataContext.SaveChanges();
        }

        public bool ExistRepositoryByName(long? ownerId, string name)
        {
            return dataContext.Repository.Where(x => x.Name == name &&x.OwnerId==ownerId && x.IsDelete==false).Any();
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
            var respository=GetRepository(respositoryId);
            if (respository == null)
            {
                return false;
            }
            if (respository.OwnerId == userId)
            {
                return true;//拥有者 拥有任意权限
            }

            var accessPermissions = GetRepositoryAccessPermissions(respositoryId, userId);
            if (accessPermissions == null)
            {
                return false;
            }
            return accessPermissions.Contains(repositoryAuthorityEnum);
        }

        public bool Verify(long? respositoryId, long? userId, HashSet<RepositoryAuthorityEnum> repositoryAuthorityEnumList)
        {

            var respository = GetRepository(respositoryId);
            if (respository.OwnerId == userId)
            {
                return true;//拥有者 拥有任意权限
            }

            //获取权限列表
            var accessPermissions = GetRepositoryAccessPermissions(respositoryId, userId);
            if (accessPermissions == null)
            {
                return false;
            }
            foreach (var item in repositoryAuthorityEnumList)
            {
                if (!accessPermissions.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
        //增加仓库计数器
        public int IncrUsn(long? repositoryId)
        {
            var repository = dataContext.Repository
               .Where(b => b.Id == repositoryId).FirstOrDefault();
            repository.Usn += 1;
            dataContext.SaveChanges();
            
            return repository.Usn;
        }
    }
}