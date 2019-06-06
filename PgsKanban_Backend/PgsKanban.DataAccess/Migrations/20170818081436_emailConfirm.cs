using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PgsKanban.DataAccess.Migrations
{
    public partial class emailConfirm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmationTokenExpirationTime",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationTokenExpirationTime",
                table: "AspNetUsers");
        }
    }
}
