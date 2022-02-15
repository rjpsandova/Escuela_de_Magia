using Microsoft.EntityFrameworkCore.Migrations;

namespace Escuela_de_Magia.Migrations
{
    public partial class Cambio1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Edad",
                table: "Estudiante",
                type: "numeric(2,0)",
                maxLength: 2,
                precision: 2,
                scale: 0,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Edad",
                table: "Estudiante",
                type: "int",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(2,0)",
                oldMaxLength: 2,
                oldPrecision: 2,
                oldScale: 0);
        }
    }
}
