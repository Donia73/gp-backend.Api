using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_backend.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddIsReadColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "FeedBacks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "FeedBacks");
        }
    }
}
