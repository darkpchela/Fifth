using Microsoft.EntityFrameworkCore.Migrations;

namespace Fifth.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionString",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "OpponentId",
                table: "GameSessions");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "GameSessions",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_GameSessions_OwnerId",
                table: "GameSessions",
                newName: "IX_GameSessions_CreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "GameSessions",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_GameSessions_CreatorId",
                table: "GameSessions",
                newName: "IX_GameSessions_OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionString",
                table: "GameSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OpponentId",
                table: "GameSessions",
                type: "int",
                nullable: true);
        }
    }
}
