using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class time20220318_03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "notes_repository_summary",
                table: "notes_repository",
                newName: "summary");

            migrationBuilder.RenameColumn(
                name: "notes_repository_owner_type",
                table: "notes_repository",
                newName: "owner_type");

            migrationBuilder.RenameColumn(
                name: "notes_repository_name",
                table: "notes_repository",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "notes_repository_license",
                table: "notes_repository",
                newName: "license");

            migrationBuilder.RenameColumn(
                name: "notes_repository_icon",
                table: "notes_repository",
                newName: "avatar");

            migrationBuilder.RenameColumn(
                name: "notes_repository_id",
                table: "notes_repository",
                newName: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "summary",
                table: "notes_repository",
                newName: "notes_repository_summary");

            migrationBuilder.RenameColumn(
                name: "owner_type",
                table: "notes_repository",
                newName: "notes_repository_owner_type");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "notes_repository",
                newName: "notes_repository_name");

            migrationBuilder.RenameColumn(
                name: "license",
                table: "notes_repository",
                newName: "notes_repository_license");

            migrationBuilder.RenameColumn(
                name: "avatar",
                table: "notes_repository",
                newName: "notes_repository_icon");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "notes_repository",
                newName: "notes_repository_id");
        }
    }
}
