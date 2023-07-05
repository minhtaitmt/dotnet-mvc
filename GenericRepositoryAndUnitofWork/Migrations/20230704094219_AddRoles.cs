using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenericRepositoryAndUnitofWork.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1855a90f-1ab4-488d-9a28-c1885972dabf", "2", "user", "user" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cce57752-58b6-4177-a6f5-a961d3a6596e", "3", "hr", "hr" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cfd87bf9-f30e-4960-b3ef-7e80ab769cd5", "1", "admin", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1855a90f-1ab4-488d-9a28-c1885972dabf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cce57752-58b6-4177-a6f5-a961d3a6596e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cfd87bf9-f30e-4960-b3ef-7e80ab769cd5");
        }
    }
}
