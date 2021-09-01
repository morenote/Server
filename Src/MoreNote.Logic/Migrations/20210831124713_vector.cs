using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

namespace MoreNote.Logic.Migrations
{
    public partial class vector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "content_vector",
                table: "note_content",
                type: "tsvector",
                nullable: true);

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "title_vector",
                table: "note",
                type: "tsvector",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content_vector",
                table: "note_content");

            migrationBuilder.DropColumn(
                name: "title_vector",
                table: "note");
        }
    }
}
