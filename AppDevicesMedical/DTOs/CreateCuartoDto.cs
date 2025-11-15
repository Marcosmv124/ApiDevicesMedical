using AppDevicesMedical.Models;
using System.ComponentModel.DataAnnotations;

namespace AppDevicesMedical.DTOs
{
    public class CreateCuartoDto
    {
        // --- Identificación básica ---
        [Required, StringLength(50)]
        public string Nombre_cuarto { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Clase_cuarto { get; set; } = string.Empty;

        // --- 20 características generales agrupadas ---

        // 1. Dimensiones y capacidad
        public decimal? Dimensiones_m2 { get; set; }
        public int? Capacidad_personas { get; set; }
        public int? Capacidad_produccion { get; set; }

        // 2. Condiciones ambientales
        public bool Control_temperatura { get; set; } = false;
        public bool Control_humedad { get; set; } = false;
        public string? Espec_flujo_aire { get; set; }
        public int? Capacidad_hvac_cfm { get; set; }
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
        public decimal? Presion_diferencial_pa { get; set; }
        public decimal? Nivel_ruido_db { get; set; }
        public decimal? Iluminacion_lux { get; set; }
        public bool Filtracion_hepa { get; set; } = false;
        public string? Nivel_limpieza_iso { get; set; }

        // --- Campos de control y documentación ---
        public string? Estado_actual { get; set; } = "Vacío";
        public string? Etapa_proceso { get; set; }
        public string? Dependencia_proceso { get; set; }

        // CORRECCIÓN: Se añade un StringLength grande para capturar descripciones largas
        // en el DTO antes de que fallen en la DB (donde Protocolo_acceso es nvarchar(max) o similar).
        [Required, StringLength(500)]
        public string Protocolo_acceso { get; set; } = string.Empty;

        public string? Protocolo_validacion { get; set; }
        public DateTime? Fecha_ultima_validacion { get; set; }
        public int? Periodo_revalidacion_meses { get; set; }
        public string? Documento_estandar_ref { get; set; }
        public string? Notas_adicionales { get; set; }

        // Método que convierte el DTO a la entidad Cuarto
        public Cuarto ToEntity()
        {
            return new Cuarto
            {
                Nombre_cuarto = this.Nombre_cuarto,
                Clase_cuarto = this.Clase_cuarto,
                Dimensiones_m2 = this.Dimensiones_m2,
                Capacidad_personas = this.Capacidad_personas,
                Capacidad_produccion = this.Capacidad_produccion,
                Control_temperatura = this.Control_temperatura,
                Control_humedad = this.Control_humedad,
                Espec_flujo_aire = this.Espec_flujo_aire,
                Capacidad_hvac_cfm = this.Capacidad_hvac_cfm,
                Tipo_acondicionamiento = this.Tipo_acondicionamiento,
                Tiempo_reposicion_aire_min = this.Tiempo_reposicion_aire_min,
                Limite_contaminacion_ufc = this.Limite_contaminacion_ufc,
                Contaminacion_actual_ufc = this.Contaminacion_actual_ufc,
                Limite_particulas_no_viables = this.Limite_particulas_no_viables,
                Conteo_particulas_no_viables = this.Conteo_particulas_no_viables,
                Limite_particulas_viables = this.Limite_particulas_viables,
                Conteo_particulas_viables = this.Conteo_particulas_viables,
                Presion_diferencial_pa = this.Presion_diferencial_pa,
                Nivel_ruido_db = this.Nivel_ruido_db,
                Iluminacion_lux = this.Iluminacion_lux,
                Filtracion_hepa = this.Filtracion_hepa,
                Nivel_limpieza_iso = this.Nivel_limpieza_iso,
                Estado_actual = this.Estado_actual,
                Etapa_proceso = this.Etapa_proceso,
                Dependencia_proceso = this.Dependencia_proceso,
                Protocolo_acceso = this.Protocolo_acceso,
                Protocolo_validacion = this.Protocolo_validacion,
                Fecha_ultima_validacion = this.Fecha_ultima_validacion,
                Periodo_revalidacion_meses = this.Periodo_revalidacion_meses,
                Documento_estandar_ref = this.Documento_estandar_ref,
                Notas_adicionales = this.Notas_adicionales
            };
        }
    }
}