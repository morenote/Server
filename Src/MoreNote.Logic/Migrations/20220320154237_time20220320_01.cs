using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class time20220320_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "role_id",
                table: "organization_team",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "role_id",
                table: "organization_member",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "organization_member_role",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organization_id = table.Column<long>(type: "bigint", nullable: true),
                    role_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization_member_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organization_member_role_mapping",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<long>(type: "bigint", nullable: true),
                    authority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization_member_role_mapping", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "organization_member_role");

            migrationBuilder.DropTable(
                name: "organization_member_role_mapping");

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "organization_team");

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "organization_member");
        }
    }
}
