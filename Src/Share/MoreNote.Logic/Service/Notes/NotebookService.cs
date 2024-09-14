using MoreNote.Logic.Database;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Logic.Service.MyOrganization;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Models.Entity.Leanote;
using MoreNote.Models.Entity.Leanote.AccessControl;
using MoreNote.Models.Entiys.Leanote.Notes;
using MoreNote.Models.Enums;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreNote.Logic.Service.Notes
{
    public class NotebookService
    {

        private DataContext dataContext;
        private OrganizationTeamService organizationTeamService;
        private NotebookMemberRoleService memberRoleService;
        private ConfigFileService ConfigFileService;
        private IDistributedIdGenerator idGenerator;
        private SubmitTreeService SubmitTreeService;

        public NotebookService(DataContext dataContext,
            OrganizationTeamService organizationTeamService,
            ConfigFileService configFileService,
            NotebookMemberRoleService repositoryMemberRoleService,
            IDistributedIdGenerator idGenerator,
            SubmitTreeService submitTreeService)
        {
            this.idGenerator = idGenerator;
            this.dataContext = dataContext;
            this.organizationTeamService = organizationTeamService;
            memberRoleService = repositoryMemberRoleService;
            ConfigFileService = configFileService;
            SubmitTreeService = submitTreeService;
        }

        public Notebook GetNotebook(long? Id)
        {
            return dataContext.Notebook.Where(b => b.Id == Id && b.IsDelete == false).FirstOrDefault();
        }

        public List<Notebook> GetNotebookList(long? userId, NotebookType repositoryType)
        {
            var list = dataContext.Notebook.Where(b => b.OwnerId == userId && b.IsDelete == false && b.NotebookType == repositoryType).ToList();
            return list;
        }
        /// <summary>
        /// 获取用户的仓库角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="respositoryId"></param>
        /// <returns></returns>
        public NotebookMemberRole GetNotebookMemberRole(long? userId, long? respositoryId)
        {
            var members = GetNotebookMember(respositoryId);

            foreach (var member in members)
            {
                if (member.RepositoryAccessorType == RepositoryMemberType.Personal && member.AccessorId == userId)
                {
                    return GetNotebookMemberRole(member.RoleId);
                }
                else
                {
                    if (organizationTeamService.ExistUser(member.AccessorId, userId))
                    {
                        return GetNotebookMemberRole(member.RoleId);
                    }
                }
            }

            return null;
        }

        public NotebookMemberRole GetNotebookMemberRole(long? roleId)
        {
            var role = dataContext.RepositoryMemberRole.Where(b => b.Id == roleId).FirstOrDefault();
            return role;
        }
        /// <summary>
        /// 获取的全部成员
        /// </summary>
        /// <param name="respositoryId"></param>
        /// <returns></returns>
        public List<NotebookMember> GetNotebookMember(long? respositoryId)
        {
            var members = dataContext.RepositoryMember.Where(b => b.NotebookId == respositoryId).ToList();
            return members;
        }
        /// <summary>
        /// 获得用户对该仓库的全部权限
        /// </summary>
        /// <param name="respositoryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public HashSet<NotebookAuthorityEnum> GetRepositoryAccessPermissions(long? respositoryId, long? userId)
        {
            var memerRole = GetNotebookMemberRole(respositoryId);
            if (memerRole == null)
            {
                return new HashSet<NotebookAuthorityEnum>();

            }
            var set = memberRoleService.GetRepositoryAuthoritySet(memerRole.Id);
            return set;
        }

        private void AddNotebook(Notebook notebook)
        {
            dataContext.Notebook.Add(notebook);
            dataContext.SaveChanges();
        }

        public Notebook CreateNotebook(Notebook notebook)
        {
            var tempBook = new Notebook()
            {
                Id = idGenerator.NextId(),
                Name = notebook.Name,
                Description = notebook.Description,
                License = notebook.License,
                NotebookOwnerType = notebook.NotebookOwnerType,
                NotebookType = notebook.NotebookType,
                OwnerId = notebook.OwnerId,
                Visible = notebook.Visible,
                CreateTime = DateTime.Now,
                Avatar = notebook.Avatar,
                VirtualFileBasePath = notebook.VirtualFileBasePath,
                VirtualFileAccessor = notebook.VirtualFileAccessor

            };
            AddNotebook(tempBook);
            SubmitTreeService.InitTree(tempBook.Id);//初始化这个仓库的提交树
            return tempBook;
        }
        public void DeleteNotebook(long? id)
        {
            dataContext.Notebook.Where(b => b.Id == id).UpdateFromQuery(x => new Notebook() { IsDelete = true });
            dataContext.SaveChanges();
        }

        public bool ExistNotebookByName(long? ownerId, string name)
        {
            return dataContext.Notebook.Where(x => x.Name == name && x.OwnerId == ownerId && x.IsDelete == false).Any();
        }

        private string GetDefaultAvatar()
        {
            var url = ConfigFileService.ReadConfig().APPConfig.SiteUrl;
            var avatar = url + @"/images/avatar/antd.png";
            return avatar;
        }
        /// <summary>
        ///  检验某个用户是否对仓库具有某种权限
        /// </summary>
        /// <param name="respositoryId"></param>
        /// <param name="userId"></param>
        /// <param name="repositoryAuthorityEnum"></param>
        /// <returns></returns>
        public bool Verify(long? respositoryId, long? userId, NotebookAuthorityEnum repositoryAuthorityEnum)
        {
            var respository = GetNotebook(respositoryId);
            if (respository == null)
            {
                return false;
            }
            if (respository.OwnerId == userId)
            {
                return true;//拥有者 拥有任意权限
            }
            //如果是公开仓库，表示任意人都有读取权限
            if (respository.Visible && repositoryAuthorityEnum == NotebookAuthorityEnum.Read)
            {
                return true;
            }
            var accessPermissions = GetRepositoryAccessPermissions(respositoryId, userId);
            if (accessPermissions == null)
            {
                return false;
            }

            return accessPermissions.Contains(repositoryAuthorityEnum);
        }

        public bool Verify(long? respositoryId, long? userId, HashSet<NotebookAuthorityEnum> repositoryAuthorityEnumList)
        {

            var respository = GetNotebook(respositoryId);
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
        public int IncrUsn(long? id)
        {
            var repository = dataContext.Notebook
               .Where(b => b.Id == id).FirstOrDefault();
            repository.Usn += 1;
            dataContext.SaveChanges();

            return repository.Usn;
        }
    }
}