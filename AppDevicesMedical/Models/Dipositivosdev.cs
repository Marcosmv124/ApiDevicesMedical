using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    /// <summary>
    /// Entidad principal que representa un Dispositivo Médico en el inventario/sistema.
    /// Centraliza la información técnica, regulatoria y los requisitos de infraestructura.
    /// </summary>
    public class Dispositivosdev
    {
        // --- IDENTIFICADOR ---
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incremental
        public int Id_dispositivo { get; set; }

        // --- DATOS GENERALES ---
        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Descripción técnica completa del dispositivo.
        /// Mapea a un campo TEXT o VARCHAR(MAX) en BD. Es opcional (nullable).
        /// </summary>
        public string? Descripcion_detallada { get; set; }

        // --- CLAVES FORÁNEAS (RELACIONES OPCIONALES) ---
        // El uso de 'int?' (nullable int) indica que estos campos no son obligatorios.
        // Un dispositivo podría guardarse temporalmente sin asignarle categoría o cuarto.

        public int? Id_categoria { get; set; }       // FK hacia tabla Categoria
        public int? Id_clase_riesgo { get; set; }    // FK hacia tabla ClaseRiesgo
        public int? Id_tipo_dispositivo { get; set; } // FK hacia tabla TipoDispositivo
        public int? Id_cuarto_requerido { get; set; } // FK hacia tabla Cuarto

        // --- CARACTERÍSTICAS TÉCNICAS (FLAGS) ---
        // Campos booleanos para filtrado rápido. Se inicializan en false por defecto.

        public bool Es_invasivo { get; set; } = false; 
        public bool Requiere_biocompatibilidad { get; set; } = false;
        public bool Requiere_prueba_residuales { get; set; } = false;

        // --- ESTERILIZACIÓN (TRANSICIÓN) ---
        
        /// <summary>
        /// Campo de texto libre para describir el método.
        /// Útil si el método no está en el catálogo o como campo histórico.
        /// </summary>
        [StringLength(100)]
        public string? Metodo_esterilizacion_req { get; set; } 
        
        /// <summary>
        /// ✅ NUEVO: Clave foránea hacia el catálogo de métodos de esterilización.
        /// Esto permite normalizar la base de datos en lugar de usar solo texto.
        /// </summary>
        public int? IdMetodoEsterilizacion { get; set; }

        // --- DATOS REGULATORIOS ---
        [Required]
        [StringLength(50)]
        public string Estado_regulatorio { get; set; } = string.Empty; // Ej: "Aprobado", "En Revisión"

        [Required]
        public DateTime Fecha_registro { get; set; }

        // --- PROPIEDADES DE NAVEGACIÓN (VIRTUALES) ---
        // Permiten acceder a los objetos relacionados (ej. dispositivo.Categoria.Nombre).
        // Son anulables (?) porque las FKs son anulables.

        [ForeignKey("Id_categoria")]
        public virtual Categoria? Categoria { get; set; }

        [ForeignKey("Id_clase_riesgo")]
        public virtual ClaseRiesgo? ClaseRiesgo { get; set; }

        [ForeignKey("Id_tipo_dispositivo")]
        public virtual TipoDispositivo? TipoDispositivo { get; set; }

        [ForeignKey("Id_cuarto_requerido")]
        public virtual Cuarto? CuartoRequerido { get; set; }
        
        // Comentado temporalmente hasta que exista la clase 'MetodoEsterilizacion'
        //[ForeignKey("IdMetodoEsterilizacion")]
        //public virtual MetodoEsterilizacion? MetodoEsterilizacion { get; set; }
    }
}
