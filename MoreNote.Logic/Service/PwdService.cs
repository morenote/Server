using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreNote.Common.entity;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public class PwdService
    {
        /// <summary>
        /// 通过email找回密码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool FindPwd(string email)
        {

            using (var db = new DataContext())
            {
                var userid = db.User
                    .Where(b => b.Email.Equals(email)).First();
                if (userid == null)
                {

                    return false;
                }


                return true;


            }

        }
    }
}
