using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MoreNote.Logic.Service
{
    public class TokenSerivce
    {
        private DataContext dataContext;
        private WebSiteConfig config;

        public TokenSerivce(DataContext dataContext, ConfigFileService configFileService)
        {
            this.dataContext = dataContext;
            this.config = configFileService.WebConfig;
        }

        public bool AddToken(Token token)
        {
            int a = 0;

            var result = dataContext.Token.Add(token);
            a = dataContext.SaveChanges();
            return dataContext.SaveChanges() > 0;
        }

        [Obsolete]
        private static string GenerateToken24()
        {
            StringBuilder tokenBuilder = new StringBuilder();

            long? tokenid = SnowFlakeNet.GenerateSnowFlakeID();
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
        /// <param name="tokenByteSize">不可预测部分的byte长度 字节</param>
        /// <returns></returns>
        public string GenerateTokenContext(long? tokenId, int tokenByteSize = 16)
        {
            if (tokenByteSize < 1)
            {
                tokenByteSize = 1;
            }
            //byte数组A 8字节 long? tokenId
            //byte数组B tokenByteSize字节 随机生成 默认长度16
            //检验码 做一个SHA256
            //AB拼接 输出hex字符串
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] numData = BitConverter.GetBytes(tokenId.Value);
                byte[] randomData = new byte[tokenByteSize];
                rng.GetBytes(randomData);

                byte[] tokenData = new byte[numData.Length + randomData.Length];
                Array.Copy(numData, 0, tokenData, 0, numData.Length);
                Array.Copy(randomData, 0, tokenData, numData.Length, randomData.Length);

                // string token = Convert.ToBase64String(tokenData);
                string token = HexUtil.ByteArrayToString(tokenData);

                var secret = this.config.SecurityConfig.Secret;

                var hmac = SecurityUtil.SignHamc256(token, secret);
                token = token + "@" + hmac;

                Console.WriteLine();
                return token;
            }
        }

        public Token GetTokenByTokenStr(string tokenStr)
        {
            if (tokenStr == null)
            {
                return null;
            }

            var result = dataContext.Token
                       .Where(b => b.TokenStr.Equals(tokenStr)).FirstOrDefault();
            return result;
        }
        /// <summary>
        /// 判断Token合法性
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool VerifyToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            var sp = token.Split("@");
            if (sp == null || sp.Length != 2)
            {
                return false;
            }
            return SecurityUtil.VerifyHamc256(sp[0], config.SecurityConfig.Secret,sp[1]);
        }

        public User GetUserByToken(string token)
        {
            if (token == null)
            {
                return null;
            }
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

        public bool DeleteTokenByToken(string token)
        {
            dataContext.Token.Where(a => a.TokenStr.Equals(token));
            return dataContext.SaveChanges() > 0;
        }

        /// <summary>
        ///  生成并且插入
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Token GenerateToken()
        {
            long? tokenid = SnowFlakeNet.GenerateSnowFlakeID();
            var tokenContext = GenerateTokenContext(tokenid);
            Token myToken = new Token
            {
                TokenId = SnowFlakeNet.GenerateSnowFlakeID(),

                TokenStr = tokenContext,
                TokenType = 0,
                CreatedTime = DateTime.Now
            };

            return myToken;
        }

        // 生成token
        public string NewToken(long? userId, string email, int tokenType)
        {
            throw new Exception();
        }

        // 删除token
        public bool DeleteToken(long? userId, int tokenType)
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