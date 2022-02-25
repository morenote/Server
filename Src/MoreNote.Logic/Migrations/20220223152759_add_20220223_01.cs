using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoreNote.Logic.Migrations
{
    public partial class add_20220223_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "notes_repository_id",
                table: "notebook",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "notes_repository_id",
                table: "note_file",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "notes_repository_id",
                table: "note",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "notes_repository",
                columns: table => new
                {
                    notes_repository_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    notes_repository_name = table.Column<long>(type: "bigint", nullable: true),
                    notes_repository_summary = table.Column<long>(type: "bigint", nullable: true),
                    notes_repository_license = table.Column<long>(type: "bigint", nullable: true),
                    notes_repository_type = table.Column<int>(type: "integer", nullable: false),
                    owner_id = table.Column<long>(type: "bigint", nullable: true),
                    visible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notes_repository", x => x.notes_repository_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notes_repository");

            migrationBuilder.DropColumn(
                name: "notes_repository_id",
                table: "notebook");

            migrationBuilder.DropColumn(
                name: "notes_repository_id",
                table: "note_file");

            migrationBuilder.DropColumn(
                name: "notes_repository_id",
                table: "note");
        }
    }
}
