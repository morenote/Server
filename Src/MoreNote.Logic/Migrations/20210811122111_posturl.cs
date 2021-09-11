using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class posturl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostUrl",
                table: "user_info",
                newName: "post_url");

            migrationBuilder.RenameColumn(
                name: "BlogUrl",
                table: "user_info",
                newName: "blog_url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "post_url",
                table: "user_info",
                newName: "PostUrl");

            migrationBuilder.RenameColumn(
                name: "blog_url",
                table: "user_info",
                newName: "BlogUrl");
        }
    }
}
