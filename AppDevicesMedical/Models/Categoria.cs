using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Mapea a IDENTITY(1,1)
        public int Id_Categoria { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre_Categoria { get; set; } = string.Empty; // Mapea a nombre_Categoria

        [StringLength(50)]
        public string? Descripcion { get; set; }

        [Required]
        [Column(TypeName = "Date")] // Especifica el tipo DATE para la base de datos
        public DateTime Fecha_de_creación { get; set; } // Mapea a fecha_de_creación
    }
}
