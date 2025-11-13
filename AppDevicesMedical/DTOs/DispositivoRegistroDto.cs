using System.ComponentModel.DataAnnotations;

namespace AppDevicesMedical.DTOs
{
    public class DispositivoRegistroDto
    {
        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty;

        // FKs
        public int? Id_categoria { get; set; }
        public int? Id_normas_aplicables { get; set; }
        public int? Id_cuarto { get; set; }

        // Campos de valor
        [StringLength(25)]
        public string? Clase_riesgo { get; set; }

        [Required]
        [StringLength(100)]
        public string Estado { get; set; } = "En Proceso";
    }
}
