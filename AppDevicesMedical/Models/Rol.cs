using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    /// <summary>
    /// Entidad que representa un Rol o Perfil de seguridad en el sistema.
    /// Define los grupos a los que pertenecen los usuarios (ej. Admin, Médico).
    /// </summary>
    public class Rol
    {
        // --- LLAVE PRIMARIA (MANUAL) ---
        /// <summary>
        /// Identificador único del rol.
        /// NOTA: No es autoincremental. Los IDs se definen manualmente (ej. 1=Admin, 2=User)
        /// para mantener consistencia entre entornos (Dev, QA, Prod).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // ⚠️ CRÍTICO: Indica que la DB NO debe generar el ID.
        public int Id_rol { get; set; }

        // --- DATOS DEL ROL ---
        /// <summary>
        /// Nombre descriptivo del rol (ej. "Administrador").
        /// </summary>
        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [StringLength(50)]
        public string Nombre_rol { get; set; } = string.Empty;

        /// <summary>
        /// Breve descripción de las responsabilidades de este rol.
        /// Es opcional (nullable).
        /// </summary>
        [StringLength(50)]
        public string? Descripcion { get; set; }

        // --- RELACIONES (NAVEGACIÓN) ---
        
        /// <summary>
        /// Colección de permisos asignados a este rol.
        /// Relación: Uno a Muchos (Un Rol tiene muchos RolPermisos).
        /// </summary>
        // ✅ Inicializado como lista vacía para evitar NullReferenceException al agregar items.
        public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
    }
}
