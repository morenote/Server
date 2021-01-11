using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MoreNote.Logic.Service
{
    public class TokenSerivce
    {
        private DependencyInjectionService dependencyInjectionService;

        public TokenSerivce(DependencyInjectionService dependencyInjectionService)
        {
            this.dependencyInjectionService = dependencyInjectionService;
        }

        public bool AddToken(Token token)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                int a = 0;

                var result = dataContext.Token.Add(token);
                a = dataContext.SaveChanges();
                return dataContext.SaveChanges() > 0;
            }
        }

        [Obsolete]
        private static string GenerateToken()
        {
            StringBuilder tokenBuilder = new StringBuilder();

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
        public string GenerateToken(long tokenId, int tokenByteSize = 16)
        {
            if (tokenByteSize < 1)
            {
                tokenByteSize = 1;
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

        public Token GetTokenByTokenStr(long userid, string str)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.Token
                           .Where(b => b.UserId.Equals(userid) && b.TokenStr.Equals(str)).FirstOrDefault();
                return result;
            }
        }

        public User GetUserByToken(string token)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.Token
                      .Where(b => b.TokenStr.Equals(token)).FirstOrDefault();
                if (result != null)
                {
                    var user = dataContext.User
                    .Where(b => b.UserId == result.UserId).FirstOrDefault();
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool DeleteTokenByToken(string token)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                dataContext.Token.Where(a => a.TokenStr.Equals(token));
                return dataContext.SaveChanges() > 0;
            }
        }

        // 生成token
        public string NewToken(long userId, string email, int tokenType)
        {
            throw new Exception();
        }

        // 删除token
        public bool DeleteToken(long userId, int tokenType)
        {
            throw new Exception();
        }

        // 验证token, 是否存在, 过时?
        public bool VerifyToken(string token, int tokenType)
        {
            throw new Exception();
        }
    }
}