using System;

using MoreNote.Common.Util;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public class AuthService
    {
        public static bool LoginByPWD(String email, string pwd, out string tokenStr,out User user)
        {
            user = UserService.GetUser(email);
            if (user != null)
            {
                string temp = Encrypt_Helper.Hash256Encrypt(pwd + user.Salt);
                if (temp.Equals(user.Pwd))
                {
                    Token myToken = new Token
                    {
                        TokenId = SnowFlake_Net.GenerateSnowFlakeID(),
                        UserId = user.UserId,
                        Email = user.Email,
                        TokenStr = RndNum.CreatRndNum(32),
                        Type = 0,
                        CreatedTime = DateTime.Now
                    };
                    TokenSerivce.AddToken(myToken);
                    tokenStr = myToken.TokenStr;
                    return true;
                }
                else
                {
                    tokenStr = "";
                    return false;
                }

            }
            else
            {
                tokenStr = "";
                return false;
            }

        }
        public static bool LoginByTokem(string email, string token)
        {
            return true;
        }
        /// <summary>
        /// 通过Token判断用户是否登录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        public static bool IsLogin(long userid,string tokenStr)
        {
            Token token = TokenSerivce.GetTokenByTokenStr(userid
                , tokenStr);
            if (token!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
