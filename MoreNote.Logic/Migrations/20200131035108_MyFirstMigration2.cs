using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class MyFirstMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HostServiceProvider_ServiceProviderCompany_ServiceProviderC~",
                table: "HostServiceProvider");

            migrationBuilder.DropForeignKey(
                name: "FK_SecretReport_HostServiceProvider_HostServiceProviderId",
                table: "SecretReport");

            migrationBuilder.DropForeignKey(
                name: "FK_SecretReport_Reporter_ReporterId",
                table: "SecretReport");

            migrationBuilder.DropForeignKey(
                name: "FK_SecretReport_ServiceProviderCompany_ServiceProviderCompanyId",
                table: "SecretReport");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceProviderCompany_ServiceProviderLegalPerson_LegalPers~",
                table: "ServiceProviderCompany");

            migrationBuilder.DropIndex(
                name: "IX_ServiceProviderCompany_LegalPersonPersonId",
                table: "ServiceProviderCompany");

            migrationBuilder.DropIndex(
                name: "IX_SecretReport_HostServiceProviderId",
                table: "SecretReport");

            migrationBuilder.DropIndex(
                name: "IX_SecretReport_ReporterId",
                table: "SecretReport");

            migrationBuilder.DropIndex(
                name: "IX_SecretReport_ServiceProviderCompanyId",
                table: "SecretReport");

            migrationBuilder.DropIndex(
                name: "IX_HostServiceProvider_ServiceProviderCompanyId",
                table: "HostServiceProvider");

            migrationBuilder.DropColumn(
                name: "LegalPersonPersonId",
                table: "ServiceProviderCompany");

            migrationBuilder.DropColumn(
                name: "ServiceProviderCompanyId",
                table: "SecretReport");

            migrationBuilder.DropColumn(
                name: "ServiceProviderCompanyId",
                table: "HostServiceProvider");

            migrationBuilder.RenameColumn(
                name: "ReporterId",
                table: "SecretReport",
                newName: "reporterId");

            migrationBuilder.RenameColumn(
                name: "HostServiceProviderId",
                table: "SecretReport",
                newName: "hostServiceProviderId");

            migrationBuilder.AddColumn<long>(
                name: "LegalPersonId",
                table: "ServiceProviderCompany",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "reporterId",
                table: "SecretReport",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "hostServiceProviderId",
                table: "SecretReport",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "serviceProviderId",
                table: "SecretReport",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Reporter",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ServiceProviderId",
                table: "HostServiceProvider",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegalPersonId",
                table: "ServiceProviderCompany");

            migrationBuilder.DropColumn(
                name: "serviceProviderId",
                table: "SecretReport");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Reporter");

            migrationBuilder.DropColumn(
                name: "ServiceProviderId",
                table: "HostServiceProvider");

            migrationBuilder.RenameColumn(
                name: "reporterId",
                table: "SecretReport",
                newName: "ReporterId");

            migrationBuilder.RenameColumn(
                name: "hostServiceProviderId",
                table: "SecretReport",
                newName: "HostServiceProviderId");

            migrationBuilder.AddColumn<long>(
                name: "LegalPersonPersonId",
                table: "ServiceProviderCompany",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ReporterId",
                table: "SecretReport",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "HostServiceProviderId",
                table: "SecretReport",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "ServiceProviderCompanyId",
                table: "SecretReport",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ServiceProviderCompanyId",
                table: "HostServiceProvider",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceProviderCompany_LegalPersonPersonId",
                table: "ServiceProviderCompany",
                column: "LegalPersonPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_SecretReport_HostServiceProviderId",
                table: "SecretReport",
                column: "HostServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_SecretReport_ReporterId",
                table: "SecretReport",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_SecretReport_ServiceProviderCompanyId",
                table: "SecretReport",
                column: "ServiceProviderCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_HostServiceProvider_ServiceProviderCompanyId",
                table: "HostServiceProvider",
                column: "ServiceProviderCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_HostServiceProvider_ServiceProviderCompany_ServiceProviderC~",
                table: "HostServiceProvider",
                column: "ServiceProviderCompanyId",
                principalTable: "ServiceProviderCompany",
                principalColumn: "ServiceProviderCompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SecretReport_HostServiceProvider_HostServiceProviderId",
                table: "SecretReport",
                column: "HostServiceProviderId",
                principalTable: "HostServiceProvider",
                principalColumn: "HostServiceProviderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SecretReport_Reporter_ReporterId",
                table: "SecretReport",
                column: "ReporterId",
                principalTable: "Reporter",
                principalColumn: "ReporterId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SecretReport_ServiceProviderCompany_ServiceProviderCompanyId",
                table: "SecretReport",
                column: "ServiceProviderCompanyId",
                principalTable: "ServiceProviderCompany",
                principalColumn: "ServiceProviderCompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceProviderCompany_ServiceProviderLegalPerson_LegalPers~",
                table: "ServiceProviderCompany",
                column: "LegalPersonPersonId",
                principalTable: "ServiceProviderLegalPerson",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
