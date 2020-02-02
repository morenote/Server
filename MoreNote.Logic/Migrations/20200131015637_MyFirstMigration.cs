using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MoreNote.Logic.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    AlbumId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    seq = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.AlbumId);
                });

            migrationBuilder.CreateTable(
                name: "AppInfo",
                columns: table => new
                {
                    appid = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    appautor = table.Column<string>(nullable: true),
                    appdetail = table.Column<string>(nullable: true),
                    appname = table.Column<string>(nullable: true),
                    apppackage = table.Column<string>(nullable: true),
                    appdownurl = table.Column<string>(nullable: true),
                    applogourl = table.Column<string>(nullable: true),
                    appversion = table.Column<string>(nullable: true),
                    imglist = table.Column<string[]>(nullable: true),
                    appsize = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInfo", x => x.appid);
                });

            migrationBuilder.CreateTable(
                name: "Archive",
                columns: table => new
                {
                    ArchiveId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archive", x => x.ArchiveId);
                });

            migrationBuilder.CreateTable(
                name: "AttachInfo",
                columns: table => new
                {
                    AttachId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NoteId = table.Column<long>(nullable: false),
                    UploadUserId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Size = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachInfo", x => x.AttachId);
                });

            migrationBuilder.CreateTable(
                name: "BlogComment",
                columns: table => new
                {
                    CommentId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NoteId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    ToCommentId = table.Column<string>(nullable: true),
                    ToUserId = table.Column<long>(nullable: false),
                    LikeNum = table.Column<int>(nullable: false),
                    LikeUserIds = table.Column<long[]>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogComment", x => x.CommentId);
                });

            migrationBuilder.CreateTable(
                name: "BlogCommentPublic",
                columns: table => new
                {
                    BlogCommentPublicId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsILikeIt = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCommentPublic", x => x.BlogCommentPublicId);
                });

            migrationBuilder.CreateTable(
                name: "BlogInfoCustom",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(nullable: true),
                    UserLogo = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    SubTitle = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    OpenComment = table.Column<string>(nullable: true),
                    CommentType = table.Column<string>(nullable: true),
                    ThemeId = table.Column<string>(nullable: true),
                    SubDomain = table.Column<string>(nullable: true),
                    Domain = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogInfoCustom", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BlogLike",
                columns: table => new
                {
                    LikeId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NoteId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogLike", x => x.LikeId);
                });

            migrationBuilder.CreateTable(
                name: "BlogSingle",
                columns: table => new
                {
                    SingleId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    UrlTitle = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogSingle", x => x.SingleId);
                });

            migrationBuilder.CreateTable(
                name: "BlogStat",
                columns: table => new
                {
                    NodeId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReadNum = table.Column<int>(nullable: false),
                    LikeNum = table.Column<int>(nullable: false),
                    CommentNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogStat", x => x.NodeId);
                });

            migrationBuilder.CreateTable(
                name: "BlogUrls",
                columns: table => new
                {
                    BlogUrlsId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IndexUrl = table.Column<string>(nullable: true),
                    CateUrl = table.Column<string>(nullable: true),
                    SearchUrl = table.Column<string>(nullable: true),
                    SingleUrl = table.Column<string>(nullable: true),
                    PostUrl = table.Column<string>(nullable: true),
                    ArchiveUrl = table.Column<string>(nullable: true),
                    TagsUrl = table.Column<string>(nullable: true),
                    TagPostsUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogUrls", x => x.BlogUrlsId);
                });

            migrationBuilder.CreateTable(
                name: "Cate",
                columns: table => new
                {
                    CateId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentCateId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    UrlTitle = table.Column<string>(nullable: true),
                    CateId1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cate", x => x.CateId);
                    table.ForeignKey(
                        name: "FK_Cate_Cate_CateId1",
                        column: x => x.CateId1,
                        principalTable: "Cate",
                        principalColumn: "CateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    ConfigId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    ValueStr = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Config", x => x.ConfigId);
                });

            migrationBuilder.CreateTable(
                name: "EmailLog",
                columns: table => new
                {
                    LogId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Msg = table.Column<string>(nullable: true),
                    Ok = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLog", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    FileId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    AlbumId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    IsDefaultAlbum = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    FromFileId = table.Column<long>(nullable: false),
                    NumberOfFileReferences = table.Column<int>(nullable: false),
                    sha1 = table.Column<string>(nullable: true),
                    md5 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.FileId);
                });

            migrationBuilder.CreateTable(
                name: "FriendLinks",
                columns: table => new
                {
                    FriendLinksId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ThemeId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendLinks", x => x.FriendLinksId);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    GroupId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    UserCount = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    GroupUserId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => x.GroupUserId);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    NoteId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    CreatedUserId = table.Column<long>(nullable: false),
                    NotebookId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    Src = table.Column<string>(nullable: true),
                    ImgSrc = table.Column<string>(nullable: true),
                    Tags = table.Column<string[]>(nullable: true),
                    IsTrash = table.Column<bool>(nullable: false),
                    IsBlog = table.Column<bool>(nullable: false),
                    UrlTitle = table.Column<string>(nullable: true),
                    IsRecommend = table.Column<bool>(nullable: false),
                    IsTop = table.Column<bool>(nullable: false),
                    HasSelfDefined = table.Column<bool>(nullable: false),
                    ReadNum = table.Column<int>(nullable: false),
                    LikeNum = table.Column<int>(nullable: false),
                    CommentNum = table.Column<int>(nullable: false),
                    IsMarkdown = table.Column<bool>(nullable: false),
                    AttachNum = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    RecommendTime = table.Column<DateTime>(nullable: false),
                    PublicTime = table.Column<DateTime>(nullable: false),
                    UpdatedUserId = table.Column<long>(nullable: false),
                    Usn = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsPublicShare = table.Column<bool>(nullable: false),
                    ContentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.NoteId);
                });

            migrationBuilder.CreateTable(
                name: "Notebook",
                columns: table => new
                {
                    NotebookId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    ParentNotebookId = table.Column<long>(nullable: false),
                    Seq = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    UrlTitle = table.Column<string>(nullable: true),
                    NumberNotes = table.Column<int>(nullable: false),
                    IsTrash = table.Column<bool>(nullable: false),
                    IsBlog = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    IsWX = table.Column<bool>(nullable: false),
                    Usn = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notebook", x => x.NotebookId);
                });

            migrationBuilder.CreateTable(
                name: "NoteContent",
                columns: table => new
                {
                    NoteContentId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NoteId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    IsBlog = table.Column<bool>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Abstract = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedUserId = table.Column<long>(nullable: false),
                    IsHistory = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteContent", x => x.NoteContentId);
                });

            migrationBuilder.CreateTable(
                name: "NoteImage",
                columns: table => new
                {
                    NoteImageId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NoteId = table.Column<long>(nullable: false),
                    ImageId = table.Column<long>(nullable: false),
                    userCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteImage", x => x.NoteImageId);
                });

            migrationBuilder.CreateTable(
                name: "Page",
                columns: table => new
                {
                    PageId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurPage = table.Column<int>(nullable: false),
                    TotalPage = table.Column<int>(nullable: false),
                    PerPageSize = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page", x => x.PageId);
                });

            migrationBuilder.CreateTable(
                name: "Reporter",
                columns: table => new
                {
                    ReporterId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WebSite = table.Column<string>(nullable: true),
                    IsIdentify = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporter", x => x.ReporterId);
                });

            migrationBuilder.CreateTable(
                name: "ReportInfo",
                columns: table => new
                {
                    ReportId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NoteId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    CommentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportInfo", x => x.ReportId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceProviderLegalPerson",
                columns: table => new
                {
                    PersonId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    About = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProviderLegalPerson", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionId = table.Column<long>(nullable: false),
                    LoginTimes = table.Column<int>(nullable: false),
                    Captcha = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suggestion",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    Addr = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "TagCount",
                columns: table => new
                {
                    TagCountId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    Tag = table.Column<string>(nullable: true),
                    IsBlog = table.Column<bool>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagCount", x => x.TagCountId);
                });

            migrationBuilder.CreateTable(
                name: "Theme",
                columns: table => new
                {
                    ThemeId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    AuthorUrl = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Info = table.Column<string[]>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    Style = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theme", x => x.ThemeId);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                columns: table => new
                {
                    TokenId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    TokenStr = table.Column<string>(nullable: true),
                    tokenTag = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.TokenId);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountType = table.Column<string>(nullable: true),
                    AccountStartTime = table.Column<DateTime>(nullable: false),
                    AccountEndTime = table.Column<DateTime>(nullable: false),
                    MaxImageNum = table.Column<int>(nullable: false),
                    MaxImageSize = table.Column<int>(nullable: false),
                    MaxAttachNum = table.Column<int>(nullable: false),
                    MaxAttachSize = table.Column<int>(nullable: false),
                    MaxPerAttachSize = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserAndBlog",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    BlogTitle = table.Column<string>(nullable: true),
                    BlogLogo = table.Column<string>(nullable: true),
                    BlogUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAndBlog", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserAndBlogUrl",
                columns: table => new
                {
                    UserAndBlogUrlId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BlogUrl = table.Column<string>(nullable: true),
                    PostUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAndBlogUrl", x => x.UserAndBlogUrlId);
                });

            migrationBuilder.CreateTable(
                name: "UserBlog",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Logo = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    SubTitle = table.Column<string>(nullable: true),
                    AboutMe = table.Column<string>(nullable: true),
                    CanComment = table.Column<bool>(nullable: false),
                    CommentType = table.Column<string>(nullable: true),
                    DisqusId = table.Column<string>(nullable: true),
                    Style = table.Column<string>(nullable: true),
                    Css = table.Column<string>(nullable: true),
                    ThemeId = table.Column<long>(nullable: false),
                    ThemePath = table.Column<string>(nullable: true),
                    CateIds = table.Column<string[]>(nullable: true),
                    Singles = table.Column<string[]>(nullable: true),
                    PerPageSize = table.Column<int>(nullable: false),
                    SortField = table.Column<string>(nullable: true),
                    IsAsc = table.Column<bool>(nullable: false),
                    SubDomain = table.Column<string>(nullable: true),
                    Domain = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlog", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserBlogBase",
                columns: table => new
                {
                    Logo = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    SubTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlogBase", x => x.Logo);
                });

            migrationBuilder.CreateTable(
                name: "UserBlogComment",
                columns: table => new
                {
                    UserBlogCommentId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanComment = table.Column<bool>(nullable: false),
                    CommentType = table.Column<string>(nullable: true),
                    DisqusId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlogComment", x => x.UserBlogCommentId);
                });

            migrationBuilder.CreateTable(
                name: "UserBlogStyle",
                columns: table => new
                {
                    UserBlogStyleId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Style = table.Column<string>(nullable: true),
                    Css = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBlogStyle", x => x.UserBlogStyleId);
                });

            migrationBuilder.CreateTable(
                name: "ArchiveMonth",
                columns: table => new
                {
                    ArchiveMonthId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Month = table.Column<int>(nullable: false),
                    ArchiveId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveMonth", x => x.ArchiveMonthId);
                    table.ForeignKey(
                        name: "FK_ArchiveMonth_Archive_ArchiveId",
                        column: x => x.ArchiveId,
                        principalTable: "Archive",
                        principalColumn: "ArchiveId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(nullable: true),
                    Verified = table.Column<bool>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    UsernameRaw = table.Column<string>(nullable: true),
                    Pwd = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    Cost = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    Logo = table.Column<string>(nullable: true),
                    Theme = table.Column<string>(nullable: true),
                    NotebookWidth = table.Column<int>(nullable: false),
                    NoteListWidth = table.Column<int>(nullable: false),
                    MdEditorWidth = table.Column<int>(nullable: false),
                    LeftIsMin = table.Column<bool>(nullable: false),
                    ThirdUserId = table.Column<string>(nullable: true),
                    ThirdUsername = table.Column<string>(nullable: true),
                    ThirdType = table.Column<int>(nullable: false),
                    ImageNum = table.Column<int>(nullable: false),
                    ImageSize = table.Column<int>(nullable: false),
                    AttachNum = table.Column<int>(nullable: false),
                    AttachSize = table.Column<int>(nullable: false),
                    FromUserId = table.Column<string>(nullable: true),
                    AccountType = table.Column<int>(nullable: false),
                    AccountStartTime = table.Column<DateTime>(nullable: false),
                    AccountEndTime = table.Column<DateTime>(nullable: false),
                    MaxImageNum = table.Column<int>(nullable: false),
                    MaxImageSize = table.Column<int>(nullable: false),
                    MaxAttachNum = table.Column<int>(nullable: false),
                    MaxAttachSize = table.Column<int>(nullable: false),
                    MaxPerAttachSize = table.Column<int>(nullable: false),
                    Usn = table.Column<int>(nullable: false),
                    FullSyncBefore = table.Column<DateTime>(nullable: false),
                    GroupId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceProviderCompany",
                columns: table => new
                {
                    ServiceProviderCompanyId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SPName = table.Column<string>(nullable: true),
                    LegalPersonPersonId = table.Column<long>(nullable: true),
                    RegionDate = table.Column<DateTime>(nullable: false),
                    WebSite = table.Column<string>(nullable: true),
                    OldWebSite = table.Column<string[]>(nullable: true),
                    RegistrationPlace = table.Column<string>(nullable: true),
                    FoundDate = table.Column<DateTime>(nullable: false),
                    IsBlock = table.Column<bool>(nullable: false),
                    RiskIndex = table.Column<int>(nullable: false),
                    MentionByName = table.Column<int>(nullable: false),
                    AnomalyDetection = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProviderCompany", x => x.ServiceProviderCompanyId);
                    table.ForeignKey(
                        name: "FK_ServiceProviderCompany_ServiceProviderLegalPerson_LegalPers~",
                        column: x => x.LegalPersonPersonId,
                        principalTable: "ServiceProviderLegalPerson",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NoteTag",
                columns: table => new
                {
                    TagId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(nullable: false),
                    Tag = table.Column<string>(nullable: true),
                    Usn = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TagUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTag", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_NoteTag_Tag_TagUserId",
                        column: x => x.TagUserId,
                        principalTable: "Tag",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    NoteId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: true),
                    UrlTitle = table.Column<string>(nullable: true),
                    ImgSrc = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    PublicTime = table.Column<DateTime>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    Abstract = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Tags = table.Column<string[]>(nullable: true),
                    CommentNum = table.Column<int>(nullable: false),
                    ReadNum = table.Column<int>(nullable: false),
                    LikeNum = table.Column<int>(nullable: false),
                    IsMarkdown = table.Column<bool>(nullable: false),
                    ArchiveId = table.Column<long>(nullable: true),
                    ArchiveMonthId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Post_Archive_ArchiveId",
                        column: x => x.ArchiveId,
                        principalTable: "Archive",
                        principalColumn: "ArchiveId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Post_ArchiveMonth_ArchiveMonthId",
                        column: x => x.ArchiveMonthId,
                        principalTable: "ArchiveMonth",
                        principalColumn: "ArchiveMonthId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogItem",
                columns: table => new
                {
                    NoteId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Abstract = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    HasMore = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogItem", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_BlogItem_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HostServiceProvider",
                columns: table => new
                {
                    HostServiceProviderId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HostName = table.Column<string>(nullable: true),
                    ServiceProviderCompanyId = table.Column<long>(nullable: true),
                    ServiceType = table.Column<string>(nullable: true),
                    ISP = table.Column<string>(nullable: true),
                    BeiAnGov = table.Column<string>(nullable: true),
                    WebSite = table.Column<string>(nullable: true),
                    OldWebSite = table.Column<string[]>(nullable: true),
                    RegistrationPlace = table.Column<string>(nullable: true),
                    FoundDate = table.Column<DateTime>(nullable: false),
                    IsBlock = table.Column<bool>(nullable: false),
                    RiskIndex = table.Column<int>(nullable: false),
                    MentionByName = table.Column<int>(nullable: false),
                    AnomalyDetection = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostServiceProvider", x => x.HostServiceProviderId);
                    table.ForeignKey(
                        name: "FK_HostServiceProvider_ServiceProviderCompany_ServiceProviderC~",
                        column: x => x.ServiceProviderCompanyId,
                        principalTable: "ServiceProviderCompany",
                        principalColumn: "ServiceProviderCompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SecretReport",
                columns: table => new
                {
                    SecretReportId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HostServiceProviderId = table.Column<long>(nullable: true),
                    ServiceProviderCompanyId = table.Column<long>(nullable: true),
                    ReporterId = table.Column<long>(nullable: true),
                    IsRisk = table.Column<bool>(nullable: false),
                    ReportContent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecretReport", x => x.SecretReportId);
                    table.ForeignKey(
                        name: "FK_SecretReport_HostServiceProvider_HostServiceProviderId",
                        column: x => x.HostServiceProviderId,
                        principalTable: "HostServiceProvider",
                        principalColumn: "HostServiceProviderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SecretReport_Reporter_ReporterId",
                        column: x => x.ReporterId,
                        principalTable: "Reporter",
                        principalColumn: "ReporterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SecretReport_ServiceProviderCompany_ServiceProviderCompanyId",
                        column: x => x.ServiceProviderCompanyId,
                        principalTable: "ServiceProviderCompany",
                        principalColumn: "ServiceProviderCompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveMonth_ArchiveId",
                table: "ArchiveMonth",
                column: "ArchiveId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogItem_UserId",
                table: "BlogItem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cate_CateId1",
                table: "Cate",
                column: "CateId1");

            migrationBuilder.CreateIndex(
                name: "IX_HostServiceProvider_ServiceProviderCompanyId",
                table: "HostServiceProvider",
                column: "ServiceProviderCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTag_TagUserId",
                table: "NoteTag",
                column: "TagUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_ArchiveId",
                table: "Post",
                column: "ArchiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_ArchiveMonthId",
                table: "Post",
                column: "ArchiveMonthId");

            migrationBuilder.CreateIndex(
                name: "IX_SecretReport_HostServiceProviderId",
                table: "SecretReport",
                column: "HostServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_SecretReport_ReporterId",
                table: "SecretReport",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_SecretReport_ServiceProviderCompanyId",
                table: "SecretReport",
                column: "ServiceProviderCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProviderCompany_LegalPersonPersonId",
                table: "ServiceProviderCompany",
                column: "LegalPersonPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_User_GroupId",
                table: "User",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "AppInfo");

            migrationBuilder.DropTable(
                name: "AttachInfo");

            migrationBuilder.DropTable(
                name: "BlogComment");

            migrationBuilder.DropTable(
                name: "BlogCommentPublic");

            migrationBuilder.DropTable(
                name: "BlogInfoCustom");

            migrationBuilder.DropTable(
                name: "BlogItem");

            migrationBuilder.DropTable(
                name: "BlogLike");

            migrationBuilder.DropTable(
                name: "BlogSingle");

            migrationBuilder.DropTable(
                name: "BlogStat");

            migrationBuilder.DropTable(
                name: "BlogUrls");

            migrationBuilder.DropTable(
                name: "Cate");

            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.DropTable(
                name: "EmailLog");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "FriendLinks");

            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "Notebook");

            migrationBuilder.DropTable(
                name: "NoteContent");

            migrationBuilder.DropTable(
                name: "NoteImage");

            migrationBuilder.DropTable(
                name: "NoteTag");

            migrationBuilder.DropTable(
                name: "Page");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "ReportInfo");

            migrationBuilder.DropTable(
                name: "SecretReport");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "Suggestion");

            migrationBuilder.DropTable(
                name: "TagCount");

            migrationBuilder.DropTable(
                name: "Theme");

            migrationBuilder.DropTable(
                name: "Token");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropTable(
                name: "UserAndBlog");

            migrationBuilder.DropTable(
                name: "UserAndBlogUrl");

            migrationBuilder.DropTable(
                name: "UserBlog");

            migrationBuilder.DropTable(
                name: "UserBlogBase");

            migrationBuilder.DropTable(
                name: "UserBlogComment");

            migrationBuilder.DropTable(
                name: "UserBlogStyle");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "ArchiveMonth");

            migrationBuilder.DropTable(
                name: "HostServiceProvider");

            migrationBuilder.DropTable(
                name: "Reporter");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "Archive");

            migrationBuilder.DropTable(
                name: "ServiceProviderCompany");

            migrationBuilder.DropTable(
                name: "ServiceProviderLegalPerson");
        }
    }
}
