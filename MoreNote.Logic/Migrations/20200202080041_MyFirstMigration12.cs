using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class MyFirstMigration12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "FromUserId",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FromUserId",
                table: "User",
                type: "text",
                nullable: true,
                oldClrType: typeof(long));
        }
    }
}
