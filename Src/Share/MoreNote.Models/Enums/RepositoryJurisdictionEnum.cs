namespace MoreNote.Models.Enums
{
	/// <summary>
	/// 访问权限枚举
	/// </summary>
	public enum NotebookAuthorityEnum
	{
		/**
         * 拥有者默认具有超级管理员权限
         * 对于属于组织的仓库，由组织的权限策略决定
         */
		SuperAdmin = 10,//超级管理员权限，


		Read = 101,//读取
		Write = 102,//写入
		CreatIssue = 103,//创建议题
		ManagementIssue = 104,//管理议题

		Comment = 201,//评论
		ManagementComment = 202,//管理评论

		Clone = 301,//克隆到本地
		Pull = 302,//拉取
		Push = 303,//推送
		Fork = 304,//Fork
		PullRequest = 305,//提交修改请求

		CreatBranch = 401,//创建分支
		PushBranch = 402,//推送分支
		DeleteBranch = 403,//删除分支

		CreatTag = 501,//创建里程碑
		DeleteTag = 502,//删除里程碑
		ChangeTag = 503,//修改已经存在的里程碑

		ManagementMember = 601,//管理成员

		Approval = 701,//审批权限

		ManagementRepositoryProperties = 801,//管理仓库属性
		DeleteRepository = 802//删除仓库
	}
}
