using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class Rol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Crucial: Evita el autoincremento (IDENTITY)
        public int Id_rol { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre_rol { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Descripcion { get; set; }
    }
}
