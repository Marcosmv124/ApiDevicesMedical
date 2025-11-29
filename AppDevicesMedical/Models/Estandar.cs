using System.ComponentModel.DataAnnotations;

namespace AppDevicesMedical.Models
{
    public class Estandar
    {
        [Key]
        public int IdEstandar { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreEstandar { get; set; }

        [StringLength(100)]
        public string TipoEstandar { get; set; }

        public virtual ICollection<TransferenciaEstandar> TransferenciaEstandares { get; set; }
    }
}
