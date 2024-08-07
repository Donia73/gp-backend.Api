using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_backend.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddRiskColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Risk",
                table: "Diseases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Risk",
                table: "Diseases");
        }
    }
}
