using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelAdmin.Data;
using HotelAdmin.Models;

namespace HotelAdmin.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hoy = DateTime.Today;

            var model = new DashboardViewModel
            {
                ReservasActivas = await _context.Reservas
                    .CountAsync(r => r.Estado == EstadoReserva.Confirmada && r.CheckOut >= hoy),

                HuespedesHoy = await _context.Reservas
                    .CountAsync(r => r.Estado == EstadoReserva.Confirmada && r.CheckIn <= hoy && r.CheckOut >= hoy),

                ReservasRecientes = await _context.Reservas
                    .OrderByDescending(r => r.Id)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }
    }
}