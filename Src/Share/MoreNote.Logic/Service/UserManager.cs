using MySql.Data.MySqlClient;
using NickelProject.Logic.DB;
using NickelProject.Logic.Entity;
using System.Collections.Generic;
using System.Linq;

namespace NickelProject.Logic.Model
{
    public class UserManager
    {
        /// <summary>
        /// 向数据库中插入用户
        /// </summary>
        /// <param name="userEntity">用户</param>
        /// <returns>返回是否成功 1成功,其他均为失败</returns>
        public static int AddUser(UserEntity userEntity)
        {
            int a = 0;
            using (var db = new DataContext())
            {
                db.User.Add(userEntity);
                a=db.SaveChanges();
            }
            return a;

        }
        //删除用户
        //查询用户
        /// <summary>
        /// 通过用户ID查询用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static List<UserEntity> SelectUserByUserId(string userId)
        {
            using (var db = new DataContext())
            {
                List<UserEntity> userEntities = db.User
                    .Where(b => b.Userid.Equals(userId))
                    .ToList<UserEntity>();
                return userEntities;
                
               
            }
        }
        public static List<UserEntity> SelectUserByUserEmail(string email)
        {
            using (var db = new DataContext())
            {
                List<UserEntity> userEntities = db.User
                    .Where(b => b.Email.Equals(email))
                    .ToList<UserEntity>();
                return userEntities;


            }
        }
        public static List<UserEntity> SelectUserByUserName(string name)
        {
            using (var db = new DataContext())
            {
                List<UserEntity> userEntities = db.User
                    .Where(b => b.UserName.Equals(name))
                    .ToList<UserEntity>();
                return userEntities;


            }
        }
        /// <summary>
        /// 选取若干用户
        /// </summary>
        /// <param name="minLimit">从第几个位置</param>
        /// <param name="maxLimit">一共选取多少</param>
        /// <returns></returns>
        public static List<UserEntity> SelectAllUser(int minLimit, int maxLimit)
        {

            using (var db = new DataContext())
            {
                List<UserEntity> userEntities = db.User.Skip(minLimit).Take(maxLimit)
                    .ToList<UserEntity>();
                    return userEntities;
             
            }
        }
        /// <summary>
        /// 激活用户
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="token">用户授权密钥</param>
        /// <returns></returns>
        public static int ActivateUser(string userid, string token)
        {
            int a = 0;
            using (var db = new DataContext())
            {
                UserEntity userEntity = db.User
                    .Where(b => b.Userid.Equals(userid)).FirstOrDefault();
                if (userid != null)
                {
                    userEntity.activity = true;
                }
                a=db.SaveChanges();
            }
            return a;

        }
        public enum SqlGroup
        {
            root, admin
        }
        public static int editGroup(string userid, SqlGroup userGroup)
        {
            int a = 0;
            using (var db = new DataContext())
            {
                UserEntity userEntity = db.User
                    .Where(b => b.Userid.Equals(userid)).FirstOrDefault();
                if (userid != null)
                {
                    userEntity.UserGroup = userGroup.ToString();
                   
                }
                a = db.SaveChanges();
            }
            return a;
            

        }

        public static int ChangeUserDataByUserId(string userid, string newName, string newEmail, string newPassword, string newPhone, string newQQ, string newTwitter, string newIntro)
        {
            int a = 0;
            using (var db = new DataContext())
            {
                UserEntity userEntity = db.User
                    .Where(b => b.Userid.Equals(userid)).FirstOrDefault();
                if (userid != null)
                {
                    userEntity.UserName = newName;
                    userEntity.Email = newEmail;
                    userEntity.PassWord = newPassword;
                    userEntity.Telephone = newPhone;
                    userEntity.QQ = newQQ;
                    userEntity.Twitter = newTwitter;
                    userEntity.Intro = newIntro;
                }
                a = db.SaveChanges();
            }
            return a;
        }

        public static int ChangeAvatar(string newAvatar, string userid)
        {
            int a = 0;
            using (var db = new DataContext())
            {
                UserEntity userEntity = db.User
                    .Where(b => b.Userid.Equals(userid)).FirstOrDefault();
                if (userid != null)
                {
                    userEntity.Avatar = newAvatar;
                 
                }
                a = db.SaveChanges();
            }
            return a;
          

        }
        public static int ChangeGb(string userid, int gb, bool add)
        {

            int a = 0;
            using (var db = new DataContext())
            {
                UserEntity userEntity = db.User
                    .Where(b => b.Userid.Equals(userid)).FirstOrDefault();
                if (userid != null)
                {
                    if (add)
                    {
                        userEntity.Gb += gb;

                    }
                    else
                    {
                        userEntity.Gb -= gb;

                    }
                   

                }
                a = db.SaveChanges();
            }
            return a;
          

        }

        public static int ChangeTokenByEmail(string email, string token)
        {
            int a = 0;
            using (var db = new DataContext())
            {
                UserEntity userEntity = db.User
                    .Where(b => b.Email.Equals(email)).FirstOrDefault();
                if (userEntity != null)
                {
                    userEntity.Token = token;
                }
                a = db.SaveChanges();
            }
            return a;
        }

    }
}
