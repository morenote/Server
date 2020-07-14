using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MoreNote.Logic.Migrations
{
    public partial class spam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpamDB",
                columns: table => new
                {
                    SpamId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Input = table.Column<string>(nullable: true),
                    Prediction = table.Column<bool>(nullable: false),
                    Score = table.Column<float>(nullable: false),
                    ManualCheck = table.Column<bool>(nullable: false),
                    ManualResult = table.Column<bool>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpamDB", x => x.SpamId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpamDB");
        }
    }
}
