using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class Especialidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Con auto-incremento
        public int Id_Especialidad { get; set; }
        [Required]
        [StringLength(50)]
        public string Nom_Especialidad { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
