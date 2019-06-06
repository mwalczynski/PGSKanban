using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PgsKanban.DataAccess.Migrations
{
    public partial class BoardHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "Boards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
            migrationBuilder.Sql("UPDATE Boards SET Hash = REPLACE(NEWID(),'-','')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Boards");
        }
    }
}
