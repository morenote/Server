using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public class TokenSerivce
    {
        public static bool AddToken(Token token)
        {
            int a = 0;
            using (var db = new DataContext())
            {
                var result = db.Token.Add(token);
                a = db.SaveChanges();
                return db.SaveChanges() > 0;
            }

        }
        public static Token GetTokenByTokenStr(long userid,string str)
        {

            using (var db = new DataContext())
            {
                var result = db.Token
                    .Where(b => b.UserId.Equals(userid)&&b.TokenStr.Equals(str)).FirstOrDefault();
                return result;
            }
        }
        public static User GetUserByToken(string token)
        {

            using (var db = new DataContext())
            {
                var result = db.Token
                    .Where(b => b.TokenStr.Equals(token)).FirstOrDefault();
                if (result!=null)
                {
                    var user = db.User
                    .Where(b => b.UserId==result.UserId).FirstOrDefault();
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
