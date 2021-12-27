using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class fido2_1227_11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fido2_item_user_info_user_id",
                table: "fido2_item");

            migrationBuilder.DropIndex(
                name: "IX_fido2_item_user_id",
                table: "fido2_item");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "fido2_item");

            migrationBuilder.AddColumn<long>(
                name: "OwnerUserId",
                table: "fido2_item",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_fido2_item_OwnerUserId",
                table: "fido2_item",
                column: "OwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_fido2_item_user_info_OwnerUserId",
                table: "fido2_item",
                column: "OwnerUserId",
                principalTable: "user_info",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fido2_item_user_info_OwnerUserId",
                table: "fido2_item");

            migrationBuilder.DropIndex(
                name: "IX_fido2_item_OwnerUserId",
                table: "fido2_item");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "fido2_item");

            migrationBuilder.AddColumn<long>(
                name: "user_id",
                table: "fido2_item",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_fido2_item_user_id",
                table: "fido2_item",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_fido2_item_user_info_user_id",
                table: "fido2_item",
                column: "user_id",
                principalTable: "user_info",
                principalColumn: "user_id");
        }
    }
}
