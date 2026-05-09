using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piano.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOefeningNamen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Oefeningen",
                keyColumn: "Id",
                keyValue: 1,
                column: "Naam",
                value: "Treble Clef");

            migrationBuilder.UpdateData(
                table: "Oefeningen",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Naam", "Niveau" },
                values: new object[] { "Bass Clef", 1 });

            migrationBuilder.UpdateData(
                table: "Oefeningen",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Naam", "Niveau" },
                values: new object[] { "Beide Clefs", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Oefeningen",
                keyColumn: "Id",
                keyValue: 1,
                column: "Naam",
                value: "Oefening 1");

            migrationBuilder.UpdateData(
                table: "Oefeningen",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Naam", "Niveau" },
                values: new object[] { "Oefening 2", 2 });

            migrationBuilder.UpdateData(
                table: "Oefeningen",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Naam", "Niveau" },
                values: new object[] { "Oefening 3", 3 });
        }
    }
}
