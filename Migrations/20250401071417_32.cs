using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYPIBDPatientApp.Migrations
{
    /// <inheritdoc />
    public partial class _32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CookingMethod",
                table: "DietaryLogs");

            migrationBuilder.AddColumn<int>(
                name: "Healthiness",
                table: "DietaryLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "134c1566-3f64-4ab4-b1e7-2ffe11f43e32",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "43160ab4-b57d-43cd-9e7a-45b3a2472295", "AQAAAAIAAYagAAAAEAZZGU5riCQB4RlTRYzrd0O1121l/JyPRd2n44oKWIoYwfOEd6BCodApBT6MKyKzUg==", "02c7913f-4ec6-4472-b6c2-429d3cca5164" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Healthiness",
                table: "DietaryLogs");

            migrationBuilder.AddColumn<string>(
                name: "CookingMethod",
                table: "DietaryLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "134c1566-3f64-4ab4-b1e7-2ffe11f43e32",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b674229e-aff8-4733-8ac7-adbb920ddb3e", "AQAAAAIAAYagAAAAECAnUpG976aFl/wpMC4MgxsFxrKiODttTqq/DtyaTWJV4SlZWOpIZkYqsiSI9r0afg==", "fdf4db67-11de-4d02-9c4c-e1e5719e05ed" });
        }
    }
}
