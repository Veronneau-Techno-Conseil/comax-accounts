using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseFramework.Migrations
{
    public partial class app_fromsecret : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FromSecret",
                table: "AppConfigurations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromSecret",
                table: "AppConfigurations");
        }
    }
}
