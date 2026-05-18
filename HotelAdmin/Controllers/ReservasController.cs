using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelAdmin.Data;
using HotelAdmin.Models;

namespace HotelAdmin.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas.Include(r => r.Funcionario).ToListAsync();
            ViewBag.Funcionarios = new SelectList(await _context.Funcionarios.ToListAsync(), "Id", "Nombre");
            return View(reservas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            ModelState.Remove("Funcionario");

            int dias = (reserva.CheckOut - reserva.CheckIn).Days;
            if (dias <= 0) dias = 1;

            decimal tarifa = reserva.TipoHabitacion switch
            {
                TipoHabitacion.Simple => 50000,
                TipoHabitacion.Doble => 80000,
                TipoHabitacion.Suite => 150000,
                TipoHabitacion.SuiteEjecutiva => 250000,
                _ => 50000
            };

            reserva.PrecioTotal = tarifa * dias;

            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Funcionarios = new SelectList(await _context.Funcionarios.ToListAsync(), "Id", "Nombre");
            return View("Index", await _context.Reservas.Include(r => r.Funcionario).ToListAsync());
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();

            ViewBag.Funcionarios = new SelectList(await _context.Funcionarios.ToListAsync(), "Id", "Nombre", reserva.FuncionarioId);
            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reserva reserva)
        {
            if (id != reserva.Id) return NotFound();

            ModelState.Remove("Funcionario"); 

            if (ModelState.IsValid)
            {
                try
                {
                    int dias = (reserva.CheckOut - reserva.CheckIn).Days;
                    if (dias <= 0) dias = 1;

                    decimal tarifa = reserva.TipoHabitacion switch
                    {
                        TipoHabitacion.Simple => 50000,
                        TipoHabitacion.Doble => 80000,
                        TipoHabitacion.Suite => 150000,
                        TipoHabitacion.SuiteEjecutiva => 250000,
                        _ => 50000
                    };
                    reserva.PrecioTotal = tarifa * dias;

                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Funcionarios = new SelectList(await _context.Funcionarios.ToListAsync(), "Id", "Nombre", reserva.FuncionarioId);
            return View(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id) => _context.Reservas.Any(e => e.Id == id);
    }
}