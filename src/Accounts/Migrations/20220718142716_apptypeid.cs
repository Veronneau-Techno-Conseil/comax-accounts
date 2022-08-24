using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunAxiom.Accounts.Migrations
{
    public partial class apptypeid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppClaimAssignment_OpenIddictApplications_ApplicationId",
                table: "AppClaimAssignment");

            migrationBuilder.DropIndex(
                name: "IX_AppClaimAssignment_ApplicationId",
                table: "AppClaimAssignment");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "AppClaimAssignment");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationTypeId",
                table: "AppClaimAssignment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AssignmentTags",
                table: "AppClaimAssignment",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppClaimAssignment_ApplicationTypeId",
                table: "AppClaimAssignment",
                column: "ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppClaimAssignment_ApplicationTypes_ApplicationTypeId",
                table: "AppClaimAssignment",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppClaimAssignment_ApplicationTypes_ApplicationTypeId",
                table: "AppClaimAssignment");

            migrationBuilder.DropIndex(
                name: "IX_AppClaimAssignment_ApplicationTypeId",
                table: "AppClaimAssignment");

            migrationBuilder.DropColumn(
                name: "ApplicationTypeId",
                table: "AppClaimAssignment");

            migrationBuilder.DropColumn(
                name: "AssignmentTags",
                table: "AppClaimAssignment");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationId",
                table: "AppClaimAssignment",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AppClaimAssignment_ApplicationId",
                table: "AppClaimAssignment",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppClaimAssignment_OpenIddictApplications_ApplicationId",
                table: "AppClaimAssignment",
                column: "ApplicationId",
                principalTable: "OpenIddictApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
