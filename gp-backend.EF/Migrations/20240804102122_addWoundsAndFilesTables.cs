using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_backend.EF.Migrations
{
    /// <inheritdoc />
    public partial class addWoundsAndFilesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentDisposition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FileDescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileContent_Images_FileDescriptionId",
                        column: x => x.FileDescriptionId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiseaseId = table.Column<int>(type: "int", nullable: true),
                    ImageId = table.Column<int>(type: "int", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Advice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wounds_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wounds_Diseases_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Diseases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Wounds_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileContent_FileDescriptionId",
                table: "FileContent",
                column: "FileDescriptionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wounds_DiseaseId",
                table: "Wounds",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Wounds_ImageId",
                table: "Wounds",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Wounds_UserId",
                table: "Wounds",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileContent");

            migrationBuilder.DropTable(
                name: "Wounds");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
