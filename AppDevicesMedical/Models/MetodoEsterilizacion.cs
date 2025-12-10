using System.ComponentModel.DataAnnotations;

namespace AppDevicesMedical.Models
{
    /// <summary>
    /// Catálogo de Métodos de Esterilización.
    /// Define los procesos validados para eliminar carga microbiana de los dispositivos.
    /// Ejemplos: "Óxido de Etileno (EtO)", "Rayos Gamma", "Autoclave/Vapor".
    /// </summary>
    public class MetodoEsterilizacion
    {
        // --- IDENTIFICADOR ---
        [Key]
        // Al ser un int y tener [Key], EF Core asumirá por defecto que es IDENTITY (Auto-incremental)
        // a menos que especifiques lo contrario.
        public int IdMetodo { get; set; }

        // --- NOMBRE DEL MÉTODO ---
        [Required]
        [StringLength(100)]
        public string NombreMetodo { get; set; } = string.Empty; // Inicializado para evitar warning CS8618

        // --- DESCRIPCIÓN ---
        /// <summary>
        /// Detalles adicionales o normativa asociada (ej. "ISO 11135").
        /// Es opcional (nullable).
        /// </summary>
        [StringLength(255)]
        public string? Descripcion { get; set; }
    }
}
