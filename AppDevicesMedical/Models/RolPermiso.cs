using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    /// <summary>
    /// Tabla Intermedia (Junction Table) para la relación Muchos a Muchos.
    /// Define qué permisos tiene asignado cada rol específico.
    /// </summary>
    [Table("Rol_Permiso")] // Mapea a la tabla "Rol_Permiso" en la BD.
    public class RolPermiso
    {
        // --- CLAVE COMPUESTA (COMPOSITE KEY) ---
        // La identidad de esta fila es la combinación de ambos IDs.
        // No puede haber dos filas con el mismo Rol y el mismo Permiso (evita duplicados).

        [Key, Column(Order = 1)]
        public int IdRol { get; set; } // Parte 1 de la PK y FK hacia Rol

        [Key, Column(Order = 2)]
        public int IdPermiso { get; set; } // Parte 2 de la PK y FK hacia Permiso

        // --- PROPIEDADES DE NAVEGACIÓN ---
        
        /// <summary>
        /// Referencia al objeto Rol completo.
        /// Permite acceder a datos como Rol.Nombre_rol
        /// </summary>
        [ForeignKey("IdRol")]
        public Rol Rol { get; set; } = null!; // null! indica que EF se encargará de llenarlo (non-nullable reference)

        /// <summary>
        /// Referencia al objeto Permiso completo.
        /// Permite acceder a datos como Permiso.Nombre ("VER_DASHBOARD")
        /// </summary>
        [ForeignKey("IdPermiso")]
        public Permiso Permiso { get; set; } = null!;
    }
}
