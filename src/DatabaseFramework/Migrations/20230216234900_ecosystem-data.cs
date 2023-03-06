using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseFramework.Migrations
{
    public partial class ecosystemdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Ecosystems",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { -4, "Governance applications including Let's Agree", "Comax - Governance" },
                    { -3, "Accounts and security ecosystem", "Comax - Accounts" },
                    { -2, "Orchestrator ecosystem", "Comax - Orchestrator" },
                    { -1, "Commons client ecosystem including the agent", "Comax - Commons" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ecosystems",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Ecosystems",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "Ecosystems",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Ecosystems",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
