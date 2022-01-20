using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HorrorTacticsApi2.Migrations
{
    public partial class AddStoryAndStoryScenes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StorySceneEntityId",
                table: "Images",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StorySceneEntityId",
                table: "Audios",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 600, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoryScenes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentStoryId = table.Column<long>(type: "INTEGER", nullable: false),
                    Texts = table.Column<string>(type: "TEXT", maxLength: 15000, nullable: false),
                    Timers = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryScenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryScenes_Stories_ParentStoryId",
                        column: x => x.ParentStoryId,
                        principalTable: "Stories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_StorySceneEntityId",
                table: "Images",
                column: "StorySceneEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Audios_StorySceneEntityId",
                table: "Audios",
                column: "StorySceneEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryScenes_ParentStoryId",
                table: "StoryScenes",
                column: "ParentStoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audios_StoryScenes_StorySceneEntityId",
                table: "Audios",
                column: "StorySceneEntityId",
                principalTable: "StoryScenes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_StoryScenes_StorySceneEntityId",
                table: "Images",
                column: "StorySceneEntityId",
                principalTable: "StoryScenes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audios_StoryScenes_StorySceneEntityId",
                table: "Audios");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_StoryScenes_StorySceneEntityId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "StoryScenes");

            migrationBuilder.DropTable(
                name: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_Images_StorySceneEntityId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Audios_StorySceneEntityId",
                table: "Audios");

            migrationBuilder.DropColumn(
                name: "StorySceneEntityId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "StorySceneEntityId",
                table: "Audios");
        }
    }
}
