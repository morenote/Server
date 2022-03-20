using MoreNote.Logic.Database;
using MoreNote.Models.Entity.Leanote.MyOrganization;
using MoreNote.Models.Enum;

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
        private OrganizationMemberRoleService memberRoleService;
        public OrganizationService(DataContext dataContext,OrganizationMemberRoleService organizationMemberRoleService)
        {
            this.dataContext = dataContext;
            this.memberRoleService = organizationMemberRoleService;
        }
        /// <summary>
        /// 获取我的组织
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Organization> GetMyOrganization(long userId)
        {
            var list=dataContext.Organization.Where(b=>b.OwnerId==userId).ToList<Organization>();
            return list;
        }

        public Organization GetOrganizationById(long? orgId)
        {
            var org = dataContext.Organization.Where(b => b.Id == orgId).FirstOrDefault();
            return org;
        }

        /// <summary>
        /// 获得用户对该组织的全部权限
        /// </summary>
        /// <param name="respositoryId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public HashSet<OrganizationAuthorityEnum> GetOrganizationAuthoritySet(long? organizationId, long? userId)
        {
            var member=this.GetOrganizationMember(organizationId,userId);
            var memberRole=memberRoleService.GetOrganizationMemberRole(member.RoleId);
            
            var set = this.memberRoleService.GetOrganizationAuthoritySet(memberRole.Id);

            return set;
        }

        /// <summary>
        /// 验证某个成员对某个组织具有某种权限
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="userId"></param>
        /// <param name="organizationAuthorityEnum"></param>
        /// <returns></returns>
        public  bool Verify(long? organizationId,long userId, OrganizationAuthorityEnum organizationAuthorityEnum)
        {
            var org=this.GetOrganizationById(organizationId);
            if (org.OwnerId==userId)
            {
                return true;//组织拥有者 拥有任意权限
            }

            var set=GetOrganizationAuthoritySet(organizationId,userId);
            if (set==null)
            {
                return false;
            }
            return set.Contains(organizationAuthorityEnum);
        }
        /// <summary>
        /// 获得组织内的成员
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OrganizationMember GetOrganizationMember(long? organizationId, long? userId)
        {
            return dataContext.OrganizationMember.Where(b=>b.UserId==userId).FirstOrDefault();
        }
        /// <summary>
        /// 返回我加入的组织列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Organization> GetMyJoinOrganization(long userId)
        {
            var orgs=dataContext.OrganizationMember.Where(b=>b.UserId==userId).ToList<OrganizationMember>();
 
            var list = (from org in dataContext.Set<Organization>()
                       join member in dataContext.Set<OrganizationMember>()
                       on org.Id equals member.Id
                       where member.UserId == userId
                       select new Organization());
            return list.ToList<Organization>();
        }
        /// <summary>
        /// 返回符合某种权限要求的组织列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="organizationAuthorityEnum"></param>
        /// <returns></returns>
        public List<Organization> GetMyJoinOrganization(long userId, OrganizationAuthorityEnum organizationAuthorityEnum)
        {
            var list=GetMyJoinOrganization(userId);
            var myOrgs=GetMyOrganization(userId);
            var result=new List<Organization>();
            result.AddRange(myOrgs);
            foreach (var item in list)
            {
                if (Verify(item.Id,userId,organizationAuthorityEnum))
                {
                    result.Add(item);
                }
            }
            return result;
        }





    }
}