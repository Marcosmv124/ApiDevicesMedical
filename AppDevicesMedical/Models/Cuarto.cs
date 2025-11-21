using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AppDevicesMedical.Models
{
    [Table("Cuarto_Limpio")]
    public class Cuarto
    {
        // --- Identificación básica ---
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_cuarto { get; set; }

        [Required, StringLength(50)]
        public string Nombre_cuarto { get; set; } = string.Empty;  // Ej: Cuarto 1

        [Required, StringLength(20)]
        public string Clase_cuarto { get; set; } = string.Empty;  // Clase 7, 8, 9...

        // --- 20 características generales agrupadas ---

        // 1. Dimensiones y capacidad
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Dimensiones_m2 { get; set; }  // m²

        public int? Capacidad_personas { get; set; }  // Nº máximo de personas

        public int? Capacidad_produccion { get; set; }  // Producción máxima permitida

        // 2. Condiciones ambientales
        public bool Control_temperatura { get; set; } = false;
        public bool Control_humedad { get; set; } = false;
        [StringLength(255)]
        public string? Espec_flujo_aire { get; set; }
        public int? Capacidad_hvac_cfm { get; set; }
        [StringLength(100)]
        public string? Tipo_acondicionamiento { get; set; }
        public int? Tiempo_reposicion_aire_min { get; set; }

        // 3. Parámetros de control de contaminación
        public int? Limite_contaminacion_ufc { get; set; }
        public int? Contaminacion_actual_ufc { get; set; }
        public int? Limite_particulas_no_viables { get; set; }
        public int? Conteo_particulas_no_viables { get; set; }
        public int? Limite_particulas_viables { get; set; }
        public int? Conteo_particulas_viables { get; set; }

        // 4. Condiciones ambientales adicionales
        [Column(TypeName = "decimal(6,2)")]
        public decimal? Presion_diferencial_pa { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Nivel_ruido_db { get; set; }
        [Column(TypeName = "decimal(7,2)")]
        public decimal? Iluminacion_lux { get; set; }
        public bool Filtracion_hepa { get; set; } = false;
        [StringLength(50)]
        public string? Nivel_limpieza_iso { get; set; }  // Ej: ISO 14644 Clase 8

        // --- Campos de control y documentación ---
        [StringLength(50)]
        public string? Estado_actual { get; set; } = "Vacío";

        [StringLength(50)]
        public string? Etapa_proceso { get; set; }  // Ej: Elaboración, Empaque...

        [StringLength(100)]
        public string? Dependencia_proceso { get; set; }  // Qué depende de este cuarto

        [Required]
        public string Protocolo_acceso { get; set; } = string.Empty;

        public string? Protocolo_validacion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Fecha_ultima_validacion { get; set; }

        public int? Periodo_revalidacion_meses { get; set; }

        [StringLength(255)]
        public string? Documento_estandar_ref { get; set; }

        public string? Notas_adicionales { get; set; }

        //propiedades de navegacion
        public ICollection<Dispositivosdev> Dispositivos { get; set; } = new List<Dispositivosdev>();

    }
}