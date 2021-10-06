using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MoreNote.Logic.Migrations
{
    public partial class otherEditor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "page");

            migrationBuilder.AddColumn<string>(
                name: "markdown_option",
                table: "user_info",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rich_text_option",
                table: "user_info",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "markdown_option",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "rich_text_option",
                table: "user_info");

            migrationBuilder.CreateTable(
                name: "page",
                columns: table => new
                {
                    page_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    count = table.Column<int>(type: "integer", nullable: false),
                    cur_page = table.Column<int>(type: "integer", nullable: false),
                    per_page_size = table.Column<int>(type: "integer", nullable: false),
                    total_page = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_page", x => x.page_id);
                });
        }
    }
}
