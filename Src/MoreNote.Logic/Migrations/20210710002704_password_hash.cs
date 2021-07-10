using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class password_hash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pwd_cost",
                table: "user_info",
                newName: "memory_size");

            migrationBuilder.AddColumn<int>(
                name: "degree_of_parallelism",
                table: "user_info",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "hash_iterations",
                table: "user_info",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "degree_of_parallelism",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "hash_iterations",
                table: "user_info");

            migrationBuilder.RenameColumn(
                name: "memory_size",
                table: "user_info",
                newName: "pwd_cost");
        }
    }
}
