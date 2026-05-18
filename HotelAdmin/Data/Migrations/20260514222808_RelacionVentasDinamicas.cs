using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelAdmin.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelacionVentasDinamicas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_CodigoEmpleado",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "Ventas",
                table: "Funcionarios");

            migrationBuilder.AlterColumn<string>(
                name: "NombreHuesped",
                table: "Reservas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "FuncionarioId",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CodigoEmpleado",
                table: "Funcionarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_FuncionarioId",
                table: "Reservas",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Funcionarios_FuncionarioId",
                table: "Reservas",
                column: "FuncionarioId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Funcionarios_FuncionarioId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_FuncionarioId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "FuncionarioId",
                table: "Reservas");

            migrationBuilder.AlterColumn<string>(
                name: "NombreHuesped",
                table: "Reservas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoEmpleado",
                table: "Funcionarios",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Ventas",
                table: "Funcionarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_CodigoEmpleado",
                table: "Funcionarios",
                column: "CodigoEmpleado",
                unique: true);
        }
    }
}
