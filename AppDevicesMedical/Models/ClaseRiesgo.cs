using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    /// <summary>
    /// Catálogo de Clasificación de Riesgo Sanitario.
    /// Define el nivel de riesgo potencial del dispositivo para el paciente (ej. Clase I, Clase III).
    /// Es vital para determinar los controles regulatorios aplicables.
    /// </summary>
    public class ClaseRiesgo
    {
        // --- IDENTIFICADOR ---
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incremental
        public int Id_clase_riesgo { get; set; }

        // --- NOMBRE DE LA CLASE ---
        /// <summary>
        /// Identificador corto de la norma (ej. "Clase I", "Clase IIb").
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Nombre_clase { get; set; } = string.Empty;

        // --- DESCRIPCIÓN ---
        /// <summary>
        /// Detalle explicativo del riesgo (ej. "Dispositivos de soporte de vida", "Riesgo bajo").
        /// Campo opcional.
        /// </summary>
        public string? Descripcion { get; set; }

        // --- RELACIONES ---
        
        /// <summary>
        /// Relación Uno a Muchos: Una sola clase de riesgo agrupa a muchos dispositivos diferentes.
        /// Ejemplo: Tanto un bisturí como unas gasas pueden ser Clase I.
        /// </summary>
        public virtual ICollection<Dispositivosdev> Dispositivos { get; set; } = new List<Dispositivosdev>();
    }
}
