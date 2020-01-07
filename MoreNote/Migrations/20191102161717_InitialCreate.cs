using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    ArticleId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    CreatUser = table.Column<string>(nullable: true),
                    CreatTime = table.Column<DateTime>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    ArticleType = table.Column<string>(nullable: true),
                    Sources = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SEOTitle = table.Column<string>(nullable: true),
                    SEOKeyWord = table.Column<string>(nullable: true),
                    SEODescription = table.Column<string>(nullable: true),
                    AllowComments = table.Column<bool>(nullable: false),
                    Topping = table.Column<bool>(nullable: false),
                    Recommend = table.Column<bool>(nullable: false),
                    Hot = table.Column<bool>(nullable: false),
                    TurnMap = table.Column<bool>(nullable: false),
                    RecType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.ArticleId);
                });

            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    ChapterId = table.Column<string>(nullable: false),
                    ArticleId = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    SerialNumber = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    CreatTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => x.ChapterId);
                });

            migrationBuilder.CreateTable(
                name: "Pay",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Chapter = table.Column<string>(nullable: true),
                    PayTime = table.Column<DateTime>(nullable: false),
                    Money = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Userid = table.Column<string>(nullable: false),
                    PassWord = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Group = table.Column<string>(nullable: true),
                    Intro = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    QQ = table.Column<string>(nullable: true),
                    Twitter = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Rank = table.Column<int>(nullable: false),
                    Credit = table.Column<int>(nullable: false),
                    Gb = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Userid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.DropTable(
                name: "Pay");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
