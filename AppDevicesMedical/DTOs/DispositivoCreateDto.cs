using AppDevicesMedical.Models;
using System.ComponentModel.DataAnnotations;

namespace AppDevicesMedical.DTOs
{
    public class DispositivoCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion_detallada { get; set; }
        public int? Id_categoria { get; set; }
        public int? Id_clase_riesgo { get; set; }
        public int? Id_tipo_dispositivo { get; set; }
        public int? Id_cuarto_requerido { get; set; }
        public bool Es_invasivo { get; set; } = false;
        public bool Requiere_biocompatibilidad { get; set; } = false;
        public bool Requiere_prueba_residuales { get; set; } = false;
        public string? Metodo_esterilizacion_req { get; set; }
        public string Estado_regulatorio { get; set; } = string.Empty;
        public DateTime Fecha_registro { get; set; }

        //// (Opcional) Nombres de navegación
        //public string? CategoriaNombre { get; set; }
        //public string? ClaseRiesgoNombre { get; set; }
        //public string? TipoDispositivoNombre { get; set; }
        //public string? CuartoRequeridoNombre { get; set; }

        // Método que convierte el DTO en entidad
        public Dispositivosdev ToEntity()
        {
            return new Dispositivosdev
            {
                Nombre = this.Nombre,
                Descripcion_detallada = this.Descripcion_detallada,
                Id_categoria = this.Id_categoria,
                Id_clase_riesgo = this.Id_clase_riesgo,
                Id_tipo_dispositivo = this.Id_tipo_dispositivo,
                Id_cuarto_requerido = this.Id_cuarto_requerido,
                Es_invasivo = this.Es_invasivo,
                Requiere_biocompatibilidad = this.Requiere_biocompatibilidad,
                Requiere_prueba_residuales = this.Requiere_prueba_residuales,
                Metodo_esterilizacion_req = this.Metodo_esterilizacion_req,
                Estado_regulatorio = this.Estado_regulatorio,
                Fecha_registro = this.Fecha_registro
            };
        }
    }
}

    
