using AppDevicesMedical.Models;
using System.ComponentModel.DataAnnotations;

namespace AppDevicesMedical.DTOs
{
    public class DispositivoCreateDto
    {
        public int Id_dispositivo { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion_detallada { get; set; }
        public int? Id_categoria { get; set; }
        public int? Id_clase_riesgo { get; set; }
        public int? Id_tipo_dispositivo { get; set; }
        public int? Id_cuarto_requerido { get; set; }

        // CAMBIO 1: Agregamos el ID del método (Esto es lo que el Front envía al guardar)
       // public int? IdMetodoEsterilizacion { get; set; }

        public bool Es_invasivo { get; set; } = false;
        public bool Requiere_biocompatibilidad { get; set; } = false;
        public bool Requiere_prueba_residuales { get; set; } = false;

         
         public string? Metodo_esterilizacion_req { get; set; }

        public string Estado_regulatorio { get; set; } = string.Empty;
        public DateTime Fecha_registro { get; set; }

        // --- CAMPOS DE SOLO LECTURA (Para mostrar en la tabla del Front) ---
        public string? NombreCategoria { get; set; }
        public string? NombreClaseRiesgo { get; set; }

        // 👇 CAMBIO 3: Agregamos el nombre para que en la tabla diga "Rayos Gamma" y no "2"
        public string? NombreMetodoEsterilizacion { get; set; }

        // Método para convertir DTO a Entity (BD)
        public Dispositivosdev ToEntity()
        {
            return new Dispositivosdev
            {
                // El ID NO se asigna aquí porque es autoincremental al crear
                Nombre = this.Nombre,
                Descripcion_detallada = this.Descripcion_detallada,
                Id_categoria = this.Id_categoria,
                Id_clase_riesgo = this.Id_clase_riesgo,
                Id_tipo_dispositivo = this.Id_tipo_dispositivo,
                Id_cuarto_requerido = this.Id_cuarto_requerido,

                // 👇 CAMBIO 4: Asignamos el ID nuevo a la entidad
               // IdMetodoEsterilizacion = this.IdMetodoEsterilizacion,

                Metodo_esterilizacion_req = this.Metodo_esterilizacion_req, // ❌ Ya no se usa

                Es_invasivo = this.Es_invasivo,
                Requiere_biocompatibilidad = this.Requiere_biocompatibilidad,
                Requiere_prueba_residuales = this.Requiere_prueba_residuales,
                Estado_regulatorio = this.Estado_regulatorio,
                Fecha_registro = this.Fecha_registro
            };
        }
    }
}