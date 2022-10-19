using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseFramework.Migrations
{
    public partial class AddPictureToGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "GroupPicture",
                table: "Groups",
                type: "bytea",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupPicture",
                table: "Groups");
        }
    }
}
