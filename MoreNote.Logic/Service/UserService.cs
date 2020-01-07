using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public class UserService
    {
        public static User GetUser(string email)
        {
            using (var db = new DataContext())
            {
                var result = db.User
                    .Where(b => b.Email.Equals(email)).FirstOrDefault();
                return result;
            }
        }
        public static User GetUserByUserId(long userid)
        {

            using (var db = new DataContext())
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
            using (var db = new DataContext())
            {
                var user = db.User
                    .Where(b => b.UserId.Equals(userid
                    )).FirstOrDefault();
                user.Usn += 1;
                db.SaveChanges();
                return user.Usn;
            }
        }
    }
}
