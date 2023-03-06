using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseFramework.Migrations
{
    public partial class appversioning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SortValue",
                table: "AppVersionTags",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Sensitive",
                table: "AppVersionConfigurations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ValueGenParameter",
                table: "AppVersionConfigurations",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortValue",
                table: "AppVersionTags");

            migrationBuilder.DropColumn(
                name: "Sensitive",
                table: "AppVersionConfigurations");

            migrationBuilder.DropColumn(
                name: "ValueGenParameter",
                table: "AppVersionConfigurations");
        }
    }
}
