using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    [Table("Permiso")]
    public class Permiso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPermiso { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty; // Ej: "GESTIONAR_ROLES", "VER_REPORTES_SECRETOS"

        [StringLength(255)]
        public string Descripcion { get; set; } = string.Empty;

        // Propiedad de navegación inversa (si la necesitas)
        public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
    }
}
