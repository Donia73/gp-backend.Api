using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_backend.EF.Migrations
{
    /// <inheritdoc />
    public partial class EditAppUserIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wounds_AspNetUsers_UserId",
                table: "Wounds");

            migrationBuilder.DropIndex(
                name: "IX_Wounds_UserId",
                table: "Wounds");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Wounds");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Wounds",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Wounds_ApplicationUserId",
                table: "Wounds",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wounds_AspNetUsers_ApplicationUserId",
                table: "Wounds",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wounds_AspNetUsers_ApplicationUserId",
                table: "Wounds");

            migrationBuilder.DropIndex(
                name: "IX_Wounds_ApplicationUserId",
                table: "Wounds");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Wounds",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Wounds",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Wounds_UserId",
                table: "Wounds",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wounds_AspNetUsers_UserId",
                table: "Wounds",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
