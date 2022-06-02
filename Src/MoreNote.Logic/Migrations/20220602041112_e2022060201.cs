using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class e2022060201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "change_password_reminder",
                table: "user_info",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "password_expired",
                table: "user_info",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "change_password_reminder",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "password_expired",
                table: "user_info");
        }
    }
}
