using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class GoogleAuthenticatorSecretKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "value",
                table: "Authorization",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Authorization",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "GoogleAuthenticatorSecretKey",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleAuthenticatorSecretKey",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Authorization",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Authorization",
                newName: "type");
        }
    }
}
