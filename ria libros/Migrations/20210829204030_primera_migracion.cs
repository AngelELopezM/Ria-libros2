using Microsoft.EntityFrameworkCore.Migrations;

namespace ria_libros.Migrations
{
    public partial class primera_migracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Genero = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Autor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    año = table.Column<int>(type: "int", nullable: false),
                    ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Libros");
        }
    }
}
