using System.ComponentModel.DataAnnotations;

namespace AppDevicesMedical.Models
{
    public class MetodoEsterilizacion
    {
        [Key]
        public int IdMetodo { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreMetodo { get; set; }

        [StringLength(255)]
        public string? Descripcion { get; set; }
    }
}
