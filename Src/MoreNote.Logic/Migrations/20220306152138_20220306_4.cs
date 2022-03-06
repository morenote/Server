using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class _20220306_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fido2_name",
                table: "fido2_item",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fido2_name",
                table: "fido2_item");
        }
    }
}
