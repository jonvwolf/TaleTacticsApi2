using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HorrorTacticsApi2.Migrations
{
    public partial class Initialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Format = table.Column<string>(type: "TEXT", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SizeInBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    IsVirusScanned = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

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
                name: "Audios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsBgm = table.Column<bool>(type: "INTEGER", nullable: false),
                    DurationSeconds = table.Column<uint>(type: "INTEGER", nullable: false),
                    FileId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Audios_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Width = table.Column<uint>(type: "INTEGER", nullable: false),
                    Height = table.Column<uint>(type: "INTEGER", nullable: false),
                    FileId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "AudioEntityStorySceneEntity",
                columns: table => new
                {
                    AudiosId = table.Column<long>(type: "INTEGER", nullable: false),
                    ScenesId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioEntityStorySceneEntity", x => new { x.AudiosId, x.ScenesId });
                    table.ForeignKey(
                        name: "FK_AudioEntityStorySceneEntity_Audios_AudiosId",
                        column: x => x.AudiosId,
                        principalTable: "Audios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AudioEntityStorySceneEntity_StoryScenes_ScenesId",
                        column: x => x.ScenesId,
                        principalTable: "StoryScenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageEntityStorySceneEntity",
                columns: table => new
                {
                    ImagesId = table.Column<long>(type: "INTEGER", nullable: false),
                    ScenesId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageEntityStorySceneEntity", x => new { x.ImagesId, x.ScenesId });
                    table.ForeignKey(
                        name: "FK_ImageEntityStorySceneEntity_Images_ImagesId",
                        column: x => x.ImagesId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageEntityStorySceneEntity_StoryScenes_ScenesId",
                        column: x => x.ScenesId,
                        principalTable: "StoryScenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudioEntityStorySceneEntity_ScenesId",
                table: "AudioEntityStorySceneEntity",
                column: "ScenesId");

            migrationBuilder.CreateIndex(
                name: "IX_Audios_FileId",
                table: "Audios",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageEntityStorySceneEntity_ScenesId",
                table: "ImageEntityStorySceneEntity",
                column: "ScenesId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_FileId",
                table: "Images",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryScenes_ParentStoryId",
                table: "StoryScenes",
                column: "ParentStoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioEntityStorySceneEntity");

            migrationBuilder.DropTable(
                name: "ImageEntityStorySceneEntity");

            migrationBuilder.DropTable(
                name: "Audios");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "StoryScenes");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Stories");
        }
    }
}
