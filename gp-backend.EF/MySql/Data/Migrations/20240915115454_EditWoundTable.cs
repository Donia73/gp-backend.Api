using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_backend.EF.MySql.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditWoundTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wounds_Diseases_DiseaseId",
                table: "Wounds");

            migrationBuilder.DropIndex(
                name: "IX_Wounds_DiseaseId",
                table: "Wounds");

            migrationBuilder.DropColumn(
                name: "DiseaseId",
                table: "Wounds");

            migrationBuilder.AddColumn<int>(
                name: "WoundId",
                table: "Diseases",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a0270b3d-15be-4846-aa87-b2126488b0a9", "AQAAAAIAAYagAAAAEB8hJ2bBccU3iidiBUvecj/aeZNHnQ5Q/y2cjsn2dMyFuOZJ2O5Ux9dBWFiaxDsgtw==", "a7f54610-b943-45e7-b616-7598180f4fb1" });

            migrationBuilder.CreateIndex(
                name: "IX_Diseases_WoundId",
                table: "Diseases",
                column: "WoundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diseases_Wounds_WoundId",
                table: "Diseases",
                column: "WoundId",
                principalTable: "Wounds",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diseases_Wounds_WoundId",
                table: "Diseases");

            migrationBuilder.DropIndex(
                name: "IX_Diseases_WoundId",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "WoundId",
                table: "Diseases");

            migrationBuilder.AddColumn<int>(
                name: "DiseaseId",
                table: "Wounds",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1c4671fa-dc53-4f85-aa41-6621f3d3bf12", "AQAAAAIAAYagAAAAEILx+U+qY+a3lmHaxxweArrs8mDSIme7LVDSVAk/FJBNMJsSV8MgysVIzycsrC2jqg==", "d9b65ee0-abae-4096-bd82-1b52218bad23" });

            migrationBuilder.CreateIndex(
                name: "IX_Wounds_DiseaseId",
                table: "Wounds",
                column: "DiseaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wounds_Diseases_DiseaseId",
                table: "Wounds",
                column: "DiseaseId",
                principalTable: "Diseases",
                principalColumn: "Id");
        }
    }
}
