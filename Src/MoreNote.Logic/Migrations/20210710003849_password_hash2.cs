using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class password_hash2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "memory_size",
                table: "user_info",
                newName: "password_memory_size");

            migrationBuilder.RenameColumn(
                name: "hash_iterations",
                table: "user_info",
                newName: "password_hash_iterations");

            migrationBuilder.RenameColumn(
                name: "hash_algorithm",
                table: "user_info",
                newName: "password_hash_algorithm");

            migrationBuilder.RenameColumn(
                name: "degree_of_parallelism",
                table: "user_info",
                newName: "password_degree_of_parallelism");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password_memory_size",
                table: "user_info",
                newName: "memory_size");

            migrationBuilder.RenameColumn(
                name: "password_hash_iterations",
                table: "user_info",
                newName: "hash_iterations");

            migrationBuilder.RenameColumn(
                name: "password_hash_algorithm",
                table: "user_info",
                newName: "hash_algorithm");

            migrationBuilder.RenameColumn(
                name: "password_degree_of_parallelism",
                table: "user_info",
                newName: "degree_of_parallelism");
        }
    }
}
