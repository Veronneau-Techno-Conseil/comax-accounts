using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DatabaseFramework.Migrations
{
    public partial class ecosystems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppVersionTagId",
                table: "OpenIddictApplications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContainerImage",
                table: "ApplicationTypes",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppVersionTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationTypeId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeprecationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVersionTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppVersionTags_ApplicationTypes_ApplicationTypeId",
                        column: x => x.ApplicationTypeId,
                        principalTable: "ApplicationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ecosystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ecosystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppVersionConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppVersionTagId = table.Column<int>(type: "integer", nullable: false),
                    ConfigurationKey = table.Column<string>(type: "text", nullable: false),
                    DefaultValue = table.Column<string>(type: "text", nullable: false),
                    ValueGenerator = table.Column<string>(type: "text", nullable: true),
                    UserValueMandatory = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVersionConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppVersionConfigurations_AppVersionTags_AppVersionTagId",
                        column: x => x.AppVersionTagId,
                        principalTable: "AppVersionTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcosystemApplications",
                columns: table => new
                {
                    ApplicationTypeId = table.Column<int>(type: "integer", nullable: false),
                    EcosystemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcosystemApplications", x => new { x.ApplicationTypeId, x.EcosystemId });
                    table.ForeignKey(
                        name: "FK_EcosystemApplications_ApplicationTypes_ApplicationTypeId",
                        column: x => x.ApplicationTypeId,
                        principalTable: "ApplicationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EcosystemApplications_Ecosystems_EcosystemId",
                        column: x => x.EcosystemId,
                        principalTable: "Ecosystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcosystemVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PreviousVersionId = table.Column<int>(type: "integer", nullable: true),
                    Current = table.Column<bool>(type: "boolean", nullable: false),
                    VersionName = table.Column<string>(type: "text", nullable: false),
                    EcosystemId = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeploymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeprecationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcosystemVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EcosystemVersions_Ecosystems_EcosystemId",
                        column: x => x.EcosystemId,
                        principalTable: "Ecosystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EcosystemVersions_EcosystemVersions_PreviousVersionId",
                        column: x => x.PreviousVersionId,
                        principalTable: "EcosystemVersions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<string>(type: "text", nullable: false),
                    AppVersionConfigurationId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppConfigurations_AppVersionConfigurations_AppVersionConfig~",
                        column: x => x.AppVersionConfigurationId,
                        principalTable: "AppVersionConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppConfigurations_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EcosystemVersionTags",
                columns: table => new
                {
                    EcosystemVersionId = table.Column<int>(type: "integer", nullable: false),
                    AppVersionTagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EcosystemVersionTags", x => new { x.EcosystemVersionId, x.AppVersionTagId });
                    table.ForeignKey(
                        name: "FK_EcosystemVersionTags_AppVersionTags_AppVersionTagId",
                        column: x => x.AppVersionTagId,
                        principalTable: "AppVersionTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EcosystemVersionTags_EcosystemVersions_EcosystemVersionId",
                        column: x => x.EcosystemVersionId,
                        principalTable: "EcosystemVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictApplications_AppVersionTagId",
                table: "OpenIddictApplications",
                column: "AppVersionTagId");

            migrationBuilder.CreateIndex(
                name: "IX_AppConfigurations_ApplicationId",
                table: "AppConfigurations",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppConfigurations_AppVersionConfigurationId",
                table: "AppConfigurations",
                column: "AppVersionConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVersionConfigurations_AppVersionTagId",
                table: "AppVersionConfigurations",
                column: "AppVersionTagId");

            migrationBuilder.CreateIndex(
                name: "IX_AppVersionTags_ApplicationTypeId_Name",
                table: "AppVersionTags",
                columns: new[] { "ApplicationTypeId", "Name" },
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "CreationDate", "DeprecationDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EcosystemApplications_EcosystemId",
                table: "EcosystemApplications",
                column: "EcosystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Ecosystems_Name",
                table: "Ecosystems",
                column: "Name",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "Description" });

            migrationBuilder.CreateIndex(
                name: "IX_EcosystemVersions_EcosystemId_PreviousVersionId",
                table: "EcosystemVersions",
                columns: new[] { "EcosystemId", "PreviousVersionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EcosystemVersions_EcosystemId_VersionName",
                table: "EcosystemVersions",
                columns: new[] { "EcosystemId", "VersionName" })
                .Annotation("Npgsql:IndexInclude", new[] { "Id", "PreviousVersionId", "Current", "CreationDate", "DeploymentDate", "DeprecationDate" })
                .Annotation("Npgsql:IndexSortOrder", new[] { SortOrder.Ascending, SortOrder.Descending });

            migrationBuilder.CreateIndex(
                name: "IX_EcosystemVersions_PreviousVersionId",
                table: "EcosystemVersions",
                column: "PreviousVersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EcosystemVersionTags_AppVersionTagId",
                table: "EcosystemVersionTags",
                column: "AppVersionTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenIddictApplications_AppVersionTags_AppVersionTagId",
                table: "OpenIddictApplications",
                column: "AppVersionTagId",
                principalTable: "AppVersionTags",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenIddictApplications_AppVersionTags_AppVersionTagId",
                table: "OpenIddictApplications");

            migrationBuilder.DropTable(
                name: "AppConfigurations");

            migrationBuilder.DropTable(
                name: "EcosystemApplications");

            migrationBuilder.DropTable(
                name: "EcosystemVersionTags");

            migrationBuilder.DropTable(
                name: "AppVersionConfigurations");

            migrationBuilder.DropTable(
                name: "EcosystemVersions");

            migrationBuilder.DropTable(
                name: "AppVersionTags");

            migrationBuilder.DropTable(
                name: "Ecosystems");

            migrationBuilder.DropIndex(
                name: "IX_OpenIddictApplications_AppVersionTagId",
                table: "OpenIddictApplications");

            migrationBuilder.DropColumn(
                name: "AppVersionTagId",
                table: "OpenIddictApplications");

            migrationBuilder.DropColumn(
                name: "ContainerImage",
                table: "ApplicationTypes");
        }
    }
}
