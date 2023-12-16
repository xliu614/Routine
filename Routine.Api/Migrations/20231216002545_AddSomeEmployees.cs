using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Routine.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[,]
                {
                    { new Guid("4b501cb3-d168-4cc0-b375-48fb33f317a4"), new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"), new DateTime(1976, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT232", "Nick", 2, "Cas" },
                    { new Guid("4b501cb3-d168-4cc0-b375-48fb33f318a5"), new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"), new DateTime(1977, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT239", "Nicholas", 1, "Cas" },
                    { new Guid("4b501cb3-d168-4cc0-b375-48fb33f318a6"), new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"), new DateTime(1977, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT240", "Vincent", 1, "Agent" },
                    { new Guid("4b501cb3-d168-4cc0-b375-48fb33f318b4"), new Guid("5836d897-741b-4895-ae31-e6f3f1be1ffd"), new DateTime(1978, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "MSFT240", "Louis", 1, "Gentel" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("4b501cb3-d168-4cc0-b375-48fb33f317a4"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("4b501cb3-d168-4cc0-b375-48fb33f318a5"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("4b501cb3-d168-4cc0-b375-48fb33f318a6"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("4b501cb3-d168-4cc0-b375-48fb33f318b4"));
        }
    }
}
