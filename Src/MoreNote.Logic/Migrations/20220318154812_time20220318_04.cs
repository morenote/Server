using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class time20220318_04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "summary",
                table: "notes_repository",
                newName: "description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "description",
                table: "notes_repository",
                newName: "summary");
        }
    }
}
