using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MoreNote.Logic.Migrations
{
    public partial class initDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Archive_ArchiveId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_ArchiveMonth_ArchiveMonthId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "ArchiveMonth");

            migrationBuilder.DropTable(
                name: "BlogUrls");

            migrationBuilder.DropTable(
                name: "UserAndBlog");

            migrationBuilder.DropTable(
                name: "UserAndBlogUrl");

            migrationBuilder.DropTable(
                name: "Archive");

            migrationBuilder.DropIndex(
                name: "IX_Post_ArchiveId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_ArchiveMonthId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ArchiveId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ArchiveMonthId",
                table: "Post");

            migrationBuilder.AlterColumn<int>(
                name: "Cost",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Allow",
                table: "BlogComment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RemoteIpAddress",
                table: "AccessRecords",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemotePort",
                table: "AccessRecords",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "X_Forwarded_For",
                table: "AccessRecords",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "X_Real_IP",
                table: "AccessRecords",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Authorization",
                columns: table => new
                {
                    AuthorizationId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(nullable: true),
                    value = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorization", x => x.AuthorizationId);
                    table.ForeignKey(
                        name: "FK_Authorization_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoodOrder",
                columns: table => new
                {
                    GoodOrderId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mchid = table.Column<string>(nullable: true),
                    total_fee = table.Column<int>(nullable: false),
                    out_trade_no = table.Column<string>(nullable: true),
                    body = table.Column<string>(nullable: true),
                    attch = table.Column<string>(nullable: true),
                    notify_url = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    payjs_order_id = table.Column<string>(nullable: true),
                    transaction_id = table.Column<string>(nullable: true),
                    openid = table.Column<string>(nullable: true),
                    Notify = table.Column<bool>(nullable: false),
                    PayStatus = table.Column<bool>(nullable: false),
                    Refund = table.Column<bool>(nullable: false),
                    NativeRequestMessage = table.Column<string>(nullable: true),
                    NativeResponseMessage = table.Column<string>(nullable: true),
                    NotifyResponseMessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodOrder", x => x.GoodOrderId);
                });

            migrationBuilder.CreateTable(
                name: "RandomImage",
                columns: table => new
                {
                    RandomImageId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeName = table.Column<string>(nullable: true),
                    TypeNameMD5 = table.Column<string>(nullable: true),
                    TypeNameSHA1 = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FileNameMD5 = table.Column<string>(nullable: true),
                    FileNameSHA1 = table.Column<string>(nullable: true),
                    FileSHA1 = table.Column<string>(nullable: true),
                    Sex = table.Column<bool>(nullable: false),
                    Block = table.Column<bool>(nullable: false),
                    Delete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RandomImage", x => x.RandomImageId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_UserId",
                table: "Authorization",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authorization");

            migrationBuilder.DropTable(
                name: "GoodOrder");

            migrationBuilder.DropTable(
                name: "RandomImage");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Allow",
                table: "BlogComment");

            migrationBuilder.DropColumn(
                name: "RemoteIpAddress",
                table: "AccessRecords");

            migrationBuilder.DropColumn(
                name: "RemotePort",
                table: "AccessRecords");

            migrationBuilder.DropColumn(
                name: "X_Forwarded_For",
                table: "AccessRecords");

            migrationBuilder.DropColumn(
                name: "X_Real_IP",
                table: "AccessRecords");

            migrationBuilder.AlterColumn<string>(
                name: "Cost",
                table: "User",
                type: "text",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<long>(
                name: "ArchiveId",
                table: "Post",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ArchiveMonthId",
                table: "Post",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Archive",
                columns: table => new
                {
                    ArchiveId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archive", x => x.ArchiveId);
                });

            migrationBuilder.CreateTable(
                name: "BlogUrls",
                columns: table => new
                {
                    BlogUrlsId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArchiveUrl = table.Column<string>(type: "text", nullable: true),
                    CateUrl = table.Column<string>(type: "text", nullable: true),
                    IndexUrl = table.Column<string>(type: "text", nullable: true),
                    PostUrl = table.Column<string>(type: "text", nullable: true),
                    SearchUrl = table.Column<string>(type: "text", nullable: true),
                    SingleUrl = table.Column<string>(type: "text", nullable: true),
                    TagPostsUrl = table.Column<string>(type: "text", nullable: true),
                    TagsUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogUrls", x => x.BlogUrlsId);
                });

            migrationBuilder.CreateTable(
                name: "UserAndBlog",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BlogLogo = table.Column<string>(type: "text", nullable: true),
                    BlogTitle = table.Column<string>(type: "text", nullable: true),
                    BlogUrl = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Logo = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAndBlog", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserAndBlogUrl",
                columns: table => new
                {
                    UserAndBlogUrlId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BlogUrl = table.Column<string>(type: "text", nullable: true),
                    PostUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAndBlogUrl", x => x.UserAndBlogUrlId);
                });

            migrationBuilder.CreateTable(
                name: "ArchiveMonth",
                columns: table => new
                {
                    ArchiveMonthId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArchiveId = table.Column<long>(type: "bigint", nullable: true),
                    Month = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Post_ArchiveId",
                table: "Post",
                column: "ArchiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_ArchiveMonthId",
                table: "Post",
                column: "ArchiveMonthId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveMonth_ArchiveId",
                table: "ArchiveMonth",
                column: "ArchiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Archive_ArchiveId",
                table: "Post",
                column: "ArchiveId",
                principalTable: "Archive",
                principalColumn: "ArchiveId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_ArchiveMonth_ArchiveMonthId",
                table: "Post",
                column: "ArchiveMonthId",
                principalTable: "ArchiveMonth",
                principalColumn: "ArchiveMonthId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
