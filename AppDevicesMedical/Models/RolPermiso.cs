using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    [Table("Rol_Permiso")]
    public class RolPermiso
    {
        // Clave Compuesta: Usamos el IdRol y el IdPermiso como PKs
        [Key, Column(Order = 1)]
        public int IdRol { get; set; }

        [Key, Column(Order = 2)]
        public int IdPermiso { get; set; }

        // Propiedades de Navegación

        [ForeignKey("IdRol")]
        public Rol Rol { get; set; } = null!; // Asume que ya tienes Rol.cs

        [ForeignKey("IdPermiso")]
        public Permiso Permiso { get; set; } = null!;
    }
}
