using System.ComponentModel.DataAnnotations;

namespace AppDevicesMedical.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // Datos del Perfil (ApplicationUser)
        [Required] public string Nombres { get; set; } = string.Empty;
        [Required] public string ApellidoPaterno { get; set; } = string.Empty;
        [Required] public string ApellidoMaterno { get; set; } = string.Empty;
        [Required] public string NumeroEmpleado { get; set; } = string.Empty;

        // Claves Foráneas (FKs)
        [Required] public int IdStatus { get; set; }
        public int? IdEspecialidad { get; set; } // Puede ser null

        // Rol que se asignará al usuario en Identity
        [Required] public string Rol { get; set; } = "Consultor";
    }
}
