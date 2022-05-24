using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class _20220524_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "logging_login",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    login_datetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    login_method = table.Column<string>(type: "text", nullable: true),
                    is_login_success = table.Column<bool>(type: "boolean", nullable: false),
                    error_meesage = table.Column<string>(type: "text", nullable: true),
                    ip = table.Column<string>(type: "text", nullable: true),
                    browser_request_header = table.Column<string>(type: "text", nullable: true),
                    hmac = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logging_login", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_logging_login_id_user_id_login_datetime_login_method_is_log~",
                table: "logging_login",
                columns: new[] { "id", "user_id", "login_datetime", "login_method", "is_login_success", "ip" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logging_login");
        }
    }
}
