using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYPIBDPatientApp.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BowelMovementLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoolType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Blood = table.Column<bool>(type: "bit", nullable: false),
                    Urgency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BowelMovementLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BowelMovementLogs_AspNetUsers_PatientId",
                        column: x => x.PatientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "134c1566-3f64-4ab4-b1e7-2ffe11f43e32",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a1b80a28-5ac2-47f9-8c2b-0768909f2978", "AQAAAAIAAYagAAAAEMvC3EwcbkWRO+t3TCHKXg0LC0KRHLrGiFWaWxY8y7ApIIX299ORe41btq01ScPcWA==", "5740cf84-cc07-46b0-b02c-fe9951559108" });

            migrationBuilder.CreateIndex(
                name: "IX_BowelMovementLogs_PatientId",
                table: "BowelMovementLogs",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BowelMovementLogs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "134c1566-3f64-4ab4-b1e7-2ffe11f43e32",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b1bafba0-c570-427e-8e76-75105b00df02", "AQAAAAIAAYagAAAAEEuaKmLKc8powhaW/wq+aDbyKUhNrLgz4p+gx3quk+/DXPYjNw2SgvcSu+7QwfrBtw==", "94d9da69-ecd2-422b-8b33-ec968ee9ae32" });
        }
    }
}
