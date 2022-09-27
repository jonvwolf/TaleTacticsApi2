using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HorrorTacticsApi2.Migrations
{
    public partial class FixIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_Filename",
                table: "Files");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_Filename",
                table: "Files",
                column: "Filename",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Files_Filename",
                table: "Files");

            migrationBuilder.CreateIndex(
                name: "IX_Files_Filename",
                table: "Files",
                column: "Filename");
        }
    }
}
