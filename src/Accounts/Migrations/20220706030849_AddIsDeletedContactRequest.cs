using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunAxiom.Accounts.Migrations
{
    public partial class AddIsDeletedContactRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ContactRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ContactRequests");
        }
    }
}
