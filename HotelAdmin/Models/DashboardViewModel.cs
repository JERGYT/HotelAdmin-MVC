using System.Collections.Generic;

namespace HotelAdmin.Models
{
    public class DashboardViewModel
    {
        public int ReservasActivas { get; set; }
        public int HuespedesHoy { get; set; }
        public List<Reserva> ReservasRecientes { get; set; } = new List<Reserva>();
    }
}