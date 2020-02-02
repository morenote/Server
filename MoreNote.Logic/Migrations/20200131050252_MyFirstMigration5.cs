using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class MyFirstMigration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceProviderId",
                table: "HostServiceProvider");

            migrationBuilder.AddColumn<long>(
                name: "ServiceProviderCompanyId",
                table: "HostServiceProvider",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceProviderCompanyId",
                table: "HostServiceProvider");

            migrationBuilder.AddColumn<long>(
                name: "ServiceProviderId",
                table: "HostServiceProvider",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
