using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Routine.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCompaniesProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Companies",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "Companies",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "Companies",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("552bb206-bab4-4c7f-8353-b058a6dc62f4"),
                columns: new[] { "Country", "Industry", "Product" },
                values: new object[] { "USA", "Software", "Software" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"),
                columns: new[] { "Country", "Industry", "Product" },
                values: new object[] { "USA", "Internet", "Software" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("5efc910b-2f45-43df-afae-620d40542800"),
                columns: new[] { "Country", "Industry", "Introduction", "Product" },
                values: new object[] { "USA", "Design", "Photoshop", "Software" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("5efc910b-2f45-43df-afae-620d40542844"),
                columns: new[] { "Country", "Industry", "Product" },
                values: new object[] { "USA", "Software", "Software" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("6fb600c1-9011-4fd7-9234-881379716400"),
                columns: new[] { "Country", "Industry", "Product" },
                values: new object[] { "China", "Internet", "Software" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("6fb600c1-9011-4fd7-9234-881379716422"),
                columns: new[] { "Country", "Industry", "Product" },
                values: new object[] { "USA", "Internet", "Software" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("6fb600c1-9011-4fd7-9234-881379716444"),
                columns: new[] { "Country", "Industry", "Product" },
                values: new object[] { "USA", "Software", "Software" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("70608d93-5746-4b0b-b73d-88871033a660"),
                columns: new[] { "Country", "Industry", "Product" },
                values: new object[] { "China", "Internet", "Software" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("bbdee09c-089b-4d30-bece-44df59237100"),
                columns: new[] { "Country", "Industry", "Product" },
                values: new object[] { "China", "Internet", "Software" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("bbdee09c-089b-4d30-bece-44df59237111"),
                columns: new[] { "Country", "Industry", "Product" },
                values: new object[] { "USA", "Space", "Hardware" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Industry",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "Companies");

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("5efc910b-2f45-43df-afae-620d40542800"),
                column: "Introduction",
                value: "Photoshop?");
        }
    }
}
