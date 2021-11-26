using Microsoft.EntityFrameworkCore;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Models.Entity.Leanote;
using MoreNote.Logic.Service;

namespace MoreNote.Logic.DB
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
        public DataContext(DbContextOptions<DataContext> options)
          : base(options)
        {



        }
       
     


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //建立索引
            //  modelBuilder.Entity<UserEntity>().HasIndex(b => b.Userid);
            //枚举类型转换
            modelBuilder
                .Entity<NoteFile>()
                .Property(e => e.StorageType)
                .HasConversion<int>();
            modelBuilder
                  .Entity<AttachInfo>()
                  .Property(e => e.StorageType)
                  .HasConversion<int>();
        }

        public DbSet<Album> Album { get; set; }
        public DbSet<AttachInfo> AttachInfo { get; set; }
        public DbSet<BlogInfoCustom> BlogInfoCustom { get; set; }
        public DbSet<Post> Post { get; set; }

        //public DbSet<ArchiveMonth> ArchiveMonth { get; set;}
        //public DbSet<Archive> Archive { get; set;}
        public DbSet<Cate> Cate { get; set; }

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

        
        public DbSet<Config> Config { get; set; }

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
        /**************Tag DB****************/
        public DbSet<Tag> Tag { get; set; }
        public DbSet<NoteTag> NoteTag { get; set; }
        public DbSet<TagCount> TagCount { get; set; }
        public DbSet<NoteTagMap> NoteTagMap { get;set;}

        public DbSet<Theme> Theme { get; set; }
        public DbSet<FriendLinks> FriendLinks { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserAccount> UserAccount { get; set; }

        //应用更新服务
        public DbSet<AppInfo> AppInfo { get; set; }

        public DbSet<AccessRecords> AccessRecords { get; set; }

        //随机图片服务
        public DbSet<RandomImage> RandomImage { get; set; }

        public DbSet<ResolutionStrategy> ResolutionStrategy { get; set; }
        public DbSet<ResolutionLocation> ResolutionLocation { get; set; }

        //public DbSet<RandomImage> WebReportInfo { get; set; }

        //支付功能
        public DbSet<CommodityOrder> GoodOrder { get; set; }

        public DbSet<SpamInfo> SpamInfo { get; set; }
    }
}