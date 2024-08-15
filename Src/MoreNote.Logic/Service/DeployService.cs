using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MoreNote.Common.Utils;
using MoreNote.CryptographyProvider;
using MoreNote.Logic.Database;
using MoreNote.Logic.Service.PasswordSecurity;

using System;

namespace MoreNote.Logic.Service
{
	public class DeployService
	{
		private IServiceProvider serviceProvider;

		public DeployService(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public void MigrateDatabase()
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider is null,class DeployService  is error");
			}
			//执行数据库迁移命令
			System.Console.WriteLine(" ==================== SQL Migrate  Start====================");
			using (var scope = serviceProvider.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<DataContext>();
				var cry = scope.ServiceProvider.GetRequiredService<ICryptographyProvider>();
				var passwordStoreFactory = scope.ServiceProvider.GetRequiredService<PasswordStoreFactory>();

				db.Database.Migrate();

				//======================创建种子数据========================
				//ConfigFileService configFileService = new ConfigFileService();
				//var config = configFileService.WebConfig;
				////初始化随机口令
				//var pwd = "admin123";
				//var passwordStore = passwordStoreFactory.Instance("sha256");
				//var salt = RandomTool.CreatSafeSaltByteArray(16);
				////对用户密码做哈希运算
				//var genPassData = await passwordStore.Encryption(Encoding.UTF8.GetBytes(pwd), salt, config.SecurityConfig.PasswordHashIterations);
				//var genPass = genPassData.ByteArrayToBase64();
				////初始化默认用户
				//var userId = 1223885079105900540L;
				//if (!db.User.Where(x => x.UserId == userId).Any())
				//{
				//    var admin = new User()
				//    {
				//        UserId = userId,
				//        Email = "admin@morenote.top",
				//        Verified = true,
				//        Username = "admin",
				//        UsernameRaw = "admin",
				//        Pwd = genPass,
				//        Salt = salt.ByteArrayToBase64(),
				//        PasswordMemorySize = config.SecurityConfig.PasswordStoreMemorySize,
				//        Logo = null,
				//        Theme = null,
				//        NotebookWidth = 160,
				//        NoteListWidth = 384,
				//        MdEditorWidth = 621,
				//        LeftIsMin = false,
				//        ThirdUserId = null,
				//        ThirdUsername = null,
				//        ThirdType = 0,
				//        ImageNum = 0,
				//        ImageSize = 0,
				//        AttachSize = 0,
				//        FromUserId = null,
				//        AccountType = 0,
				//        AccountStartTime = DateTime.Now,
				//        AccountEndTime = DateTime.MaxValue,
				//        MaxImageNum = 0,
				//        MaxAttachSize = 0,
				//        MaxAttachNum = 0,
				//        MaxPerAttachSize = 0,
				//        Usn = 0,
				//        FullSyncBefore = DateTime.Now,
				//        Role = "Admin",
				//        GoogleAuthenticatorSecretKey = null,
				//        PasswordHashAlgorithm = "sha256",
				//        PasswordDegreeOfParallelism = config.SecurityConfig.PasswordStoreDegreeOfParallelism,
				//        PasswordHashIterations = config.SecurityConfig.PasswordHashIterations,
				//        BlogUrl = null,
				//        MarkdownEditorOption = "ace",
				//        RichTextEditorOption = "tinymce",
				//        ChangePasswordReminder = true
				//    };
				//    await admin.AddMac(cry);
				//    db.User.Add(admin);

				//    Console.WriteLine("Initialize the admin account");
				//    Console.WriteLine("Email=admin@morenote.top");
				//    Console.WriteLine("username=admin");
				//    Console.WriteLine($"password={pwd}");
				//}
				//else
				//{
				//    Console.WriteLine("skip");
				//}

				//if (!db.UserBlog.Where(b => b.UserId == userId).Any())
				//{
				//    db.UserBlog.Add(new UserBlog()
				//    {
				//        UserId = userId,
				//        CanComment = true,
				//        CommentType = "leanote",
				//        ThemeId = null,

				//        IsAsc = true,
				//    });
				//}

				//var list = new List<string>(4) { "life", "study", "work", "tutorial" };

				//var autoIncrementId = 1235228093334032384L;
				//foreach (var item in list)
				//{
				//    autoIncrementId++;
				//    if (!db.Notebook.Where(b => b.NotebookId == autoIncrementId).Any())
				//    {
				//        db.Notebook.Add(new Notebook()
				//        {
				//            NotebookId = autoIncrementId,
				//            Seq = 0,
				//            UserId = userId,
				//            CreatedTime = DateTime.Now,
				//            Title = item,
				//            UrlTitle = item,
				//            ParentNotebookId = null
				//        });
				//    }
				//}

				db.SaveChanges();
			
			}

			System.Console.WriteLine(" ==================== SQL Migrate  Successfully====================");
		}

		public void InitSecret()
		{
			Console.WriteLine(" ==================== Initializing Secret Start==================== ");
			ConfigFileService configFileService = new ConfigFileService();
			configFileService.WebConfig.SecurityConfig.Secret = RandomTool.CreatSafeRandomBase64(32);
			configFileService.Save();
			Console.WriteLine(" ==================== Initialized Secret Successfully ==================== ");
		}
	}
}