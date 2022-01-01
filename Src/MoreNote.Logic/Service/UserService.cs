using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.HySystem;
using MoreNote.Common.Utils;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service.PasswordSecurity;
using MoreNote.Models.Entity.Leanote;

namespace MoreNote.Logic.Service
{
    public class UserService
    {
        private DataContext dataContext;
        public BlogService BlogService { get; set; }
        public EmailService EmailService { get; set; }
        public WebSiteConfig Config { get; set; }

        public UserService(DataContext dataContext, ConfigFileService configFileService)
        {
            this.dataContext = dataContext;
            this.Config = configFileService.WebConfig;
        }

        public User GetUser(string email)
        {
            var result = dataContext.User
                       .Where(b => b.Email.Equals(email)).FirstOrDefault();
            return result;
        }

        public User GetUserByUserId(long? userid)
        {
            if (userid == null)
            {
                return null;
            }
            var result = dataContext.User
                          .Include(user => user.FIDO2Items)
                          .Where(b => b.UserId==(userid)).FirstOrDefault();
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
               .Where(b => b.UserId == (userid
               )).FirstOrDefault();
            user.Usn += 1;
            dataContext.SaveChanges();
            return user.Usn;
        }

        public int GetUsn(long? userId)
        {
            throw new Exception();
        }

