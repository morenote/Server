using Microsoft.EntityFrameworkCore;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.HySystem;
using MoreNote.Common.Utils;
using MoreNote.Config.ConfigFile;
using MoreNote.CryptographyProvider;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Logic.Service.Logging;
using MoreNote.Logic.Service.PasswordSecurity;
using MoreNote.Models.Entity.Leanote;
using MoreNote.Models.Entity.Leanote.Blog;
using MoreNote.Models.Entity.Leanote.User;
using MoreNote.Models.Entity.Security.FIDO2;
using MoreNote.Models.Entiys.Leanote.Notes;
using MoreNote.Models.Enums;
using MoreNote.Models.Enums.Common.Editor;
using MoreNote.Models.Model;
using MoreNote.SecurityProvider.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service
{
	public class UserService
	{
		private DataContext dataContext;
		public BlogService BlogService { get; set; }
		public EmailService EmailService { get; set; }
		public WebSiteConfig Config { get; set; }
		private IDistributedIdGenerator idGenerator;
		private PasswordStoreFactory passwordStoreFactory;

		private ILoggingService logging { get; set; }

		protected ICryptographyProvider cryptographyProvider { get; set; }
		public UserService(DataContext dataContext, IDistributedIdGenerator idGenerator,
			ConfigFileService configFileService,
			PasswordStoreFactory passwordStoreFactory,
			ILoggingService logging,
			ICryptographyProvider cryptographyProvider)
		{
			this.idGenerator = idGenerator;
			this.dataContext = dataContext;
			this.Config = configFileService.ReadConfig();
			this.passwordStoreFactory = passwordStoreFactory;
			this.cryptographyProvider = cryptographyProvider;
			this.logging = logging;
		}

		public UserLoginSecurityStrategy GetGetUserLoginSecurityStrategy(string userName)
		{


			var user = GetUserByUserName(userName);
			if (user == null)
			{
				return null;

			}
			var securityStrategy = new UserLoginSecurityStrategy()
			{
				UserId = user.Id,
				UserName = user.Username,

			};
			return securityStrategy;



		}
		public UserInfo GetUserByToken(string token)
		{
			if (token == null)
			{
				return null;
			}
			var result = dataContext.Token
				  .Where(b => b.TokenStr.Equals(token)).FirstOrDefault();
			if (result != null)
			{
				var user = dataContext.User
				.Where(b => b.Id == result.UserId).FirstOrDefault();
				return user;
			}
			else
			{
				return null;
			}
		}
		public UserInfo GetUserByEmail(string email)
		{
			var result = dataContext.User
					   .Where(b => b.Email.Equals(email)).FirstOrDefault();
			return result;
		}

		public UserInfo GetUserByUserId(long? userid)
		{
			if (userid == null)
			{
				return null;
			}
			var result = dataContext.User

						  .Where(b => b.Id == (userid)).FirstOrDefault();
			return result;
		}

		/// <summary>
		/// 自增Usn
		/// 每次notebook,note添加, 修改, 删除, 都要修改
		/// </summary>
		/// <param name="userid">用户id</param>
		/// <returns>自增后的usn</returns>
		public int IncrUsn(long? userid)
		{
			var user = dataContext.User
			   .Where(b => b.Id == (userid
			   )).FirstOrDefault();
			user.Usn += 1;
			dataContext.SaveChanges();
			return user.Usn;
		}

		public int GetUsn(long? userId)
		{
			throw new Exception();
		}

		public bool AddUserAsync(UserInfo user)
		{
			if (user.Id == 0) user.Id = idGenerator.NextId();
			user.CreatedTime = DateTime.Now;
			user.Email = user.Email.ToLower();
			EmailService.RegisterSendActiveEmail(user, user.Email);

			dataContext.User.Add(user);
			return dataContext.SaveChanges() > 0;
		}

		public bool VDEmail(string email, out string message)
		{
			//验证邮箱冲突
			var count = dataContext.User.Where(b => b.Email.Equals(email)).Count();
			if (count != 0)
			{
				message = "Email Address Conflict";
				return false;
			}
			message = "succeed";
			return true;
		}

		public bool VDUserName(string username, out string message)
		{
			bool result = false;
			message = string.Empty;
			//验证是否包含特殊符号

			if (!MyStringUtil.isEmail(username))
			{
				//如果不是邮箱就不允许包含特殊符号
				result = MyStringUtil.IsNumAndEnCh(username);

				if (!result)
				{
					message = "Contains special symbols";
					return false;
				}
			}

			//验证长度
			if (username.Length < 6)
			{
				message = "Length less than 6";
				return false;
			}
			//验证名称冲突
			var count = dataContext.User.Where(b => b.Username.Equals(username.ToLower())).Count();
			if (count != 0)
			{
				message = "User name Conflict";
				return false;
			}
			//验证黑名单
			HashSet<String> blacklist = new HashSet<string>();
			blacklist.Add("admin");
			blacklist.Add("demo");
			blacklist.Add("root");

			result = blacklist.Contains(username.ToLower());

			return !result;
		}

		public bool VDPassWord(string password, long? userId, out string error)
		{
			error = string.Empty;
			if (string.IsNullOrEmpty(password))
			{
				error = "Invalid password ";
				return false;
			}
			if (password.Length < 6)
			{
				error = "Password length is too short , must be greater than or equal to 6 ";
				return false;
			}
			if (password.Length > 32)
			{
				error = "Password is too long  , must be less than 32 characters ";
				return false;
			}
			//验证合法性
			return true;
		}

		public void AddFIDO2Repository(long? userId, FIDO2Item fido)
		{
			var user = dataContext.User.Include(p => p.FIDO2Items).Where(b => b.Id == userId).FirstOrDefault();
			//user.FIDO2Items.Add(fido);
			dataContext.Fido2Items.Add(fido);
			dataContext.SaveChanges();
		}

		public void SetUserLoginSecurityPolicyLevel(long? userId, LoginSecurityPolicyLevel level)
		{


			var user = dataContext.User.Where(b => b.Id == userId).First();
			user.LoginSecurityPolicyLevel = level;

			dataContext.User.Where(b => b.Id == userId).UpdateFromQuery(x => new UserInfo()
			{
				LoginSecurityPolicyLevel = level,
				Hmac = user.Hmac
			});
			dataContext.SaveChanges();
		}

		public bool SetMDEditorPreferences(long? userId, string mdOption)
		{
			var user = dataContext.User.Where(b => b.Id == userId).FirstOrDefault();
			if (user == null)
			{
				return false;
			}
			user.PreferredMarkdownEditor = (MarkdownEditorOption)Enum.Parse(typeof(MarkdownEditorOption), mdOption);

			dataContext.SaveChanges();
			return true;
		}
		public bool SetRTEditorPreferences(long? userId, string rtOption)
		{
			var user = dataContext.User.Where(b => b.Id == userId).FirstOrDefault();
			if (user == null)
			{
				return false;
			}

			user.PreferredRichTextEditor = (RichTextEditorOption)Enum.Parse(typeof(RichTextEditorOption), rtOption);
			dataContext.SaveChanges();
			return true;
		}

		public bool AddBlogUser(UserBlog user)
		{
			if (user.UserId == 0) user.UserId = idGenerator.NextId();
			dataContext.UserBlog.Add(user);
			return dataContext.SaveChanges() > 0;
		}

		// 通过email得到userId
		public string GetUserId(string email)
		{
			throw new Exception();
		}

		// 得到用户名
		public string GetUsername(long? userId)
		{
			throw new Exception();
		}

		// 得到用户名
		public string GetUsernameById(long? userId)
		{
			throw new Exception();
		}

		// 是否存在该用户 email
		public bool IsExistsUser(string email)
		{
			if (dataContext.User.Where(m => m.Email.Equals(email.ToLower())).Count() > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		// 是否存在该用户 username
		public bool IsExistsUserByUsername(string userName)
		{
			throw new Exception();
		}

		// 得到用户信息, userId, username, email
		public UserInfo GetUserInfoByAny(string idEmailUsername)
		{
			throw new Exception();
		}

		public void setUserLogo(UserInfo
			 user)
		{
			throw new Exception();
		}

		// 仅得到用户
		public UserInfo GetUser(long? userId)
		{
			throw new Exception();
		}

		// 得到用户信息 userId
		public UserInfo GetUserInfo(long? userId)
		{
			if (userId == null)
			{
				return null;
			}
			var result = dataContext.User
					  .Where(b => b.Id == userId).FirstOrDefault();
			return result;
		}

		// 得到用户信息 email
		public UserInfo GetUserInfoByEmail(string email)
		{
			throw new Exception();
		}

		// 得到用户信息 username
		public UserInfo GetUserInfoByUsername(string username)
		{
			throw new Exception();
		}

		public UserInfo GetUserInfoByThirdUserId(string thirdUserId)
		{
			throw new Exception();
		}

		public UserInfo[] ListUserInfosByUserIds(long[] userIds)
		{
			throw new Exception();
		}

		public UserInfo ListUserInfosByEmails(string[] email)
		{
			throw new Exception();
		}

		// 用户信息即可
		public Dictionary<long, UserInfo> MapUserInfoByUserIds(long[] userIds)
		{
			throw new Exception();
		}

		// 用户信息和博客设置信息
		public Dictionary<long, UserInfo> MapUserInfoAndBlogInfosByUserIds(long[] userIds)
		{
			throw new Exception();
		}

		// 返回info.UserAndBlog
		public Dictionary<long?, UserAndBlog> MapUserAndBlogByUserIds(long?[] userIds)
		{
			//todo: MapUserAndBlogByUserIds
			return null;
		}

		// 得到用户信息+博客主页
		public UserInfo GetUserAndBlogUrl(long? userId, Notebook repository)
		{
			UserInfo user = GetUserInfo(userId);

			UserBlog userBlog = BlogService.GetUserBlog(userId);
			BlogUrls blogUrls = BlogService.GetBlogUrls(userBlog, user, repository);
			//UserAndBlogUrl userAndBlogUrl = new UserAndBlogUrl()
			//{
			//    User = user,
			//    BlogUrl = blogUrls.IndexUrl,
			//    PostUrl = blogUrls.PostUrl,
			//};
			user.BlogUrl = blogUrls.IndexUrl;
			user.PostUrl = blogUrls.PostUrl;

			return user;
		}

		/// <summary>
		/// 得到userAndBlog公开信息
		/// </summary>
		/// <param name="userId">用户ID</param>
		/// <returns></returns>
		public UserAndBlog GetUserAndBlog(long? userId, Notebook repository)
		{
			var user = this.GetUserInfo(userId);
			var userBlog = BlogService.GetUserBlog(userId);

			var userAndBlog = new UserAndBlog()
			{
				UserId = user.Id,
				Username = user.Username,
				Email = user.Email,
				Logo = user.Logo,
				BlogTitle = userBlog.Title,
				BlogLogo = userBlog.Logo,
				BlogUrl = BlogService.GetUserBlogUrl(userBlog, user.Username),
				BlogUrls = BlogService.GetBlogUrls(userBlog, user, repository)
			};
			return userAndBlog;
		}

		// 通过ids得到users, 按id的顺序组织users
		public UserInfo[] GetUserInfosOrderBySeq(long[] userIds)
		{
			throw new Exception();
		}

		// 使用email(username), 得到用户信息
		public UserInfo GetUserInfoByName(string emailOrUsername)
		{
			throw new Exception();
		}

		public UserInfo GetUserByUserName(string Username)
		{
			var result = dataContext.User.Where(b => b.Username.Equals(Username.ToLower()));
			return result == null ? null : result.FirstOrDefault();
		}

		// 更新username
		public bool UpdateUsername(long? userId, string username)
		{
			var user = dataContext.User.Where(b => b.Id == userId).FirstOrDefault();
			if (user == null)
			{
				return false;
			}
			user.UsernameRaw = username;
			user.Username = username;
			return dataContext.SaveChanges() > 0;
		}

		// 修改头像
		public bool UpdateAvatar(long? userId, string avatarPath)
		{
			var user = dataContext.User.Where(b => b.Id == userId).FirstOrDefault();
			user.Logo = avatarPath;
			dataContext.SaveChanges();
			return true;
		}

		//----------------------
		// 已经登录了的用户修改密码
		public async Task<bool> UpdatePwd(long? userId, string oldPwd, string pwd)
		{
			var user = dataContext.User.Where(b => b.Id == userId).First();

			IPasswordStore passwordStore = passwordStoreFactory.Instance(user);
			//验证旧密码
			var vd = passwordStore.VerifyPassword(user.Pwd.Base64ToByteArray(), oldPwd.Base64ToByteArray(), user.Salt.Base64ToByteArray(), user.PasswordHashIterations);
			if (!vd)
			{
				return vd;
			}
			//产生新的盐
			var salt = RandomTool.CreatSafeSaltByteArray(16);

			passwordStore = passwordStoreFactory.Instance(Config.SecurityConfig);
			//更新用户生成密码哈希的安全策略
			user.PasswordDegreeOfParallelism = Config.SecurityConfig.PasswordStoreDegreeOfParallelism;
			user.PasswordHashAlgorithm = Config.SecurityConfig.PasswordHashAlgorithm;
			user.PasswordHashIterations = Config.SecurityConfig.PasswordHashIterations;
			user.PasswordMemorySize = Config.SecurityConfig.PasswordStoreMemorySize;
			//更新盐
			user.Salt = salt.ByteArrayToBase64();
			//生成新的密码哈希
			user.Pwd = (passwordStore.Encryption(pwd.Base64ToByteArray(), salt, user.PasswordHashIterations)).ByteArrayToBase64();

			return dataContext.SaveChanges() > 0;
		}

		// 管理员重置密码
		public bool ResetPwd(long? adminUserId, long? userId, string pwd)
		{
			throw new Exception();
		}

		// 修改主题
		public bool UpdateTheme(long? userId, string theme)
		{
			throw new Exception();
		}

		// 帐户类型设置
		public bool UpdateAccount(long? userId, string accountType, DateTime accountStartTime, DateTime accountEndTime, int maxImageNum, int maxImageSize, int maxAttachNum, int maxAttachSize, int maxPerAttachSize)
		{
			throw new Exception();
		}

		//---------------
		// 修改email

		// 注册后验证邮箱
		public bool ActiveEmail(string token, out string email)
		{
			throw new Exception();
		}

		// 修改邮箱
		// 在此之前, 验证token是否过期
		// 验证email是否有人注册了
		public bool UpdateEmail(string token, out string email)
		{
			throw new Exception();
		}

		//------------
		// 偏好设置

		// 宽度
		public bool UpdateColumnWidth(long? userId, int notebookWidth, int noteListWidth, int mdEditorWidth)
		{
			dataContext.User.Where(x => x.Id == userId).UpdateFromQuery(p => new UserInfo()
			{
				NotebookWidth = notebookWidth,
				NoteListWidth = noteListWidth,
				MdEditorWidth = mdEditorWidth
			});
			dataContext.SaveChanges();
			return true;
		}

		// 左侧是否隐藏
		public bool UpdateLeftIsMin(long? userId, bool leftIsMin)
		{
			dataContext.User.Where(x => x.Id == userId)
				.UpdateFromQuery(p => new UserInfo()
				{
					LeftIsMin = leftIsMin
				});
			dataContext.SaveChanges();
			return true;
		}

		//-------------
		// user admin
		public UserInfo[] ListUsers(int pageNumber, int pageSize, string sortField, bool isAsc, string email, out Page page)
		{
			var users = dataContext.User.Where(b => b.Username.Equals(email) || b.UsernameRaw.Equals(email) || b.Email.Equals(email)).OrderBy(b => b.Id).Skip((pageNumber - 1) * 10).Take(pageSize).ToArray();

			var count = users.Count();

			page = Page.Instance(pageNumber, pageSize, count, null);

			return users;
		}

		public UserInfo[] ListUsers(int pageNumber, int pageSize, string sortField, bool isAsc, out Page page)
		{
			var users = dataContext.User.OrderBy(b => b.Id).OrderBy(b => b.Id).Skip((pageNumber - 1) * 10).Take(pageSize).ToArray();

			var count = users.Count();

			page = Page.Instance(pageNumber, pageSize, count, null);

			return users;
		}

		public UserInfo[] GetAllUserByFilter(string userFilterEmail, string userFilterWhiteList, string userFilterBlackList, bool verified)
		{
			throw new Exception();
		}

		// 统计
		public int CountUser()
		{
			return dataContext.User.Count();
		}

		public void InitFIDO2Repositories(long? userId)
		{

		}
	}
}