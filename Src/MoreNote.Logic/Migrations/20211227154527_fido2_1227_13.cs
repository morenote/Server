using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class fido2_1227_13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "fido2_item",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_fido2_item_UserId",
                table: "fido2_item",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_fido2_item_user_info_UserId",
                table: "fido2_item",
                column: "UserId",
                principalTable: "user_info",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fido2_item_user_info_UserId",
                table: "fido2_item");

            migrationBuilder.DropIndex(
                name: "IX_fido2_item_UserId",
                table: "fido2_item");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "fido2_item");
        }
    }
}
