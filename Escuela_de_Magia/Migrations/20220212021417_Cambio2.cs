using Microsoft.EntityFrameworkCore.Migrations;

namespace Escuela_de_Magia.Migrations
{
    public partial class Cambio2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Identificacion",
                table: "Estudiante",
                type: "numeric(10,0)",
                maxLength: 10,
                precision: 10,
                scale: 0,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Identificacion",
                table: "Estudiante",
                type: "int",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,0)",
                oldMaxLength: 10,
                oldPrecision: 10,
                oldScale: 0);
        }
    }
}
