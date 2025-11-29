using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class SistemaCalidad
    {
        [Key]
        [StringLength(5)]
        public string IdSistema { get; set; } // 'MX', 'JP'

        [Required]
        [StringLength(100)]
        public string NombreSistema { get; set; }

        [StringLength(100)]
        public string Pais { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal FactorCosto { get; set; }
    }
}
