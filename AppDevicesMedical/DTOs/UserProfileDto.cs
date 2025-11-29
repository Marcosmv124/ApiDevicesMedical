namespace AppDevicesMedical.DTOs
{
    public class UserProfileDto
    {
        public int IdUsuario { get; set; }
        public string NumeroEmpleado { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string NombreRol { get; set; } = string.Empty;
        // Puedes agregar más campos si los necesitas (Email, Foto, etc.)

        // Propiedad extra para nombre completo si el front la quiere fácil
        public string NombreCompleto => $"{Nombres} {ApellidoPaterno}";
    }
}
