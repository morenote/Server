using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    /// <inheritdoc />
    public partial class client : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "client_password_degree_of_parallelism",
                table: "user_info",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "client_password_hash_algorithm",
                table: "user_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "client_password_hash_iterations",
                table: "user_info",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "client_password_memory_size",
                table: "user_info",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "client_salt",
                table: "user_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "enc_algorithms",
                table: "repository",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "enc_key",
                table: "repository",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pbkdf2_salt",
                table: "repository",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "enc_algorithms",
                table: "note_content",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "enc_key",
                table: "note_content",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pbkdf2_salt",
                table: "note_content",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "client_password_degree_of_parallelism",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "client_password_hash_algorithm",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "client_password_hash_iterations",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "client_password_memory_size",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "client_salt",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "enc_algorithms",
                table: "repository");

            migrationBuilder.DropColumn(
                name: "enc_key",
                table: "repository");

            migrationBuilder.DropColumn(
                name: "pbkdf2_salt",
                table: "repository");

            migrationBuilder.DropColumn(
                name: "enc_algorithms",
                table: "note_content");

            migrationBuilder.DropColumn(
                name: "enc_key",
                table: "note_content");

            migrationBuilder.DropColumn(
                name: "pbkdf2_salt",
                table: "note_content");
        }
    }
}
