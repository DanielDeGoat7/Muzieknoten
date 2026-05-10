using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Piano.Migrations
{
    /// <inheritdoc />
    public partial class Resultaat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Resultaten",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Resultaten",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Resultaten",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "LeerlingId",
                table: "Resultaten");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Resultaten",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Resultaten_OefeningId",
                table: "Resultaten",
                column: "OefeningId");

            migrationBuilder.CreateIndex(
                name: "IX_Resultaten_UserId",
                table: "Resultaten",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resultaten_AspNetUsers_UserId",
                table: "Resultaten",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resultaten_Oefeningen_OefeningId",
                table: "Resultaten",
                column: "OefeningId",
                principalTable: "Oefeningen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resultaten_AspNetUsers_UserId",
                table: "Resultaten");

            migrationBuilder.DropForeignKey(
                name: "FK_Resultaten_Oefeningen_OefeningId",
                table: "Resultaten");

            migrationBuilder.DropIndex(
                name: "IX_Resultaten_OefeningId",
                table: "Resultaten");

            migrationBuilder.DropIndex(
                name: "IX_Resultaten_UserId",
                table: "Resultaten");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Resultaten");

            migrationBuilder.AddColumn<int>(
                name: "LeerlingId",
                table: "Resultaten",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Resultaten",
                columns: new[] { "Id", "LeerlingId", "OefeningId", "Score", "datetime" },
                values: new object[,]
                {
                    { 1, 1, 1, 85, new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, 2, 92, new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, 3, 98, new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }
    }
}
