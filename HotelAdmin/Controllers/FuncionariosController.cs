using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelAdmin.Data;
using HotelAdmin.Models;

namespace HotelAdmin.Controllers
{
    [Authorize]
    public class FuncionariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FuncionariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _context.Funcionarios.Include(f => f.Reservas).ToListAsync();
            return View(lista);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Funcionario funcionario)
        {
            ModelState.Remove("Reservas");
            funcionario.Email ??= "ventas@hotel.com";
            funcionario.Telefono ??= "N/A";

            if (ModelState.IsValid)
            {
                _context.Add(funcionario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var f = await _context.Funcionarios.FindAsync(id);
            if (f != null)
            {
                _context.Funcionarios.Remove(f);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}