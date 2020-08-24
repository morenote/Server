using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using MoreNote.Common.Util;
using MoreNote.Common.Utils;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public class TokenSerivce
    {
        public static bool AddToken(Token token)
        {
            int a = 0;
            using (var db = DataContext.getDataContext())
            {
                var result = db.Token.Add(token);
                a = db.SaveChanges();
                return db.SaveChanges() > 0;
            }
        }
        [Obsolete]

        private static string GenerateToken()
        {
            StringBuilder tokenBuilder=new StringBuilder();

            long tokenid = SnowFlakeNet.GenerateSnowFlakeID();
            tokenBuilder.Append(tokenid.ToHex24());
            tokenBuilder.Append("@");
            tokenBuilder.Append(RandomTool.CreatRandomString(16));
            tokenBuilder.Append("@");
            tokenBuilder.Append(DateTime.Now);
            var token = Base64Util.ToBase64String(tokenBuilder.ToString());
            return token;
        }
        /// <summary>
        /// 产生不可预测的Token
        /// </summary>
        /// <param name="tokenId">tokenId</param>
        /// <param name="tokenByteSize">不可预测部分的byte长度</param>
        /// <returns></returns>
        public static string GenerateToken(long tokenId,int tokenByteSize=16)
        {
            if (tokenByteSize<1)
            {
                tokenByteSize=1;
            }
            //byte数组A 8字节 long tokenId
            //byte数组B tokenByteSize字节 随机生成 默认长度16
            //AB拼接 输出hex字符串
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] numData = BitConverter.GetBytes(tokenId);
                byte[] randomData = new byte[tokenByteSize];
                rng.GetBytes(randomData);
                
                byte[] tokenData = new byte[numData.Length + randomData.Length];
                Array.Copy(numData, 0, tokenData, 0, numData.Length);
                Array.Copy(randomData, 0, tokenData, numData.Length, randomData.Length);
               // string token = Convert.ToBase64String(tokenData);
                string token = HexUtil.ByteArrayToString(tokenData);
                Console.WriteLine();
                return token;
            }
        }
        public static Token GetTokenByTokenStr(long userid,string str)
        {

            using (var db = DataContext.getDataContext())
            {
                var result = db.Token
                    .Where(b => b.UserId.Equals(userid)&&b.TokenStr.Equals(str)).FirstOrDefault();
                return result;
            }
        }
        public static User GetUserByToken(string token)
        {

            using (var db = DataContext.getDataContext())
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
        public static bool DeleteTokenByToken(string token)
        {
            using (var db = DataContext.getDataContext())
            {
                db.Token.Where(a => a.TokenStr.Equals(token));
                return db.SaveChanges() > 0;
            }
        }
        // 生成token
        public static  string NewToken(long userId,string email,int tokenType)
        {
            throw new Exception();
        }
        // 删除token
        public static bool DeleteToken(long userId,int tokenType)
        {
            throw new Exception();
        }
        // 验证token, 是否存在, 过时?
        public static bool VerifyToken(string token,int tokenType)
        {
            throw new Exception();
        }

    }
}
