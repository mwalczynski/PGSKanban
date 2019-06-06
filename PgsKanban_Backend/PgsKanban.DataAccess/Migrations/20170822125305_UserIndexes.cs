using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PgsKanban.DataAccess.Migrations
{
    public partial class UserIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "FirstNameIndex",
                table: "AspNetUsers",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "LastNameIndex",
                table: "AspNetUsers",
                column: "LastName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "FirstNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "LastNameIndex",
                table: "AspNetUsers");
        }
    }
}
