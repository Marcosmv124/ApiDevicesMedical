using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class Dispositivosdev
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_dispositivo { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion_detallada { get; set; } // TEXT, NULLABLE (NUEVO)

        // Claves Foráneas (FKs)
        public int? Id_categoria { get; set; }
        public int? Id_clase_riesgo { get; set; }
        public int? Id_tipo_dispositivo { get; set; }
        public int? Id_cuarto_requerido { get; set; }

        // Nuevos campos booleanos
        public bool Es_invasivo { get; set; } = false; // DEFAULT: false (NUEVO)
        public bool Requiere_biocompatibilidad { get; set; } = false; // DEFAULT: false (NUEVO)
        public bool Requiere_prueba_residuales { get; set; } = false; // DEFAULT: false (NUEVO)

        [StringLength(100)]
        public string? Metodo_esterilizacion_req { get; set; } // VARCHAR, NULLABLE (NUEVO)
        
        // ✅ ESTE ES EL CAMPO NUEVO (La columna INT que acabas de crear en SQL)
        public int? IdMetodoEsterilizacion { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado_regulatorio { get; set; } = string.Empty; // VARCHAR

        [Required]
        public DateTime Fecha_registro { get; set; }

        // Propiedades de Navegación
        [ForeignKey("Id_categoria")]
        public virtual Categoria? Categoria { get; set; }

        [ForeignKey("Id_clase_riesgo")]
        public virtual ClaseRiesgo? ClaseRiesgo { get; set; }

        [ForeignKey("Id_tipo_dispositivo")]
        public virtual TipoDispositivo? TipoDispositivo { get; set; }

        [ForeignKey("Id_cuarto_requerido")]
        public virtual Cuarto? CuartoRequerido { get; set; }
      
        //[ForeignKey("IdMetodoEsterilizacion")]
        //public virtual MetodoEsterilizacion MetodoEsterilizacion { get; set; }
    }
}