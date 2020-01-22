using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MoreNote.Logic.Migrations
{
    public partial class FirstMigration18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppInfo");
        }
    }
}
