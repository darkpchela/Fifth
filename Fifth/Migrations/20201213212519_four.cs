using Microsoft.EntityFrameworkCore.Migrations;

namespace Fifth.Migrations
{
    public partial class four : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionTags_To_GameSessions",
                table: "SessionTags");

            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.CreateTable(
                name: "GameInfoDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    Started = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameInfoDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameSessions_To_Users",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameInfoDatas_CreatorId",
                table: "GameInfoDatas",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionTags_To_GameSessions",
                table: "SessionTags",
                column: "SessionId",
                principalTable: "GameInfoDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionTags_To_GameSessions",
                table: "SessionTags");

            migrationBuilder.DropTable(
                name: "GameInfoDatas");

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Started = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameSessions_To_Users",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_CreatorId",
                table: "GameSessions",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionTags_To_GameSessions",
                table: "SessionTags",
                column: "SessionId",
                principalTable: "GameSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
