using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class MyFirstMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "serviceProviderId",
                table: "SecretReport");

            migrationBuilder.AddColumn<long>(
                name: "serviceProviderCompanyId",
                table: "SecretReport",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "serviceProviderCompanyId",
                table: "SecretReport");

            migrationBuilder.AddColumn<long>(
                name: "serviceProviderId",
                table: "SecretReport",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
