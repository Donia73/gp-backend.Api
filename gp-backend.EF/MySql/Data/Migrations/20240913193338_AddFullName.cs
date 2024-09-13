using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_backend.EF.MySql.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "FullName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1c4671fa-dc53-4f85-aa41-6621f3d3bf12", null, "AQAAAAIAAYagAAAAEILx+U+qY+a3lmHaxxweArrs8mDSIme7LVDSVAk/FJBNMJsSV8MgysVIzycsrC2jqg==", "d9b65ee0-abae-4096-bd82-1b52218bad23" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b0f2babb-cfb4-4690-a107-3b025e372b30", "AQAAAAIAAYagAAAAEBMnZDaPA9sD6pN8sYyjVlDuKXmsUY9Q8Ytxz5u+rexNU4/RLxxU+av1BCZilMmuhA==", "35d3d6b8-b05e-4281-887d-a68fe1cf6936" });
        }
    }
}
