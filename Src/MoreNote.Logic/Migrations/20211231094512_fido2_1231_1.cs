using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class fido2_1231_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fido2_item_user_info_OwnerUserId",
                table: "fido2_item");

            migrationBuilder.DropForeignKey(
                name: "FK_fido2_item_user_info_UserId",
                table: "fido2_item");

            migrationBuilder.DropIndex(
                name: "IX_fido2_item_OwnerUserId",
                table: "fido2_item");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "fido2_item");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "fido2_item",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_fido2_item_UserId",
                table: "fido2_item",
                newName: "IX_fido2_item_user_id");

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "fido2_item",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_fido2_item_user_info_user_id",
                table: "fido2_item",
                column: "user_id",
                principalTable: "user_info",
                principalColumn: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fido2_item_user_info_user_id",
                table: "fido2_item");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "fido2_item",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_fido2_item_user_id",
                table: "fido2_item",
                newName: "IX_fido2_item_UserId");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "fido2_item",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_fido2_item_user_info_UserId",
                table: "fido2_item",
                column: "UserId",
                principalTable: "user_info",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
