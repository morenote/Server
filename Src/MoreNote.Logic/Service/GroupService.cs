using MoreNote.Logic.Entity;
using MoreNote.Models.Entity.Leanote.User;

using System;

namespace MoreNote.Logic.Service
{
	public class GroupService
	{

		// 添加分组
		public bool AddGroup(long? userId, string title)
		{
			throw new Exception();
		}
		// 删除分组
		// 判断是否有好友
		public bool DeleteGroup(long? userId, long? groupId)
		{
			throw new Exception();
		}
		// 修改group标题
		public bool UpdateGroupTitle(long? userId, long? groupId, string title)
		{
			throw new Exception();
		}
		// 得到用户的所有分组(包括下的所有用户)
		public GroupTeam[] GetGroupsAndUsers(long? userId)
		{
			throw new Exception();
		}
		// 仅仅得到所有分组
		public GroupTeam[] GetGroups(long? userId)
		{
			throw new Exception();
		}
		// 得到我的和我所属组的ids
		public long[] GetMineAndBelongToGroupIds(long? userId)
		{
			throw new Exception();
		}

		// 获取包含此用户的组对象数组
		// 获取该用户所属组, 和我的组
		public GroupTeam[] GetGroupsContainOf(long? userId)
		{
			throw new Exception();
		}
		// 得到分组, shareService用
		public GroupTeam GetGroup(long? userId, long? groupId)
		{
			throw new Exception();
		}
		// 得到某分组下的用户
		public UserInfo[] GetUsers(long? groupId)
		{
			throw new Exception();
		}
		// 得到我所属的所有分组ids
		public long[] GetBelongToGroupIds(long? userId)
		{
			throw new Exception();
		}
		public bool isMyGroup(long? ownUserIdmlong, long? groupId)
		{
			throw new Exception();
		}
		// 判断组中是否包含指定用户
		public bool IsExistsGroupUser(long? userId, long? groupId)
		{
			throw new Exception();
		}
		// 为group添加用户
		// 用户是否已存在?
		public bool AddUser(long? ownUserId, long? groupId, long? userId)
		{
			throw new Exception();
		}
		// 删除用户
		public bool DeleteUser(long? ownUserId, long? groupId, long? userId)
		{
			throw new Exception();
		}
	}
}
