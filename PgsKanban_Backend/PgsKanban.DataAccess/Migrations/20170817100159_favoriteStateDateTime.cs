using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PgsKanban.DataAccess.Migrations
{
    public partial class favoriteStateDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastTimeSetFavorite",
                table: "UserBoards",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastTimeSetFavorite",
                table: "UserBoards");
        }
    }
}
