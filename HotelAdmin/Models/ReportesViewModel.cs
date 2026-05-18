namespace HotelAdmin.Models
{
    public class ReportesViewModel
    {
        public string Periodo { get; set; } = "month";
        public int TotalReservas { get; set; }
        public decimal IngresosTotales { get; set; }
        public int TotalVentasFuncionarios { get; set; }
        public double PromedioVentasFuncionario { get; set; }
    }
}