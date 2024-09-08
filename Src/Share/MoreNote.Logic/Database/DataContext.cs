using Microsoft.EntityFrameworkCore;

using MoreNote.Config.ConfigFile;
using MoreNote.Config.ConfigFilePath.IMPL;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Models.Entity.Leanote;
using MoreNote.Models.Entity.Leanote.Blog;
using MoreNote.Models.Entity.Leanote.Management;
using MoreNote.Models.Entity.Leanote.Management.Email;
using MoreNote.Models.Entity.Leanote.Management.Loggin;
using MoreNote.Models.Entity.Leanote.MyOrganization;
using MoreNote.Models.Entity.Leanote.MyRepository;
using MoreNote.Models.Entity.Leanote.Notes;
using MoreNote.Models.Entity.Leanote.Pay;
using MoreNote.Models.Entity.Leanote.Security;
using MoreNote.Models.Entity.Leanote.Synchronized;
using MoreNote.Models.Entity.Leanote.User;
using MoreNote.Models.Entity.Security.FIDO2;
using MoreNote.Models.Entiys.Leanote.Notes;

namespace MoreNote.Logic.Database
{
	public class DataContext : DbContext
	{

		private WebSiteConfig config;

		public DataContext(DbContextOptions<DataContext> options)
		  : base(options)
		{
			this.config = new ConfigFileService(new ServerConfigFilePahFinder()).ReadConfig();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//建立索引
			//  modelBuilder.Entity<UserEntity>().HasIndex(b => b.Userid);
			//枚举类型转换
			if (this.config.SecurityConfig.DataBaseEncryption)
			{
				var aesKey = this.config.SecurityConfig.DataBaseEncrypthonKey;
				//if (!string.IsNullOrEmpty(aesKey))
				//{
				//    var aesProvider = new Microsoft.EntityFrameworkCore.DataEncryption.Providers.AesProvider(aesKey.HexToByteArray());
				//    modelBuilder.UseEncryption(aesProvider);
				//}
			}

			modelBuilder
				.Entity<NoteFile>()
				.Property(e => e.StorageType)
				.HasConversion<int>();
			modelBuilder
				  .Entity<AttachInfo>()
				  .Property(e => e.StorageType)
				  .HasConversion<int>();


			modelBuilder.Entity<UserInfo>()
			   .HasMany(b => b.FIDO2Items)
			   .WithOne();
			modelBuilder
				.Entity<UserInfo>()
				.Property(e => e.PreferredMarkdownEditor)
				.HasConversion<string>();
			modelBuilder
			   .Entity<UserInfo>()
			   .Property(e => e.PreferredRichTextEditor)
			   .HasConversion<string>();
			modelBuilder
			  .Entity<Note>()
			  .Property(e => e.ExtendedName)
			  .HasConversion<string>();

			modelBuilder.Entity<AccessRecords>()
				.Property(e=>e.AccessTime)
				.HasConversion<string>();
			//============================种子数据 数据库迁移的时候自动生成================================
		}

		public DbSet<Album> Album { get; set; }
		public DbSet<AttachInfo> AttachInfo { get; set; }
		public DbSet<BlogInfoCustom> BlogInfoCustom { get; set; }
		public DbSet<BlogPost> Post { get; set; }

		//public DbSet<ArchiveMonth> ArchiveMonth { get; set;}
		//public DbSet<Archive> Archive { get; set;}
		public DbSet<BlogCate> Cate { get; set; }

		/// <summary>
		/// Blog
		/// </summary>
		public DbSet<UserBlogBase> UserBlogBase { get; set; }

		public DbSet<UserBlogComment> UserBlogComment { get; set; }
		public DbSet<UserBlogStyle> UserBlogStyle { get; set; }
		public DbSet<UserBlog> UserBlog { get; set; }
		public DbSet<BlogStat> BlogStat { get; set; }
		public DbSet<BlogSingle> BlogSingle { get; set; }
		public DbSet<BlogLike> BlogLike { get; set; }
		/// <summary>
		/// 博客评论
		/// </summary>
		public DbSet<BlogComment> BlogComment { get; set; }
		public DbSet<BlogCommentPublic> BlogCommentPublic { get; set; }
		/// <summary>
		/// 博客域名绑定
		/// </summary>
		public DbSet<BlogHostingBundle> BlogHostingBundle { get; set; }

