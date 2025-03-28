using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYPIBDPatientApp.Migrations
{
    /// <inheritdoc />
    public partial class Tokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isRevoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "134c1566-3f64-4ab4-b1e7-2ffe11f43e32",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b674229e-aff8-4733-8ac7-adbb920ddb3e", "AQAAAAIAAYagAAAAECAnUpG976aFl/wpMC4MgxsFxrKiODttTqq/DtyaTWJV4SlZWOpIZkYqsiSI9r0afg==", "fdf4db67-11de-4d02-9c4c-e1e5719e05ed" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "134c1566-3f64-4ab4-b1e7-2ffe11f43e32",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "50119701-3e63-49d7-82e5-8d11330458b2", "AQAAAAIAAYagAAAAEAljMxel4KEY9r9z3eAHVR8brDRVqRF4vE2vl2myGkdPi3ImjY46tQ26x4It1DJh3Q==", "de952234-37dc-49be-90d5-545a01d09c9f" });
        }
    }
}
