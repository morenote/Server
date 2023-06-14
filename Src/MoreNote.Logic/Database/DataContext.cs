using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;

using MoreNote.AutoFac;
using MoreNote.Common.ExtensionMethods;
using MoreNote.CryptographyProvider;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
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

using System;
using System.Collections.Generic;

namespace MoreNote.Logic.Database
{
    public class DataContext : DbContext
    {
        //public DataContext()
        //{
        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    //测试服务器
        //    ConfigFileService configFileService = new ConfigFileService();
        //    var postgres = configFileService.WebConfig;
        //    optionsBuilder.UseNpgsql(postgres.PostgreSql.Connection);

        //}
        private WebSiteConfig config;
     
      


        public DataContext(DbContextOptions<DataContext> options)
          : base(options)
        {
            this.config = new ConfigFileService().WebConfig;
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
        public DbSet<BlogComment> BlogComment { get; set; }
        public DbSet<BlogCommentPublic> BlogCommentPublic { get; set; }
        public DbSet<BlogHostingBundle> BlogHostingBundle { get; set; }


        public DbSet<EmailLog> EmailLog { get; set; }
        public DbSet<NoteFile> NoteFile { get; set; }
        public DbSet<GroupTeam> Group { get; set; }
        public DbSet<GroupTeamUser> GroupUser { get; set; }
        public DbSet<Notebook> Notebook { get; set; }
        public DbSet<NoteImage> NoteImage { get; set; }

        /**************Note DB****************/
        public DbSet<Note> Note { get; set; }
        public DbSet<NoteContent> NoteContent { get; set; }

        public DbSet<ReportInfo> ReportInfo { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<Suggestion> Suggestion { get; set; }
        /*******************NotesRepository***********************/

        //笔记仓库
        public DbSet<Repository> Repository { get; set; }

     

        //仓库成员角色
        public DbSet<RepositoryMemberRole> RepositoryMemberRole { get; set; }
        public DbSet<RepositoryMemberRoleMapping> RepositoryMemberRoleMapping { get; set; }

        //仓库成员
        public DbSet<RepositoryMember> RepositoryMember { get; set; }

        /****************************************************************/
        public DbSet<Organization> Organization { get; set; }
        public DbSet<OrganizationMember> OrganizationMember { get; set; }
        public DbSet<OrganizationMemberRole> OrganizationMemberRole { get; set; }
        public DbSet<OrganizationMemberRoleMapping> OrganizationMemberRoleMapping { get; set; }
        public DbSet<OrganizationTeam> OrganizationTeam { get; set; }
        public DbSet<OrganizationTeamMember> OrganizationTeamMember { get; set; }

        /**************Tag DB****************/
        public DbSet<Tag> Tag { get; set; }
        public DbSet<NoteTag> NoteTag { get; set; }
        public DbSet<TagCount> TagCount { get; set; }
        public DbSet<NoteTagMap> NoteTagMap { get; set; }

        public DbSet<Theme> Theme { get; set; }
        public DbSet<FriendLinks> FriendLinks { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<UserInfo> User { get; set; }
        public DbSet<USBKeyBinding> USBKeyBindings { get; set; }
        public DbSet<FIDO2Item> Fido2Items { get; set; }
        public DbSet<UserAccount> UserAccount { get; set; }

        //应用更新服务
   

        public DbSet<AccessRecords> AccessRecords { get; set; }

        //随机图片服务
        public DbSet<RandomImage> RandomImage { get; set; }

 

        //public DbSet<RandomImage> WebReportInfo { get; set; }

        //支付功能
        public DbSet<CommodityOrder> GoodOrder { get; set; }

        public DbSet<SpamInfo> SpamInfo { get; set; }

        //日志
        public DbSet<LoggingLogin> LoggingLogin { get; set; }
        public DbSet<DataSignLogging> DataSignLogging { get; set; }

        public DbSet<RealNameInformation> RealNameInformation { get; set; }

        public DbSet<VirtualFileInfo> VirtualFileInfo { get; set; }
        public DbSet<VirtualFolderInfo> VirtualFolderInfo { get; set; }


        public DbSet<SubmitTree> SubmitTrees { get; set; }
        public DbSet<SubmitBlock> SubmitBlocks { get; set; }
        public DbSet<SubmitOperation> SubmitOperations { get; set; }


    }
}