        public bool AddUser(User user)
        {
            if (user.UserId == 0) user.UserId = SnowFlakeNet.GenerateSnowFlakeID();
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

            if (!HyString.isEmail(username))
            {
                //如果不是邮箱就不允许包含特殊符号
                result = HyString.IsNumAndEnCh(username);

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
            var user = dataContext.User.Include(p => p.FIDO2Items).Where(b => b.UserId == userId).FirstOrDefault();
            //user.FIDO2Items.Add(fido);
            dataContext.FIDO2Repository.Add(fido);
            dataContext.SaveChanges();
        }

        public bool SetEditorPreferences(long? userId, string mdOption, string rtOption)
        {
            var user = dataContext.User.Where(b => b.UserId == userId).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            user.MarkdownEditorOption = mdOption;
            user.RichTextEditorOption = rtOption;
            dataContext.SaveChanges();
            return true;
        }

        public bool AddBlogUser(UserBlog user)
        {
            if (user.UserId == 0) user.UserId = SnowFlakeNet.GenerateSnowFlakeID();
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
        public User GetUserInfoByAny(string idEmailUsername)
        {
            throw new Exception();
        }

        public void setUserLogo(User
             user)
        {
            throw new Exception();
        }

        // 仅得到用户
        public User GetUser(long? userId)
        {
            throw new Exception();
        }

        // 得到用户信息 userId
        public User GetUserInfo(long? userId)
        {
            if (userId == null)
            {
                return null;
            }
            var result = dataContext.User
                      .Where(b => b.UserId == userId).FirstOrDefault();
            return result;
        }

        // 得到用户信息 email
        public User GetUserInfoByEmail(string email)
        {
            throw new Exception();
        }

        // 得到用户信息 username
        public User GetUserInfoByUsername(string username)
        {
            throw new Exception();
        }

        public User GetUserInfoByThirdUserId(string thirdUserId)
        {
            throw new Exception();
        }

        public User[] ListUserInfosByUserIds(long[] userIds)
        {
            throw new Exception();
        }

        public User ListUserInfosByEmails(string[] email)
        {
            throw new Exception();
        }

        // 用户信息即可
        public Dictionary<long, User> MapUserInfoByUserIds(long[] userIds)
        {
            throw new Exception();
        }

        // 用户信息和博客设置信息
        public Dictionary<long, User> MapUserInfoAndBlogInfosByUserIds(long[] userIds)
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
        public User GetUserAndBlogUrl(long? userId)
        {
            User user = GetUserInfo(userId);

            UserBlog userBlog = BlogService.GetUserBlog(userId);
            BlogUrls blogUrls = BlogService.GetBlogUrls(userBlog, user);
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
        public UserAndBlog GetUserAndBlog(long? userId)
        {
            var user = this.GetUserInfo(userId);
            var userBlog = BlogService.GetUserBlog(userId);

            var userAndBlog = new UserAndBlog()
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Logo = user.Logo,
                BlogTitle = userBlog.Title,
                BlogLogo = userBlog.Logo,
                BlogUrl = BlogService.GetUserBlogUrl(userBlog, user.Username),
                BlogUrls = BlogService.GetBlogUrls(userBlog, user)
            };
            return userAndBlog;
        }

        // 通过ids得到users, 按id的顺序组织users
        public User[] GetUserInfosOrderBySeq(long[] userIds)
        {
            throw new Exception();
        }

        // 使用email(username), 得到用户信息
        public User GetUserInfoByName(string emailOrUsername)
        {
            throw new Exception();
        }

        public User GetUserByUserName(string Username)
        {
            var result = dataContext.User.Where(b => b.Username.Equals(Username.ToLower()));
            return result == null ? null : result.FirstOrDefault();
        }

        // 更新username
        public bool UpdateUsername(long? userId, string username)
        {
            var user = dataContext.User.Where(b => b.UserId == userId).FirstOrDefault();
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
            var user = dataContext.User.Where(b => b.UserId == userId).FirstOrDefault();
            user.Logo = avatarPath;
            dataContext.SaveChanges();
            return true;
        }

        //----------------------
        // 已经登录了的用户修改密码
        public bool UpdatePwd(long? userId, string oldPwd, string pwd)
        {
            var user = dataContext.User.Where(b => b.UserId == userId).First();

            IPasswordStore passwordStore = PasswordStoreFactory.Instance(user);
            //验证旧密码
            var vd = passwordStore.VerifyPassword(user.Pwd.Base64ToByteArray(), oldPwd.ToByteArrayByUtf8(), user.Salt.Base64ToByteArray(), user.PasswordHashIterations);
            if (!vd)
            {
                return vd;
            }
            //产生新的盐
            var salt = RandomTool.CreatSafeSaltByteArray(16);

            passwordStore = PasswordStoreFactory.Instance(Config.SecurityConfig);
            //更新用户生成密码哈希的安全策略
            user.PasswordDegreeOfParallelism = Config.SecurityConfig.PasswordStoreDegreeOfParallelism;
            user.PasswordHashAlgorithm = Config.SecurityConfig.PasswordHashAlgorithm;
            user.PasswordHashIterations = Config.SecurityConfig.PasswordHashIterations;
            user.PasswordMemorySize = Config.SecurityConfig.PasswordStoreMemorySize;
            //更新盐
            user.Salt = salt.ByteArrayToBase64();
            //生成新的密码哈希
            user.Pwd = passwordStore.Encryption(pwd.ToByteArrayByUtf8(), salt, user.PasswordHashIterations).ByteArrayToBase64();
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
            throw new Exception();
        }

        // 左侧是否隐藏
        public bool UpdateLeftIsMin(long? userId, bool leftIsMin)
        {
            throw new Exception();
        }

        //-------------
        // user admin
        public User[] ListUsers(int pageNumber, int pageSize, string sortField, bool isAsc, string email, out Page page)
        {
            var users = dataContext.User.Where(b => b.Username.Equals(email) || b.UsernameRaw.Equals(email) || b.Email.Equals(email)).OrderBy(b => b.UserId).Skip((pageNumber - 1) * 10).Take(pageSize).ToArray();

            var count = users.Count();

            page = Page.Instance(pageNumber, pageSize, count, null);

            return users;
        }

        public User[] ListUsers(int pageNumber, int pageSize, string sortField, bool isAsc, out Page page)
        {
            var users = dataContext.User.OrderBy(b => b.UserId).OrderBy(b => b.UserId).Skip((pageNumber - 1) * 10).Take(pageSize).ToArray();

            var count = users.Count();

            page = Page.Instance(pageNumber, pageSize, count, null);

            return users;
        }

        public User[] GetAllUserByFilter(string userFilterEmail, string userFilterWhiteList, string userFilterBlackList, bool verified)
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