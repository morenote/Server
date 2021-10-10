using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class addNoteFileIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_random_image_random_image_id_type_name_sex_is_delete_block_~",
                table: "random_image",
                columns: new[] { "random_image_id", "type_name", "sex", "is_delete", "block", "file_sha1" });

            migrationBuilder.CreateIndex(
                name: "IX_note_file_file_id_user_id_sha1",
                table: "note_file",
                columns: new[] { "file_id", "user_id", "sha1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_random_image_random_image_id_type_name_sex_is_delete_block_~",
                table: "random_image");

            migrationBuilder.DropIndex(
                name: "IX_note_file_file_id_user_id_sha1",
                table: "note_file");
        }
    }
}
