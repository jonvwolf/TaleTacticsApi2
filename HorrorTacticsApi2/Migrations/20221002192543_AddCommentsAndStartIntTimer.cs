using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HorrorTacticsApi2.Migrations
{
    public partial class AddCommentsAndStartIntTimer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "StorySceneCommands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StartInternalTimer",
                table: "StorySceneCommands",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "StorySceneCommands");

            migrationBuilder.DropColumn(
                name: "StartInternalTimer",
                table: "StorySceneCommands");
        }
    }
}
