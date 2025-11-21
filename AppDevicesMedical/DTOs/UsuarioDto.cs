using AppDevicesMedical.Models;

namespace AppDevicesMedical.DTOs
{
    public class UsuarioDto
    {
        // Identificador
        public int IdUsuario { get; set; }

        // Datos personales
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string NumeroEmpleado { get; set; } = string.Empty;

        // Contraseña (solo en registro o cambio)
        public string Password { get; set; } = string.Empty;

        // Fecha de creación (nullable para PUT)
        public DateTime? FechaCreacion { get; set; }

        // Rol
        public int IdRol { get; set; }
        public string? NombreRol { get; set; }

        // Status
        public int IdStatus { get; set; }   // ⚠️ mejor no nullable si en BD es NOT NULL
        public string? DescripcionStatus { get; set; }

        // Especialidad
        public int IdEspecialidad { get; set; }   // ⚠️ mejor no nullable si en BD es NOT NULL
        public string? NombreEspecialidad { get; set; }
    }
}