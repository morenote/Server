using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Models.Entity.Leanote.User;
using System;
using System.Linq;
using System.Text;

namespace MoreNote.Logic.Service
{
    public class TokenSerivce
    {
        private DataContext dataContext;
        private WebSiteConfig config;
        private IDistributedIdGenerator idGenerator;

        public TokenSerivce(DataContext dataContext, ConfigFileService configFileService, IDistributedIdGenerator idGenerator)
        {
            this.idGenerator = idGenerator;

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

        public Token GenerateToken(UserInfo user)
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
        /// <param name="tokenByteSize">不可预测部分的byte长度 字节 默认8</param>
        /// <returns></returns>
        public string GenerateTokenContext(long? tokenId, int tokenByteSize = 8)
        {
            if (tokenByteSize < 1)
            {
                tokenByteSize = 1;
            }
            /**
             * 客户端不应该对token最任何形式的假设，应当作为一个随机字符串处理
             * 系统可能在之后的版本做出新的更改
             * 生成流程：
             * byte数组A 8字节 long? tokenId
             * byte数组B randomHex 随机生成 默认长度16
             * C=SHA256HMAC(A+B)
             * Token=A+B+C,是用@符合分割
             * 
             * token传输到系统之后，系统是用hmac验证token合法性
             * token合法之后，系统会进行数据库查询，验证token的签发是否有效
             * 因此,签名部分引用于验证token是数据否被篡改
             * 
             * todo:修改为JWT验证方式
             * **/



            // 产生8字节的随机盐
            var randomHex = RandomTool.CreatSafeRandomHex(8);
            //拼接token
            var tokenBuider = new StringBuilder();
            tokenBuider.Append(tokenId.ToHex());
            tokenBuider.Append("@");
            tokenBuider.Append(randomHex);
            //获取配置文件密钥
            var secret = this.config.SecurityConfig.Secret;
            //使用配置文件密钥对token进行签名
            var sign = SecurityUtil.SignHamc256Hex(tokenBuider.ToString(), secret);
            //拼接上sign
            tokenBuider.Append("@");
            tokenBuider.Append(sign);
            return tokenBuider.ToString();
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
            var id = sp[0];
            var randomData = sp[1];
            var sign = sp[2];
            var tokenBuider = new StringBuilder();
            tokenBuider.Append(id);
            tokenBuider.Append("@");
            tokenBuider.Append(randomData);
            var secret = config.SecurityConfig.Secret;
            //计算hmac
            return SecurityUtil.VerifyHamc256Hex(tokenBuider.ToString(), secret, sign);
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
            return SecurityUtil.VerifyHamc256Hex(tokenBuider.ToString(), secret, sign);
        }

        /// <summary>
        /// 校验token,并获取token代表的user
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserInfo GetUserByToken(string token)
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
        public Token GenerateToken(long? userId, string email)
        {
            long? tokenid = idGenerator.NextId();
            var tokenContext = GenerateTokenContext(tokenid);
            Token myToken = new Token
            {
                Id = idGenerator.NextId(),
                UserId = userId,
                Email = email,
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