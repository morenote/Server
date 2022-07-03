using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class userinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "user_info",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "user_info",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "user_info",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "geographic",
                table: "user_info",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "user_info",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "profile",
                table: "user_info",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "address",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "country",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "geographic",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "profile",
                table: "user_info");
        }
    }
}
