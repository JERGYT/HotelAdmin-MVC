using Microsoft.AspNetCore.Identity;

namespace HotelAdmin.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;
    }
}