using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ensek.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 25, nullable: false),
                    LastName = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountId", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 2344, "Tommy", "Test" },
                    { 2233, "Barry", "Test" },
                    { 8766, "Sally", "Test" },
                    { 2345, "Jerry", "Test" },
                    { 2346, "Ollie", "Test" },
                    { 2347, "Tara", "Test" },
                    { 2348, "Tammy", "Test" },
                    { 2349, "Simon", "Test" },
                    { 2350, "Colin", "Test" },
                    { 2351, "Gladys", "Test" },
                    { 2352, "Greg", "Test" },
                    { 2353, "Tony", "Test" },
                    { 2355, "Arthur", "Test" },
                    { 2356, "Craig", "Test" },
                    { 6776, "6776", "Test" },
                    { 4534, "JOSH", "Test" },
                    { 1234, "Freya", "Test" },
                    { 1239, "Noddy", "Test" },
                    { 1240, "Archie", "Test" },
                    { 1241, "Lara", "Test" },
                    { 1242, "Tim", "Test" },
                    { 1243, "Graham", "Test" },
                    { 1244, "Tony", "Test" },
                    { 1245, "Neville", "Test" },
                    { 1246, "Jo", "Test" },
                    { 1247, "Jim", "Test" },
                    { 1248, "Pam", "Test" }
                });

            migrationBuilder.CreateTable(
                name: "MeterReadings",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false),
                    MeterReadingDateTime = table.Column<DateTime>(nullable: false),
                    MeterReadValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadings", x => new { x.AccountId, x.MeterReadingDateTime });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "MeterReadings");
        }
    }
}
