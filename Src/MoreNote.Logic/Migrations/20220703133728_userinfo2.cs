using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class userinfo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "geographic",
                table: "user_info",
                newName: "title");

            migrationBuilder.AddColumn<string>(
                name: "geographic_city",
                table: "user_info",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "geographic_province",
                table: "user_info",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "group",
                table: "user_info",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "signature",
                table: "user_info",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "tags",
                table: "user_info",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "geographic_city",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "geographic_province",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "group",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "signature",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "tags",
                table: "user_info");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "user_info",
                newName: "geographic");
        }
    }
}
