using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MoreNote.Logic.Service.DistributedIDGenerator;

namespace MoreNote.Logic.Service
{
    public class TokenSerivce
    {
        private DataContext dataContext;
        private WebSiteConfig config;
        private IDistributedIdGenerator idGenerator;
        public TokenSerivce(DataContext dataContext, ConfigFileService configFileService, IDistributedIdGenerator idGenerator)
        {
            this.idGenerator=  idGenerator;
            
            this.dataContext = dataContext;
            this.config = configFileService.WebConfig;
        }

        public bool SaveToken(Token token)
        {
            int a = 0;

            var result = dataContext.Token.Add(token);
            a = dataContext.SaveChanges();
            return dataContext.SaveChanges() > 0;
        }
        public Token GenerateToken(User user)
        {
            long? tokenid = idGenerator.NextId();
            //生成token的数据
            var tokenContext = GenerateTokenContext(tokenid);
            Token myToken = new Token
            {
                Id = idGenerator.NextId(),
                UserId = user.Id,
                Email = user.Email,
                TokenStr = tokenContext,
                TokenType = 0,
                CreatedTime = DateTime.Now
            };
            return myToken;

        }


 

        /// <summary>
        /// 产生不可预测的Token
        /// </summary>
        /// <param name="tokenId">tokenId</param>
        /// <param name="tokenByteSize">不可预测部分的byte长度 字节 默认16</param>
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
               
                //随机盐
                byte[] randomData = new byte[tokenByteSize];
                rng.GetBytes(randomData);

                //拼接token
                var tokenBuider=new StringBuilder();
                tokenBuider.Append(tokenId.ToHex());
                tokenBuider.Append("@");
                tokenBuider.Append(randomData.ByteArrayToHex());

               
                //获取配置文件密钥
                var secret = this.config.SecurityConfig.Secret;
                //使用配置文件密钥对token进行签名
                var sign = SecurityUtil.SignHamc256(tokenBuider.ToString(), secret);
                //拼接上sign
                tokenBuider.Append("@");
                tokenBuider.Append(sign);
          
                return tokenBuider.ToString();
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
            if (sp == null || sp.Length != 3)
            {
                return false;
            }
            var id=sp[0];
            var randomData=sp[1];
            var sign= sp[2];
            var tokenBuider=new StringBuilder();
            tokenBuider.Append(id);
            tokenBuider.Append("@");
            tokenBuider.Append(randomData);
            var secret= config.SecurityConfig.Secret;
            //计算hmac
            return SecurityUtil.VerifyHamc256(tokenBuider.ToString(), secret,sign);
        }
        public bool VerifyToken(long? userId, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            var sp = token.Split("@");
            if (sp == null || sp.Length != 3)
            {
                return false;
            }
            var id = sp[0];
            var randomData = sp[1];
            var sign = sp[2];
            var tokenBuider = new StringBuilder();
            tokenBuider.Append(id);
            tokenBuider.Append("@");
            tokenBuider.Append(randomData);
            var secret = config.SecurityConfig.Secret;
            if (!userId.ToHex().Equals(id))
            {
                return false;
            }
            //计算hmac
            return SecurityUtil.VerifyHamc256(tokenBuider.ToString(), secret, sign);
        }

        /// <summary>
        /// 校验token,并获取token代表的user
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public User GetUserByToken(string token)
        {

            if (!VerifyToken(token))
            {
                return null;
            }
            if (token == null)
            {
                return null;
            }
            var result = dataContext.Token
                  .Where(b => b.TokenStr.Equals(token)).FirstOrDefault();
            if (result != null)
            {
                var user = dataContext.User
                .Where(b => b.Id == result.UserId).FirstOrDefault();
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
        public Token GenerateToken(long? userId,string email)
        {
            long? tokenid = idGenerator.NextId();
            var tokenContext = GenerateTokenContext(tokenid);
            Token myToken = new Token
            {
                Id = idGenerator.NextId(),
                UserId= userId,
                Email= email,
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