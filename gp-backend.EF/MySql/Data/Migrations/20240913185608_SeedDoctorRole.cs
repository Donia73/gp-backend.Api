using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_backend.EF.MySql.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDoctorRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ad4c4ecd-1e91-4180-be16-45df72e44436", null, "Doc", "DOC" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b0f2babb-cfb4-4690-a107-3b025e372b30", "AQAAAAIAAYagAAAAEBMnZDaPA9sD6pN8sYyjVlDuKXmsUY9Q8Ytxz5u+rexNU4/RLxxU+av1BCZilMmuhA==", "35d3d6b8-b05e-4281-887d-a68fe1cf6936" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad4c4ecd-1e91-4180-be16-45df72e44436");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "966b42d3-93d5-4786-a56b-24b4fcd5d3da", "AQAAAAIAAYagAAAAEGRM+dqr7nVpVrrC8tBIEJez3T07mfBaW881b5eC0S/og/uwa5g4LL6E7qhdBI674g==", "4802716e-fc8e-4a2f-9828-a92f1eaf06c5" });
        }
    }
}
