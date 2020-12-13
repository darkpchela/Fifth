using Microsoft.EntityFrameworkCore.Migrations;

namespace Fifth.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectionString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    OpponentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Started = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameSessions_To_Users",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionTags",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_SessionTags_To_GameSessions",
                        column: x => x.SessionId,
                        principalTable: "GameSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_OwnerId",
                table: "GameSessions",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionTags_SessionId",
                table: "SessionTags",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "UQ_Text",
                table: "Tags",
                column: "Text",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Login",
                table: "Users",
                column: "Login",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionTags");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
