using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;

using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Logic.Service.PasswordSecurity;

namespace MoreNote.Logic.Service
{
    public class AuthService
    {

        private DataContext dataContext;
        public UserService UserService { get;set;}
        public TokenSerivce TokenSerivce { get;set;}
        private IPasswordStore passwordStore { get;set;}
        public NotebookService NotebookService { get;set;}
        private WebSiteConfig config;
        private IDistributedIdGenerator idGenerator;
        private PasswordStoreFactory passwordStoreFactory;
        public AuthService(DataContext dataContext, IPasswordStore passwordStore, NotebookService notebookService,ConfigFileService configFileService, IDistributedIdGenerator idGenerator, PasswordStoreFactory passwordStoreFactory)
        {
            this.idGenerator = idGenerator;
            this.dataContext = dataContext;
            this.passwordStore = passwordStore;
            this.NotebookService = notebookService;
            this.config = configFileService.WebConfig;
            this.passwordStoreFactory = passwordStoreFactory;
        }

        public  async Task<string> LoginByPWD(String email, string pwd)
        {
            User user;
            string tokenStr;
            if (email.Contains("@"))
            {
                user = UserService.GetUserByEmail(email);
            }
            else{
                user=UserService.GetUserByUserName(email);
            }
            if (user==null)
            {
                tokenStr=null;
                user=null;
                return null;
            }
            var passwordStore = passwordStoreFactory.Instance(user);

            if (user != null)
            {
                var result =  passwordStore.VerifyPassword(user.Pwd.Base64ToByteArray(), pwd.Base64ToByteArray(), user.Salt.Base64ToByteArray(), user.PasswordHashIterations);
                if (result)
                {
                    long? tokenid = idGenerator.NextId();
                    //生成token的数据
                    var tokenContext= TokenSerivce.GenerateTokenContext(tokenid);
                    Token myToken = new Token
                    {
                        TokenId = idGenerator.NextId(),
                        UserId = user.UserId,
                        Email = user.Email,
                        TokenStr = tokenContext,
                        TokenType = 0,
                        CreatedTime = DateTime.Now
                    };
                    TokenSerivce.SaveToken(myToken);
                    tokenStr = myToken.TokenStr;
                    return tokenStr;
                }
                else
                {
                    tokenStr = "";
                    return null;
                }
            }
            else
            {
                tokenStr = "";
                return null;
            }

        }
        

        public  bool LoginByToken(string email, string token)
        {

            return false;
        }
        public  bool LoginByToken( string token)
        {

            var user=TokenSerivce.GetUserByToken(token);
            return false;
        }

      
        /// <summary>
        /// 通过Token判断用户是否登录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="tokenStr"></param>
        /// <returns></returns>
        public  bool IsLogin(long? userid,string tokenStr)
        {
          
            Token token = TokenSerivce.GetTokenByTokenStr(tokenStr);
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
        public  async Task<bool> Register(string email, string pwd, long? fromUserId)
        {
            email=email.ToLower();//邮箱保存时全部使用小写形式
            var Msg = "";
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
            var salt= RandomTool.CreatSafeSaltByteArray(16);

            var passwordStore=passwordStoreFactory.Instance(config.SecurityConfig);
            //对用户密码做哈希运算
            string genPass=(  passwordStore.Encryption(pwd.Base64ToByteArray(), salt,config.SecurityConfig.PasswordHashIterations)).ByteArrayToBase64();
            if (string.IsNullOrEmpty(genPass))
            {
                Msg="密码处理过程出现错误";
                return false;
            }
            var userId=idGenerator.NextId();
            //生成一个新用户
            User user = new User()
            {
                UserId = userId,
                Email = email,
                Username = userId.ToHex(),
                UsernameRaw = userId.ToHex(),
                PasswordHashIterations=config.SecurityConfig.PasswordHashIterations,//加密强度=1
                PasswordDegreeOfParallelism= config.SecurityConfig.PasswordStoreDegreeOfParallelism,
                PasswordMemorySize=config.SecurityConfig.PasswordStoreMemorySize,
                Pwd = genPass,
                PasswordHashAlgorithm = config.SecurityConfig.PasswordHashAlgorithm,
                Salt = salt.ByteArrayToBase64(),
                FromUserId = fromUserId,
                Role="User",
                NotebookWidth=160,
                NoteListWidth=384,
                MdEditorWidth=621,
                LeftIsMin=false,
                Verified=false,
                Usn = 1
            };
            if (user.Email.Equals("admin@morenote.top"))
            {
                user.Role = "Admin";
            }
            if (await Register(user))
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


        public  async Task<bool> Register(User user)
        {
          
            if (await UserService.AddUserAsync(user))
            {
                var list=new List<string>(4){ "life", "study", "work", "tutorial" };
                foreach (var item in list)
                {
                    // 添加笔记本, 生活, 学习, 工作
                    var userId = user.UserId;
                    var notebook = new Notebook()
                    {
                        NotebookId = idGenerator.NextId(),
                        Seq = 0,
                        UserId = userId,
                        CreatedTime = DateTime.Now,
                        Title=item,
                        ParentNotebookId=null,
                    };
                    NotebookService.AddNotebook(notebook);
                }
                //用户博客信息
                var user_blog=new UserBlog()
                {
                    UserId=user.UserId,
                    CanComment=true,
                    CommentType="leanote",
                    ThemeId=null,
                    IsAsc=true,
                };
                //增加博客用户信息
                UserService.AddBlogUser(user_blog);
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
