using AppDevicesMedical.Models;

namespace AppDevicesMedical.DTOs
{
    public class CuartoDto
    {
        // --- Identificación básica ---
        public int Id_cuarto { get; set; }
        public string Nombre_cuarto { get; set; } = string.Empty;
        public string Clase_cuarto { get; set; } = string.Empty;

        // --- 20 características generales agrupadas ---

        // 1. Dimensiones y capacidad
        public decimal? Dimensiones_m2 { get; set; }
        public int? Capacidad_personas { get; set; }
        public int? Capacidad_produccion { get; set; }

        // 2. Condiciones ambientales
        public bool Control_temperatura { get; set; }
        public bool Control_humedad { get; set; }
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
        public bool Filtracion_hepa { get; set; }
        public string? Nivel_limpieza_iso { get; set; }

        // --- Campos de control y documentación ---
        public string? Estado_actual { get; set; }
        public string? Etapa_proceso { get; set; }
        public string? Dependencia_proceso { get; set; }
        public string Protocolo_acceso { get; set; } = string.Empty;
        public string? Protocolo_validacion { get; set; }
        public DateTime? Fecha_ultima_validacion { get; set; }
        public int? Periodo_revalidacion_meses { get; set; }
        public string? Documento_estandar_ref { get; set; }
        public string? Notas_adicionales { get; set; }
        // Constructor que recibe la entidad Cuarto
        public CuartoDto(Cuarto c)
        {
            Id_cuarto = c.Id_cuarto;
            Nombre_cuarto = c.Nombre_cuarto;
            Clase_cuarto = c.Clase_cuarto;
            Dimensiones_m2 = c.Dimensiones_m2;
            Capacidad_personas = c.Capacidad_personas;
            Capacidad_produccion = c.Capacidad_produccion;
            Control_temperatura = c.Control_temperatura;
            Control_humedad = c.Control_humedad;
            Espec_flujo_aire = c.Espec_flujo_aire;
            Capacidad_hvac_cfm = c.Capacidad_hvac_cfm;
            Tipo_acondicionamiento = c.Tipo_acondicionamiento;
            Tiempo_reposicion_aire_min = c.Tiempo_reposicion_aire_min;
            Limite_contaminacion_ufc = c.Limite_contaminacion_ufc;
            Contaminacion_actual_ufc = c.Contaminacion_actual_ufc;
            Limite_particulas_no_viables = c.Limite_particulas_no_viables;
            Conteo_particulas_no_viables = c.Conteo_particulas_no_viables;
            Limite_particulas_viables = c.Limite_particulas_viables;
            Conteo_particulas_viables = c.Conteo_particulas_viables;
            Presion_diferencial_pa = c.Presion_diferencial_pa;
            Nivel_ruido_db = c.Nivel_ruido_db;
            Iluminacion_lux = c.Iluminacion_lux;
            Filtracion_hepa = c.Filtracion_hepa;
            Nivel_limpieza_iso = c.Nivel_limpieza_iso;
            Estado_actual = c.Estado_actual;
            Etapa_proceso = c.Etapa_proceso;
            Dependencia_proceso = c.Dependencia_proceso;
            Protocolo_acceso = c.Protocolo_acceso;
            Protocolo_validacion = c.Protocolo_validacion;
            Fecha_ultima_validacion = c.Fecha_ultima_validacion;
            Periodo_revalidacion_meses = c.Periodo_revalidacion_meses;
            Documento_estandar_ref = c.Documento_estandar_ref;
            Notas_adicionales = c.Notas_adicionales;
        }
    }
}