		/// <summary>
		/// 邮件发送日志
		/// </summary>
		public DbSet<EmailLog> EmailLog { get; set; }
		/// <summary>
		/// 笔记文件
		/// </summary>
		public DbSet<NoteFile> NoteFile { get; set; }
		public DbSet<GroupTeam> Group { get; set; }
		public DbSet<GroupTeamUser> GroupUser { get; set; }
		/// <summary>
		/// 笔记本
		/// </summary>
		public DbSet<Notebook> Notebook { get; set; }
		public DbSet<NoteCollection> NoteCollection { get; set; }
		/// <summary>
		/// 笔记图片
		/// </summary>
		public DbSet<NoteImage> NoteImage { get; set; }

		/**************Note DB****************/
		/// <summary>
		/// 笔记信息
		/// </summary>
		public DbSet<Note> Note { get; set; }
		/// <summary>
		/// 笔记内容
		/// </summary>
		public DbSet<NoteContent> NoteContent { get; set; }
		/// <summary>
		/// 举报信息
		/// </summary>
		public DbSet<ReportInfo> ReportInfo { get; set; }
        /// <summary>
        /// Session
        /// </summary>
        public DbSet<Session> Session { get; set; }
		/// <summary>
		/// 建议
		/// </summary>
		public DbSet<Suggestion> Suggestion { get; set; }
		/*******************NotesRepository***********************/

		//笔记仓库
		public DbSet<Repository> Repository { get; set; }
		//仓库成员角色
		public DbSet<RepositoryMemberRole> RepositoryMemberRole { get; set; }
        /// <summary>
        /// 仓库成员角色映射
        /// </summary>
        public DbSet<RepositoryMemberRoleMapping> RepositoryMemberRoleMapping { get; set; }
        /// <summary>
        /// 仓库成员
        /// </summary>
        public DbSet<RepositoryMember> RepositoryMember { get; set; }

		/****************************************************************/

		/// <summary>
		/// 组织
		/// </summary>
		public DbSet<Organization> Organization { get; set; }
		/// <summary>
		/// 组织成员
		/// </summary>
		public DbSet<OrganizationMember> OrganizationMember { get; set; }
		/// <summary>
		/// 组织成员角色
		/// </summary>
		public DbSet<OrganizationMemberRole> OrganizationMemberRole { get; set; }
        /// <summary>
        /// 组织成员和角色映射关系
        /// </summary>
        public DbSet<OrganizationMemberRoleMapping> OrganizationMemberRoleMapping { get; set; }
		/// <summary>
		/// 组织小队
		/// </summary>
		public DbSet<OrganizationTeam> OrganizationTeam { get; set; }
        /// <summary>
        /// 组织小队成员
        /// </summary>

        public DbSet<OrganizationTeamMember> OrganizationTeamMember { get; set; }

		/**************Tag DB****************/
		public DbSet<Tag> Tag { get; set; }
		public DbSet<NoteTag> NoteTag { get; set; }
		public DbSet<TagCount> TagCount { get; set; }
		public DbSet<NoteTagMap> NoteTagMap { get; set; }
		/// <summary>
		/// 主题
		/// </summary>
		public DbSet<Theme> Theme { get; set; }
		/// <summary>
		/// 友链
		/// </summary>
		public DbSet<FriendLinks> FriendLinks { get; set; }
		/// <summary>
		/// token
		/// </summary>
		public DbSet<Token> Token { get; set; }
		/// <summary>
		/// 用户
		/// </summary>
		public DbSet<UserInfo> User { get; set; }
		/// <summary>
		/// UKEY绑定
		/// </summary>
		public DbSet<USBKeyBinding> USBKeyBindings { get; set; }
		/// <summary>
		/// FIDO2密钥
		/// </summary>
		public DbSet<FIDO2Item> Fido2Items { get; set; }
		/// <summary>
		/// 用户账号
		/// </summary>
		public DbSet<UserAccount> UserAccount { get; set; }
        /// <summary>
        /// 访问日志
        /// </summary>
        public DbSet<AccessRecords> AccessRecords { get; set; }
		//支付功能
		public DbSet<CommodityOrder> GoodOrder { get; set; }
		/// <summary>
		/// 机器学习识别垃圾信息
		/// </summary>
		public DbSet<SpamInfo> SpamInfo { get; set; }
		/// <summary>
		/// 登录日志
		/// </summary>
		public DbSet<LoggingLogin> LoggingLogin { get; set; }

		public DbSet<VirtualFileInfo> VirtualFileInfo { get; set; }
		public DbSet<VirtualFolderInfo> VirtualFolderInfo { get; set; }


		public DbSet<SubmitTree> SubmitTrees { get; set; }
		public DbSet<SubmitBlock> SubmitBlocks { get; set; }
		public DbSet<SubmitOperation> SubmitOperations { get; set; }


	}
}