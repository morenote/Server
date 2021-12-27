using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class fido2_1227_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fido2_repository_user_info_user_id",
                table: "fido2_repository");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "fido2_repository",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_fido2_repository_user_id",
                table: "fido2_repository",
                newName: "IX_fido2_repository_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_fido2_repository_user_info_UserId",
                table: "fido2_repository",
                column: "UserId",
                principalTable: "user_info",
                principalColumn: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fido2_repository_user_info_UserId",
                table: "fido2_repository");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "fido2_repository",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_fido2_repository_UserId",
                table: "fido2_repository",
                newName: "IX_fido2_repository_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_fido2_repository_user_info_user_id",
                table: "fido2_repository",
                column: "user_id",
                principalTable: "user_info",
                principalColumn: "user_id");
        }
    }
}
