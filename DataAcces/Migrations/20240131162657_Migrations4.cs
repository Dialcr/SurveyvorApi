﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BePrácticasLaborales.Migrations
{
    /// <inheritdoc />
    public partial class Migrations4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "admin", "ADMIN" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "support", "SUPPORT" }
            );
        }
    }
}
