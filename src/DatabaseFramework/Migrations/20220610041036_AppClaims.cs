using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DatabaseFramework.Migrations
{
    public partial class AppClaims : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppNamespace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppNamespace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppNamespaceId = table.Column<int>(type: "integer", nullable: false),
                    ClaimName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppClaim_AppNamespace_AppNamespaceId",
                        column: x => x.AppNamespaceId,
                        principalTable: "AppNamespace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppClaimAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppClaimId = table.Column<int>(type: "integer", nullable: false),
                    ApplicationId = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClaimAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppClaimAssignment_AppClaim_AppClaimId",
                        column: x => x.AppClaimId,
                        principalTable: "AppClaim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppClaimAssignment_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppClaim_AppNamespaceId",
                table: "AppClaim",
                column: "AppNamespaceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppClaimAssignment_AppClaimId",
                table: "AppClaimAssignment",
                column: "AppClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_AppClaimAssignment_ApplicationId",
                table: "AppClaimAssignment",
                column: "ApplicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppClaimAssignment");

            migrationBuilder.DropTable(
                name: "AppClaim");

            migrationBuilder.DropTable(
                name: "AppNamespace");
        }
    }
}
