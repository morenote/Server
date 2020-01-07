using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Migrations
{
    public partial class _11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "UserGroup",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGroup",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "User",
                type: "longtext",
                nullable: true);
        }
    }
}
