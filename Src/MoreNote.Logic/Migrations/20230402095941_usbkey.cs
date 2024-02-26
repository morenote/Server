using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoreNote.Logic.Migrations
{
	/// <inheritdoc />
	public partial class usbkey : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "usbkey_binding",
				columns: table => new
				{
					id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					userid = table.Column<long>(name: "user_id", type: "bigint", nullable: true),
					modulus = table.Column<string>(type: "text", nullable: false),
					exponent = table.Column<string>(type: "text", nullable: false),
					hash = table.Column<string>(type: "text", nullable: false),
					hmac = table.Column<string>(type: "text", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_usbkey_binding", x => x.id);
				});

			migrationBuilder.CreateIndex(
				name: "IX_usbkey_binding_user_id_modulus",
				table: "usbkey_binding",
				columns: new[] { "user_id", "modulus" });
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "usbkey_binding");
		}
	}
}
