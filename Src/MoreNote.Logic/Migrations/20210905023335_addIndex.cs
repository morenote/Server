using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class addIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_note_content_note_id_user_id_is_history",
                table: "note_content",
                columns: new[] { "note_id", "user_id", "is_history" });

            migrationBuilder.CreateIndex(
                name: "IX_note_user_id_is_blog_is_deleted",
                table: "note",
                columns: new[] { "user_id", "is_blog", "is_deleted" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_note_content_note_id_user_id_is_history",
                table: "note_content");

            migrationBuilder.DropIndex(
                name: "IX_note_user_id_is_blog_is_deleted",
                table: "note");
        }
    }
}
