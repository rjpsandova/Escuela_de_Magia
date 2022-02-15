using Microsoft.EntityFrameworkCore.Migrations;

namespace Escuela_de_Magia.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Casa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Casa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estudiante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Identificacion = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    Edad = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    CasaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estudiante_Casa_CasaId",
                        column: x => x.CasaId,
                        principalTable: "Casa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Casa_Id",
                table: "Casa",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estudiante_CasaId",
                table: "Estudiante",
                column: "CasaId");

            migrationBuilder.CreateIndex(
                name: "IX_Estudiante_Id",
                table: "Estudiante",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estudiante");

            migrationBuilder.DropTable(
                name: "Casa");
        }
    }
}
