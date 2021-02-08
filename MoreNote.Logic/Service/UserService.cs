using MoreNote.Common.Utils;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreNote.Logic.Service
{
    public class UserService
    {
        private DataContext dataContext;
        public BlogService BlogService { get;set;}
        public EmailService EmailService { get;set;}
        public UserService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public User GetUser(string email)
        {
          
                var result = dataContext.User
                           .Where(b => b.Email.Equals(email)).FirstOrDefault();
                return result;
            
        }

        public User GetUserByUserId(long userid)
        {
           
                var result = dataContext.User
                         .Where(b => b.UserId.Equals(userid)).FirstOrDefault();
                return result;
            
        }

        /// <summary>
        /// 自增Usn
        /// 每次notebook,note添加, 修改, 删除, 都要修改
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns>自增后的usn</returns>
        public int IncrUsn(long userid)
        {
           
                var user = dataContext.User
                   .Where(b => b.UserId == (userid
                   )).FirstOrDefault();
                user.Usn += 1;
                dataContext.SaveChanges();
                return user.Usn;
            
        }

        public int GetUsn(long userId)
        {
            throw new Exception();
        }

        public bool AddUser(User
             user)
        {
          
            if (user.UserId == 0) user.UserId = SnowFlakeNet.GenerateSnowFlakeID();
            user.CreatedTime = DateTime.Now;
            user.Email = user.Email.ToLower();
            EmailService.RegisterSendActiveEmail(user, user.Email);
          
                dataContext.User.Add(user);
                return dataContext.SaveChanges() > 0;
            
        }

        // 通过email得到userId
        public string GetUserId(string email)
        {
            throw new Exception();
        }

        // 得到用户名
        public string GetUsername(long userId)
        {
            throw new Exception();
        }

        // 得到用户名
        public string GetUsernameById(long userId)
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
        public User GetUser(long userId)
        {
            throw new Exception();
        }

        // 得到用户信息 userId
        public User GetUserInfo(long userId)
        {
            
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
        public Dictionary<string, UserAndBlog> MapUserAndBlogByUserIds(long[] userIds)
        {
            throw new Exception();
        }

        // 得到用户信息+博客主页
        public UserAndBlogUrl GetUserAndBlogUrl(long userId)
        {
            User user = GetUserInfo(userId);
     
            UserBlog userBlog = BlogService.GetUserBlog(userId);
            BlogUrls blogUrls = BlogService.GetBlogUrls(userBlog, user);
            UserAndBlogUrl userAndBlogUrl = new UserAndBlogUrl()
            {
                user = user,
                BlogUrl = blogUrls.IndexUrl,
                PostUrl = blogUrls.PostUrl,
            };
            return userAndBlogUrl;
        }

        // 得到userAndBlog公开信息

        public UserAndBlog GetUserAndBlog(long userId)
        {
            throw new Exception();
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
        public bool UpdateUsername(long userId, string username)
        {
            throw new Exception();
        }

        // 修改头像
        public bool UpdateAvatar(long userId, string avatarPath)
        {
            throw new Exception();
        }

        //----------------------
        // 已经登录了的用户修改密码
        public bool UpdatePwd(long userId, string oldPwd, string pwd)
        {
            throw new Exception();
        }

        // 管理员重置密码
        public bool ResetPwd(long adminUserId, long userId, string pwd)
        {
            throw new Exception();
        }

        // 修改主题
        public bool UpdateTheme(long userId, string theme)
        {
            throw new Exception();
        }

        // 帐户类型设置
        public bool UpdateAccount(long userId, string accountType, DateTime accountStartTime, DateTime accountEndTime, int maxImageNum, int maxImageSize, int maxAttachNum, int maxAttachSize, int maxPerAttachSize)
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
        public bool UpdateColumnWidth(long userId, int notebookWidth, int noteListWidth, int mdEditorWidth)
        {
            throw new Exception();
        }

        // 左侧是否隐藏
        public bool UpdateLeftIsMin(long userId, bool leftIsMin)
        {
            throw new Exception();
        }

        //-------------
        // user admin
        public void ListUsers(int pageBunber, int pageSize, string sortField, bool isAsc, string email, out Page page, out User[] users)
        {
            throw new Exception();
        }

        public User[] GetAllUserByFilter(string userFilterEmail, string userFilterWhiteList, string userFilterBlackList, bool verified)
        {
            throw new Exception();
        }

        // 统计
        public int CountUser()
        {
            throw new Exception();
        }
    }
}