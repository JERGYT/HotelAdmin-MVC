using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAdmin.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPrecioReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrecioTotal",
                table: "Reservas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecioTotal",
                table: "Reservas");
        }
    }
}
