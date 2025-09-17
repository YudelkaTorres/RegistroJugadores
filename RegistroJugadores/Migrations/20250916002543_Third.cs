using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroJugadores.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Partidas",
                table: "Jugadores",
                newName: "Victoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Victoria",
                table: "Jugadores",
                newName: "Partidas");
        }
    }
}
