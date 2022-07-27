using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HorrorTacticsApi2.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Format = table.Column<string>(type: "text", nullable: false),
                    Filename = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    IsVirusScanned = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsBgm = table.Column<bool>(type: "boolean", nullable: false),
                    DurationSeconds = table.Column<long>(type: "bigint", nullable: false),
                    FileId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Width = table.Column<long>(type: "bigint", nullable: false),
                    Height = table.Column<long>(type: "bigint", nullable: false),
                    FileId = table.Column<long>(type: "bigint", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentStoryId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false)
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
                name: "StorySceneCommands",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentStorySceneId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Texts = table.Column<string>(type: "character varying(15000)", maxLength: 15000, nullable: false),
                    Timers = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Minigames = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorySceneCommands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorySceneCommands_StoryScenes_ParentStorySceneId",
                        column: x => x.ParentStorySceneId,
                        principalTable: "StoryScenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AudioEntityStorySceneCommandEntity",
                columns: table => new
                {
                    AudiosId = table.Column<long>(type: "bigint", nullable: false),
                    SceneCommandsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioEntityStorySceneCommandEntity", x => new { x.AudiosId, x.SceneCommandsId });
                    table.ForeignKey(
                        name: "FK_AudioEntityStorySceneCommandEntity_Audios_AudiosId",
                        column: x => x.AudiosId,
                        principalTable: "Audios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AudioEntityStorySceneCommandEntity_StorySceneCommands_Scene~",
                        column: x => x.SceneCommandsId,
                        principalTable: "StorySceneCommands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageEntityStorySceneCommandEntity",
                columns: table => new
                {
                    ImagesId = table.Column<long>(type: "bigint", nullable: false),
                    SceneCommandsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageEntityStorySceneCommandEntity", x => new { x.ImagesId, x.SceneCommandsId });
                    table.ForeignKey(
                        name: "FK_ImageEntityStorySceneCommandEntity_Images_ImagesId",
                        column: x => x.ImagesId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageEntityStorySceneCommandEntity_StorySceneCommands_Scene~",
                        column: x => x.SceneCommandsId,
                        principalTable: "StorySceneCommands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudioEntityStorySceneCommandEntity_SceneCommandsId",
                table: "AudioEntityStorySceneCommandEntity",
                column: "SceneCommandsId");

            migrationBuilder.CreateIndex(
                name: "IX_Audios_FileId",
                table: "Audios",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_Filename",
                table: "Files",
                column: "Filename");

            migrationBuilder.CreateIndex(
                name: "IX_ImageEntityStorySceneCommandEntity_SceneCommandsId",
                table: "ImageEntityStorySceneCommandEntity",
                column: "SceneCommandsId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_FileId",
                table: "Images",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_StorySceneCommands_ParentStorySceneId",
                table: "StorySceneCommands",
                column: "ParentStorySceneId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryScenes_ParentStoryId",
                table: "StoryScenes",
                column: "ParentStoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioEntityStorySceneCommandEntity");

            migrationBuilder.DropTable(
                name: "ImageEntityStorySceneCommandEntity");

            migrationBuilder.DropTable(
                name: "Audios");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "StorySceneCommands");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "StoryScenes");

            migrationBuilder.DropTable(
                name: "Stories");
        }
    }
}
