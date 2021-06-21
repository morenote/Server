using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class TagMap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsBlog",
                table: "note_tag",
                newName: "is_blog");

            migrationBuilder.AddColumn<bool>(
                name: "is_blog",
                table: "note_tag_map",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_blog",
                table: "note_tag_map");

            migrationBuilder.RenameColumn(
                name: "is_blog",
                table: "note_tag",
                newName: "IsBlog");
        }
    }
}
