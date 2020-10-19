using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MoreNote.Logic.Migrations
{
    public partial class ResolutionStrategy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResolutionLocation",
                columns: table => new
                {
                    ResolutionLocationID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StrategyID = table.Column<long>(nullable: false),
                    URL = table.Column<string>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    Speed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionLocation", x => x.ResolutionLocationID);
                });

            migrationBuilder.CreateTable(
                name: "ResolutionStrategy",
                columns: table => new
                {
                    StrategyID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StrategyKey = table.Column<string>(nullable: true),
                    StrategyName = table.Column<string>(nullable: true),
                    CheckTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolutionStrategy", x => x.StrategyID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResolutionLocation");

            migrationBuilder.DropTable(
                name: "ResolutionStrategy");
        }
    }
}
