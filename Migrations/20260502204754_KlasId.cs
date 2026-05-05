using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Piano.Migrations
{
    /// <inheritdoc />
    public partial class KlasId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leerlingen_Klassen_KlasId",
                table: "Leerlingen");

            migrationBuilder.AlterColumn<int>(
                name: "KlasId",
                table: "Leerlingen",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Docenten",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Docenten",
                keyColumn: "Id",
                keyValue: 1,
                column: "IdentityUserId",
                value: "");

            migrationBuilder.UpdateData(
                table: "Docenten",
                keyColumn: "Id",
                keyValue: 2,
                column: "IdentityUserId",
                value: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Leerlingen_Klassen_KlasId",
                table: "Leerlingen",
                column: "KlasId",
                principalTable: "Klassen",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leerlingen_Klassen_KlasId",
                table: "Leerlingen");

            migrationBuilder.AlterColumn<int>(
                name: "KlasId",
                table: "Leerlingen",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityUserId",
                table: "Docenten",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "Docenten",
                keyColumn: "Id",
                keyValue: 1,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Docenten",
                keyColumn: "Id",
                keyValue: 2,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.AddForeignKey(
                name: "FK_Leerlingen_Klassen_KlasId",
                table: "Leerlingen",
                column: "KlasId",
                principalTable: "Klassen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
