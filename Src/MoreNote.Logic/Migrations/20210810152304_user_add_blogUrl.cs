using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class user_add_blogUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlogUrl",
                table: "user_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostUrl",
                table: "user_info",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogUrl",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "PostUrl",
                table: "user_info");
        }
    }
}
