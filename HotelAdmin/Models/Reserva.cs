using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelAdmin.Models
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del huésped es obligatorio")]
        public string NombreHuesped { get; set; } = string.Empty;

        [Required]
        public DateTime CheckIn { get; set; }

        [Required]
        public DateTime CheckOut { get; set; }

        [Required]
        public TipoHabitacion TipoHabitacion { get; set; }

        public EstadoReserva Estado { get; set; } = EstadoReserva.Pendiente;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioTotal { get; set; }

        [Required(ErrorMessage = "Debe asignar un funcionario a la reserva")]
        public int FuncionarioId { get; set; }

        [ForeignKey("FuncionarioId")]
        public virtual Funcionario? Funcionario { get; set; }
    }
}