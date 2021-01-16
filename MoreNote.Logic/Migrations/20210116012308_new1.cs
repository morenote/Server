using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MoreNote.Logic.Migrations
{
    public partial class new1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteTag_Tag_TagUserId",
                table: "NoteTag");

            migrationBuilder.DropTable(
                name: "BlogItem");

            migrationBuilder.DropIndex(
                name: "IX_NoteTag_TagUserId",
                table: "NoteTag");

            migrationBuilder.DropColumn(
                name: "TagUserId",
                table: "NoteTag");

            migrationBuilder.AddColumn<string[]>(
                name: "Tags",
                table: "Tag",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Tag");

            migrationBuilder.AddColumn<long>(
                name: "TagUserId",
                table: "NoteTag",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlogItem",
                columns: table => new
                {
                    NoteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Abstract = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    HasMore = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogItem", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_BlogItem_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteTag_TagUserId",
                table: "NoteTag",
                column: "TagUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogItem_UserId",
                table: "BlogItem",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTag_Tag_TagUserId",
                table: "NoteTag",
                column: "TagUserId",
                principalTable: "Tag",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
