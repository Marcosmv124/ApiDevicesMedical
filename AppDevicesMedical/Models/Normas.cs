using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class Normas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Mapea a IDENTITY(1,1)
        public int Id_norma { get; set; }

        [Required]
        [StringLength(100)]
        public string Codigo_norma { get; set; } = string.Empty; // Mapea a codigo_norma

        [StringLength(50)]
        public string? Descripcion { get; set; }
    }
}
