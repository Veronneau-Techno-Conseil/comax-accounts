using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseFramework.Migrations
{
    public partial class FK_AppNamespace_AppType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationTypeId",
                table: "AppNamespace",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppNamespace_ApplicationTypeId",
                table: "AppNamespace",
                column: "ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppNamespace_ApplicationTypes_ApplicationTypeId",
                table: "AppNamespace",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppNamespace_ApplicationTypes_ApplicationTypeId",
                table: "AppNamespace");

            migrationBuilder.DropIndex(
                name: "IX_AppNamespace_ApplicationTypeId",
                table: "AppNamespace");

            migrationBuilder.DropColumn(
                name: "ApplicationTypeId",
                table: "AppNamespace");
        }
    }
}
