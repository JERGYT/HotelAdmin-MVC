using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelAdmin.Models
{
    public class Funcionario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CodigoEmpleado { get; set; } = string.Empty;

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;

        public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}