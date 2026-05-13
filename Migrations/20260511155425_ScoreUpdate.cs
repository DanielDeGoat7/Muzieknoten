using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piano.Migrations
{
    /// <inheritdoc />
    public partial class ScoreUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AantalVragen",
                table: "Resultaten",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoedeAntwoorden",
                table: "Resultaten",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AantalVragen",
                table: "Resultaten");

            migrationBuilder.DropColumn(
                name: "GoedeAntwoorden",
                table: "Resultaten");
        }
    }
}
