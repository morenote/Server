using System;
using System.Formats.Asn1;
using Microsoft.Extensions.DependencyInjection;
using MoreNote.Common.Utils;

using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service.PasswordSecurity;

namespace MoreNote.Logic.Service
{
    public class AuthService
    {

        private DataContext dataContext;
        public UserService UserService { get;set;}
        public TokenSerivce TokenSerivce { get;set;}
        private IPasswordStore passwordStore { get;set;}
        public AuthService(DataContext dataContext, IPasswordStore passwordStore)
        {
            this.dataContext=dataContext;
            this.passwordStore=passwordStore;
            
        }

        public  bool LoginByPWD(String email, string pwd, out string tokenStr,out User user)
        {
          
            user = UserService.GetUser(email);
            if (user != null)
            {
                
                string temp = SHAEncryptHelper.Hash256Encrypt(pwd + user.Salt);
                if (temp.Equals(user.Pwd))
                {
                    long? tokenid = SnowFlakeNet.GenerateSnowFlakeID();
                    var token= TokenSerivce.GenerateToken(tokenid);
                    Token myToken = new Token
                    {
                        TokenId = SnowFlakeNet.GenerateSnowFlakeID(),
                        UserId = user.UserId,
                        Email = user.Email,
                        TokenStr = token,
                        TokenType = 0,
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
        public  bool LoginByToken(string email, string token)
        {
            return true;
        }
        /// <summary>
        /// 通过Token判断用户是否登录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        public  bool IsLogin(long? userid,string tokenStr)
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

        // 使用bcrypt认证或者Md5认证
        // Use bcrypt (Md5 depreciated)
        public  User Login(string emailOrUserName ,string pwd)
        {
            throw new Exception();
        }
        public  bool Register(string email,string pwd,long? fromUserId)
        {
           return Register( email,  pwd,  fromUserId, out string Msg);
        }
        // 注册
        /*
        注册 leanote@leanote.com userId = "5368c1aa99c37b029d000001"
        添加 在博客上添加一篇欢迎note, note1 5368c1b919807a6f95000000

        将nk1(只读), nk2(可写) 分享给该用户
        将note1 复制到用户的生活nk上
        */
        // 1. 添加用户
        // 2. 将leanote共享给我
        // [ok]
        public  bool Register(string email, string pwd, long? fromUserId,out string Msg)
        {
            if (string.IsNullOrEmpty(email)||string.IsNullOrEmpty(pwd)||pwd.Length<6)
            {
                Msg="参数错误";
                return false;
            }
          
            if (UserService.IsExistsUser(email))
            {
               Msg= "userHasBeenRegistered-"+ email;
                return false;
            }
            //产生一个盐用于保存密码
            string salt= RandomTool.CreatSafeSalt(32);
            //对用户密码做哈希运算
            string genPass= SHAEncryptHelper.Hash256Encrypt(pwd+salt);
            if (string.IsNullOrEmpty(genPass))
            {
                Msg="密码处理过程出现错误";
                return false;
            }
            User user = new User()
            {
                UserId = SnowFlakeNet.GenerateSnowFlakeID(),
                Email = email,
                Username = email,
                Pwd_Cost=1,//一次
                Pwd = genPass,
                Salt = salt,
                FromUserId = fromUserId,
                Role="User",
                Usn = 1
            };
            if (Register(user))
            {
                Msg = "注册成功";
                return true;
            }
            else
            {
                Msg = "注册失败";
                return false;
            }

        }
        public  bool Register(User user)
        {
          
            if (UserService.AddUser(user))
            {
                return true;
            }
            else
            {
                return false;
            }

           
        }
        //第三方得到用户名, 可能需要多次判断
        public  string getUsername(string thirdType,string thirdUserName)
        {
            string username=thirdType + "-" + thirdUserName;


            throw new Exception();
        }
        public User ThirdRegister(string thirdType,string thirdUserId,string thirdUserName)
        {
            throw new Exception();
        }

    }
}
