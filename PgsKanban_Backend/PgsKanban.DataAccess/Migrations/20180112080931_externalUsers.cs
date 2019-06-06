using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PgsKanban.DataAccess.Migrations
{
    public partial class externalUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ExternalUserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ExternalUserId",
                table: "Comments",
                column: "ExternalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ExternalUsers_ExternalUserId",
                table: "Comments",
                column: "ExternalUserId",
                principalTable: "ExternalUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ExternalUsers_ExternalUserId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "ExternalUsers");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ExternalUserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ExternalUserId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
