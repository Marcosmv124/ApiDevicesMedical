using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class TipoDispositivo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_tipo { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre_tipo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        // Propiedad de navegación para la relación con Dispositivo
        public virtual ICollection<Dispositivosdev> Dispositivos { get; set; } = new List<Dispositivosdev>();
    }
}