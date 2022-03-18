using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class time20220318_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "fork_counter",
                table: "notes_repository",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "notes_repository_icon",
                table: "notes_repository",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "star_counter",
                table: "notes_repository",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fork_counter",
                table: "notes_repository");

            migrationBuilder.DropColumn(
                name: "notes_repository_icon",
                table: "notes_repository");

            migrationBuilder.DropColumn(
                name: "star_counter",
                table: "notes_repository");
        }
    }
}
