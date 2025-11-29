using System.ComponentModel.DataAnnotations;

namespace AppDevicesMedical.DTOs
{
    public class RegisterDto
    {
        [Required]
        public int IdUsuario { get; set; }
        [Required] 
        public string Nombres { get; set; } = string.Empty;
        [Required] 
        public string ApellidoPaterno { get; set; } = string.Empty;
        [Required] 
        public string ApellidoMaterno { get; set; } = string.Empty;
        [Required] 
        public string NumeroEmpleado { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        // Rol
        public int IdRol { get; set; }

        // Status
        public int IdStatus { get; set; }   //mejor no nullable si en BD es NOT NULL

        // Especialidad
        public int IdEspecialidad { get; set; }   //mejor no nullable si en BD es NOT NULL
    }
   
}
