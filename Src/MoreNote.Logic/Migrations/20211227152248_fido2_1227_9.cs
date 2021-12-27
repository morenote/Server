using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class fido2_1227_9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fido2_repository");

            migrationBuilder.CreateTable(
                name: "fido2_item",
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
                    table.PrimaryKey("PK_fido2_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_fido2_item_user_info_user_id",
                        column: x => x.user_id,
                        principalTable: "user_info",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_fido2_item_id",
                table: "fido2_item",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fido2_item_user_id",
                table: "fido2_item",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fido2_item");

            migrationBuilder.CreateTable(
                name: "fido2_repository",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fido2_guid = table.Column<Guid>(type: "uuid", nullable: true),
                    fido2_cred_type = table.Column<string>(type: "text", nullable: true),
                    fido2_credential_id = table.Column<byte[]>(type: "bytea", nullable: true),
                    fido2_public_key = table.Column<byte[]>(type: "bytea", nullable: true),
                    fido2_reg_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    fido2_signature_counter = table.Column<long>(type: "bigint", nullable: false),
                    fido2_user_handle = table.Column<byte[]>(type: "bytea", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true)
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
                name: "IX_fido2_repository_id",
                table: "fido2_repository",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fido2_repository_user_id",
                table: "fido2_repository",
                column: "user_id");
        }
    }
}
