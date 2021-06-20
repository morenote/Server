using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class changeTag2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NoteTagMap",
                table: "NoteTagMap");

            migrationBuilder.RenameTable(
                name: "NoteTagMap",
                newName: "note_tag_map");

            migrationBuilder.AddPrimaryKey(
                name: "PK_note_tag_map",
                table: "note_tag_map",
                column: "note_tag_map_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_note_tag_map",
                table: "note_tag_map");

            migrationBuilder.RenameTable(
                name: "note_tag_map",
                newName: "NoteTagMap");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NoteTagMap",
                table: "NoteTagMap",
                column: "note_tag_map_id");
        }
    }
}
