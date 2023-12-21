using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Routine.Api.Migrations
{
    /// <inheritdoc />
    public partial class adjustColumnsAndAddCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Introduction = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Industry = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Product = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmployeeNo = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Country", "Industry", "Introduction", "Name", "Product" },
                values: new object[,]
                {
                    { new Guid("552bb206-bab4-4c7f-8353-b058a6dc62f4"), "USA", "Software", "Evil source", "Microsoft", "Software" },
                    { new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"), "USA", "Internet", "Never get in!", "Google", "Software" },
                    { new Guid("5efc910b-2f45-43df-afae-620d40542800"), "USA", "Design", "Photoshop", "Adobe", "Software" },
                    { new Guid("5efc910b-2f45-43df-afae-620d40542844"), "USA", "Software", "Is it a company?", "Firefox", "Software" },
                    { new Guid("5efc910b-2f45-43df-afae-620d40542845"), null, null, "Is it a company?", "TestingCompany1", null },
                    { new Guid("6fb600c1-9011-4fd7-9234-881379716400"), "China", "Internet", "From Beijing", "Baidu", "Software" },
                    { new Guid("6fb600c1-9011-4fd7-9234-881379716422"), "USA", "Internet", "Blocked", "Youtube", "Software" },
                    { new Guid("6fb600c1-9011-4fd7-9234-881379716444"), "USA", "Software", "Who?", "Yahoo", "Software" },
                    { new Guid("70608d93-5746-4b0b-b73d-88871033a660"), "China", "Internet", "Fubao China", "Alipapa", "Software" },
                    { new Guid("bbdee09c-089b-4d30-bece-44df59237100"), "China", "Internet", "From Shenzhen", "Tencent", "Software" },
                    { new Guid("bbdee09c-089b-4d30-bece-44df59237111"), "USA", "Space", "Wow", "SpaceX", "Hardware" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[,]
                {
                    { new Guid("1861341e-b42b-410c-ae21-cf11f36fc574"), new Guid("70608d93-5746-4b0b-b73d-88871033a660"), new DateTime(1957, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "A404", "Not", 1, "Man" },
                    { new Guid("4b501cb3-d168-4cc0-b375-48fb33f317a4"), new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"), new DateTime(1976, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT232", "Nick", 2, "Cas" },
                    { new Guid("4b501cb3-d168-4cc0-b375-48fb33f318a4"), new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"), new DateTime(1976, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT231", "Nick", 1, "Carter" },
                    { new Guid("4b501cb3-d168-4cc0-b375-48fb33f318a5"), new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"), new DateTime(1977, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT239", "Nicholas", 1, "Cas" },
                    { new Guid("4b501cb3-d168-4cc0-b375-48fb33f318a6"), new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"), new DateTime(1977, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT240", "Vincent", 1, "Agent" },
                    { new Guid("4b501cb3-d168-4cc0-b375-48fb33f318b4"), new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"), new DateTime(1978, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT240", "Louis", 1, "Gentel" },
                    { new Guid("679dfd33-32e4-4393-b061-f7abb8956f53"), new Guid("552bb206-bab4-4c7f-8353-b058a6dc62f4"), new DateTime(1967, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "A009", "Carr", 1, "Lee" },
                    { new Guid("72457e73-ea34-4e02-b575-8d384e82a481"), new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"), new DateTime(1986, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "G003", "Mary", 2, "King" },
                    { new Guid("7644b71d-d74e-43e2-ac32-8cbadd7b1c3a"), new Guid("552bb206-bab4-4c7f-8353-b058a6dc62f4"), new DateTime(1977, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "G097", "Kevin", 1, "Richardson" },
                    { new Guid("7eaa532c-1be5-472c-a738-94fd26e5fad6"), new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"), new DateTime(1981, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT245", "Vince", 1, "Carter" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
