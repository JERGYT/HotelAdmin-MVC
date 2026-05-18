using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using HotelAdmin.Data;
using HotelAdmin.Models;

namespace HotelAdmin.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index(string periodo = "month")
        {
            var hoy = DateTime.Today;
            DateTime inicio = periodo switch
            {
                "week" => hoy.AddDays(-7),
                "quarter" => hoy.AddMonths(-3),
                "year" => hoy.AddYears(-1),
                _ => hoy.AddMonths(-1)
            };

            var reservas = await _context.Reservas.Where(r => r.CheckIn >= inicio && r.CheckIn <= hoy).ToListAsync();
            var funcionarios = await _context.Funcionarios.Include(f => f.Reservas).ToListAsync();

            var model = new ReportesViewModel
            {
                Periodo = periodo,
                TotalReservas = reservas.Count,
                IngresosTotales = reservas.Sum(r => r.PrecioTotal),
                TotalVentasFuncionarios = funcionarios.Sum(f => f.Reservas.Count),
                PromedioVentasFuncionario = funcionarios.Any() ? funcionarios.Average(f => f.Reservas.Count) : 0
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ExportarExcel()
        {
            var reservas = await _context.Reservas.Include(r => r.Funcionario).ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte de Reservas");

                string[] headers = { "ID Reserva", "Huésped", "Fecha Entrada", "Fecha Salida", "Tipo Habitación", "Estado", "Precio Total", "Funcionario" };

                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cell(1, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1e3a5f");
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Font.Bold = true;
                }

                int currentRow = 2;
                foreach (var r in reservas)
                {
                    worksheet.Cell(currentRow, 1).Value = r.Id;
                    worksheet.Cell(currentRow, 2).Value = r.NombreHuesped;
                    worksheet.Cell(currentRow, 3).Value = r.CheckIn.ToString("yyyy-MM-dd");
                    worksheet.Cell(currentRow, 4).Value = r.CheckOut.ToString("yyyy-MM-dd");
                    worksheet.Cell(currentRow, 5).Value = r.TipoHabitacion.ToString();
                    worksheet.Cell(currentRow, 6).Value = r.Estado.ToString();

                    worksheet.Cell(currentRow, 7).Value = r.PrecioTotal;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "$ #,##0";

                    worksheet.Cell(currentRow, 8).Value = r.Funcionario?.Nombre ?? "No asignado";
                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Reporte_Hotel_{DateTime.Now:yyyyMMdd}.xlsx");
                }
            }
        }
    }
}