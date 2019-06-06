using Microsoft.EntityFrameworkCore.Migrations;

namespace PgsKanban.DataAccess.Migrations
{
    public partial class boardPriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "UserBoards",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "UserBoards");
        }
    }
}
