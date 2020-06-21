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
        public static User GetUser(string email)
        {
            using (var db = DataContext.getDataContext())
            {
                var result = db.User
                    .Where(b => b.Email.Equals(email)).FirstOrDefault();
                return result;
            }
        }
        public static User GetUserByUserId(long userid)
        {

            using (var db = DataContext.getDataContext())
            {
                var result = db.User
                    .Where(b => b.UserId.Equals(userid)).FirstOrDefault();
                return result;
            }
        }

        /// <summary>
        /// 自增Usn
        /// 每次notebook,note添加, 修改, 删除, 都要修改
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns>自增后的usn</returns>
        public static int IncrUsn(long userid)
        {
            using (var db = DataContext.getDataContext())
            {
                var user = db.User
                    .Where(b => b.UserId == (userid
                    )).FirstOrDefault();
                user.Usn += 1;
                db.SaveChanges();
                return user.Usn;
            }
        }
        public static int GetUsn(long userId)
        {
            throw new Exception();
        }
        public static bool AddUser(User
             user)
        {
            if (user.UserId == 0) user.UserId = SnowFlake_Net.GenerateSnowFlakeID();
            user.CreatedTime = DateTime.Now;
            user.Email = user.Email.ToLower();
            EmailService.RegisterSendActiveEmail(user, user.Email);
            using (var db = DataContext.getDataContext())
            {
                db.User.Add(user);
                return db.SaveChanges()>0;
            }
        }
        // 通过email得到userId
        public static string GetUserId(string email)
        {
            throw new Exception();
        }
        // 得到用户名
        public string GetUsername(long userId)
        {
            throw new Exception();
        }
        // 得到用户名
        public static string GetUsernameById(long userId)
        {
            throw new Exception();
        }
        // 是否存在该用户 email
        public static bool IsExistsUser(string email)
        {
            using (var db = DataContext.getDataContext())
            {
                if (db.User.Where(m => m.Email.Equals(email.ToLower())).Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        // 是否存在该用户 username
        public static bool IsExistsUserByUsername(string userName)
        {
            throw new Exception();
        }
        // 得到用户信息, userId, username, email
        public static User GetUserInfoByAny(string idEmailUsername)
        {
            throw new Exception();

        }
        public static void setUserLogo(User
             user)
        {
            throw new Exception();
        }
        // 仅得到用户
        public static User GetUser(long userId)
        {
            throw new Exception();
        }
        // 得到用户信息 userId
        public static User GetUserInfo(long userId)
        {
            using (var db = DataContext.getDataContext())
            {
                var result = db.User
                    .Where(b => b.UserId== userId).FirstOrDefault();
                return result;
            }
        }
        // 得到用户信息 email
        public static User GetUserInfoByEmail(string email)
        {
            throw new Exception();
        }
        // 得到用户信息 username
        public static User GetUserInfoByUsername(string username)
        {
            throw new Exception();
        }
        public static User GetUserInfoByThirdUserId(string thirdUserId)
        {
            throw new Exception();
        }
        public static User[] ListUserInfosByUserIds(long[] userIds)
        {
            throw new Exception();
        }
        public static User ListUserInfosByEmails(string[] email)
        {
            throw new Exception();
        }

        // 用户信息即可
        public static Dictionary<long, User> MapUserInfoByUserIds(long[] userIds)
        {
            throw new Exception();
        }
        // 用户信息和博客设置信息
        public static Dictionary<long, User> MapUserInfoAndBlogInfosByUserIds(long[] userIds)
        {
            throw new Exception();
        }
        // 返回info.UserAndBlog
        public static Dictionary<string, UserAndBlog> MapUserAndBlogByUserIds(long[] userIds)
        {
            throw new Exception();
        }
        // 得到用户信息+博客主页
        public static UserAndBlogUrl GetUserAndBlogUrl(long userId)
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

        public static UserAndBlog GetUserAndBlog(long userId)
        {
            throw new Exception();
        }
        // 通过ids得到users, 按id的顺序组织users
        public static User[] GetUserInfosOrderBySeq(long[] userIds)
        {
            throw new Exception();
        }
        // 使用email(username), 得到用户信息
        public static User GetUserInfoByName(string emailOrUsername)
        {
            throw new Exception();
        }
        public static User GetUserByUserName(string Username)
        {
            using (var db=new DataContext())
            {
                var result = db.User.Where(b => b.Username.Equals(Username.ToLower()));
                return result == null ? null : result.FirstOrDefault();

            }
        }
        // 更新username
        public static bool UpdateUsername(long userId, string username)
        {
            throw new Exception();
        }
        // 修改头像
        public static bool UpdateAvatar(long userId, string avatarPath)
        {
            throw new Exception();
        }

        //----------------------
        // 已经登录了的用户修改密码
        public static bool UpdatePwd(long userId, string oldPwd, string pwd)
        {
            throw new Exception();
        }
        // 管理员重置密码
        public static bool ResetPwd(long adminUserId, long userId, string pwd)
        {
            throw new Exception();
        }
        // 修改主题
        public static bool UpdateTheme(long userId, string theme)
        {
            throw new Exception();
        }
        // 帐户类型设置
        public static bool UpdateAccount(long userId, string accountType, DateTime accountStartTime, DateTime accountEndTime, int maxImageNum, int maxImageSize, int maxAttachNum, int maxAttachSize, int maxPerAttachSize)
        {
            throw new Exception();
        }
        //---------------
        // 修改email

        // 注册后验证邮箱
        public static bool ActiveEmail(string token, out string email)
        {
            throw new Exception();
        }
        // 修改邮箱
        // 在此之前, 验证token是否过期
        // 验证email是否有人注册了
        public static bool UpdateEmail(string token, out string email)
        {
            throw new Exception();
        }
        //------------
        // 偏好设置

        // 宽度
        public static bool UpdateColumnWidth(long userId, int notebookWidth, int noteListWidth, int mdEditorWidth)
        {
            throw new Exception();
        }
        // 左侧是否隐藏
        public static bool UpdateLeftIsMin(long userId, bool leftIsMin)
        {
            throw new Exception();
        }
        //-------------
        // user admin
        public static void ListUsers(int pageBunber, int pageSize, string sortField, bool isAsc, string email, out Page page, out User[] users)
        {
            throw new Exception();
        }

        public static User[] GetAllUserByFilter(string userFilterEmail, string userFilterWhiteList, string userFilterBlackList, bool verified)
        {
            throw new Exception();
        }
        // 统计
        public static int CountUser()
        {
            throw new Exception();
        }



    }
}
