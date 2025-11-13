using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class ClaseRiesgo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_clase_riesgo { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre_clase { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        // Propiedad de navegación para la relación con Dispositivo
        public virtual ICollection<Dispositivo> Dispositivos { get; set; } = new List<Dispositivo>();
    }
}
