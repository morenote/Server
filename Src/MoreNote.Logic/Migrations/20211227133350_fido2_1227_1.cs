using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class fido2_1227_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fido2_cred_type",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "fido2_credential_id",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "fido2_guid",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "fido2_public_key",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "fido2_reg_date",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "fido2_signature_counter",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "fido2_user_handle",
                table: "user_info");

            migrationBuilder.CreateTable(
                name: "fido2_repository",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    fido2_credential_id = table.Column<byte[]>(type: "bytea", nullable: true),
                    fido2_public_key = table.Column<byte[]>(type: "bytea", nullable: true),
                    fido2_user_handle = table.Column<byte[]>(type: "bytea", nullable: true),
                    fido2_signature_counter = table.Column<long>(type: "bigint", nullable: false),
                    fido2_cred_type = table.Column<string>(type: "text", nullable: true),
                    fido2_reg_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fido2_guid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fido2_repository", x => x.id);
                    table.ForeignKey(
                        name: "FK_fido2_repository_user_info_user_id",
                        column: x => x.user_id,
                        principalTable: "user_info",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_fido2_repository_id_fido2_credential_id_fido2_user_handle_f~",
                table: "fido2_repository",
                columns: new[] { "id", "fido2_credential_id", "fido2_user_handle", "fido2_reg_date", "fido2_guid" });

            migrationBuilder.CreateIndex(
                name: "IX_fido2_repository_user_id",
                table: "fido2_repository",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fido2_repository");

            migrationBuilder.AddColumn<string>(
                name: "fido2_cred_type",
                table: "user_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fido2_credential_id",
                table: "user_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fido2_guid",
                table: "user_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fido2_public_key",
                table: "user_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "fido2_reg_date",
                table: "user_info",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "fido2_signature_counter",
                table: "user_info",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "fido2_user_handle",
                table: "user_info",
                type: "text",
                nullable: true);
        }
    }
}
