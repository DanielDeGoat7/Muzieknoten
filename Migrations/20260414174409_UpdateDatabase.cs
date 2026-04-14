using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Piano.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KlasId",
                table: "Leerlingen",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Docenten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naam = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docenten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oefeningen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naam = table.Column<string>(type: "TEXT", nullable: false),
                    Niveau = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oefeningen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resultaten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    datetime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LeerlingId = table.Column<int>(type: "INTEGER", nullable: false),
                    OefeningId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resultaten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Klassen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naam = table.Column<string>(type: "TEXT", nullable: false),
                    DocentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klassen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Klassen_Docenten_DocentId",
                        column: x => x.DocentId,
                        principalTable: "Docenten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Docenten",
                columns: new[] { "Id", "Naam" },
                values: new object[,]
                {
                    { 1, "Daniël Dedden" },
                    { 2, "Jan de Boer" }
                });

            migrationBuilder.UpdateData(
                table: "Leerlingen",
                keyColumn: "Id",
                keyValue: 1,
                column: "KlasId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Leerlingen",
                keyColumn: "Id",
                keyValue: 2,
                column: "KlasId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Leerlingen",
                keyColumn: "Id",
                keyValue: 3,
                column: "KlasId",
                value: 2);

            migrationBuilder.InsertData(
                table: "Oefeningen",
                columns: new[] { "Id", "Naam", "Niveau" },
                values: new object[,]
                {
                    { 1, "Oefening 1", 1 },
                    { 2, "Oefening 2", 2 },
                    { 3, "Oefening 3", 3 }
                });

            migrationBuilder.InsertData(
                table: "Resultaten",
                columns: new[] { "Id", "LeerlingId", "OefeningId", "Score", "datetime" },
                values: new object[,]
                {
                    { 1, 1, 1, 85, new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, 2, 92, new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, 3, 98, new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Klassen",
                columns: new[] { "Id", "DocentId", "Naam" },
                values: new object[,]
                {
                    { 1, 1, "PianoX1" },
                    { 2, 2, "PianoX2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leerlingen_KlasId",
                table: "Leerlingen",
                column: "KlasId");

            migrationBuilder.CreateIndex(
                name: "IX_Klassen_DocentId",
                table: "Klassen",
                column: "DocentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leerlingen_Klassen_KlasId",
                table: "Leerlingen",
                column: "KlasId",
                principalTable: "Klassen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leerlingen_Klassen_KlasId",
                table: "Leerlingen");

            migrationBuilder.DropTable(
                name: "Klassen");

            migrationBuilder.DropTable(
                name: "Oefeningen");

            migrationBuilder.DropTable(
                name: "Resultaten");

            migrationBuilder.DropTable(
                name: "Docenten");

            migrationBuilder.DropIndex(
                name: "IX_Leerlingen_KlasId",
                table: "Leerlingen");

            migrationBuilder.DropColumn(
                name: "KlasId",
                table: "Leerlingen");
        }
    }
}
