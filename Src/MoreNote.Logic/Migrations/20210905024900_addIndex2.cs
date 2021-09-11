using Microsoft.EntityFrameworkCore.Migrations;

namespace MoreNote.Logic.Migrations
{
    public partial class addIndex2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_user_info_email_verified_username_username_raw_user_role_th~",
                table: "user_info",
                columns: new[] { "email", "verified", "username", "username_raw", "user_role", "third_user_id", "from_user_id" });

            migrationBuilder.CreateIndex(
                name: "IX_access_records_ip_x_real_ip_x_forwarded_for_access_time_url",
                table: "access_records",
                columns: new[] { "ip", "x_real_ip", "x_forwarded_for", "access_time", "url" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_info_email_verified_username_username_raw_user_role_th~",
                table: "user_info");

            migrationBuilder.DropIndex(
                name: "IX_access_records_ip_x_real_ip_x_forwarded_for_access_time_url",
                table: "access_records");
        }
    }
}
