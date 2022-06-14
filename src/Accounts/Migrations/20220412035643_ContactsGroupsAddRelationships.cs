using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommunAxiom.Accounts.Migrations
{
    public partial class ContactsGroupsAddRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberRole_Roles_RoleId",
                table: "GroupMemberRole");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_ContactType_ContactTypeId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "ContactType");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.RenameColumn(
                name: "ContactTypeId",
                table: "Notifications",
                newName: "ContactMethodTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_ContactTypeId",
                table: "Notifications",
                newName: "IX_Notifications_ContactMethodTypeId");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "GroupMemberRole",
                newName: "GroupRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMemberRole_RoleId",
                table: "GroupMemberRole",
                newName: "IX_GroupMemberRole_GroupRoleId");

            migrationBuilder.CreateTable(
                name: "ContactMethodType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMethodType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRole", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberRole_GroupRole_GroupRoleId",
                table: "GroupMemberRole",
                column: "GroupRoleId",
                principalTable: "GroupRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_ContactMethodType_ContactMethodTypeId",
                table: "Notifications",
                column: "ContactMethodTypeId",
                principalTable: "ContactMethodType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberRole_GroupRole_GroupRoleId",
                table: "GroupMemberRole");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_ContactMethodType_ContactMethodTypeId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "ContactMethodType");

            migrationBuilder.DropTable(
                name: "GroupRole");

            migrationBuilder.RenameColumn(
                name: "ContactMethodTypeId",
                table: "Notifications",
                newName: "ContactTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_ContactMethodTypeId",
                table: "Notifications",
                newName: "IX_Notifications_ContactTypeId");

            migrationBuilder.RenameColumn(
                name: "GroupRoleId",
                table: "GroupMemberRole",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMemberRole_GroupRoleId",
                table: "GroupMemberRole",
                newName: "IX_GroupMemberRole_RoleId");

            migrationBuilder.CreateTable(
                name: "ContactType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberRole_Roles_RoleId",
                table: "GroupMemberRole",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_ContactType_ContactTypeId",
                table: "Notifications",
                column: "ContactTypeId",
                principalTable: "ContactType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
