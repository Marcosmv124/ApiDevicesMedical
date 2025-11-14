using System.ComponentModel.DataAnnotations;

namespace AppDevicesMedical.DTOs
{
    public class TipoDispositivoCreateDto
    {
        public string Nombre_tipo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }
    }
}
