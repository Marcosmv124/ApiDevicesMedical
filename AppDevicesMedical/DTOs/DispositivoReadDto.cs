namespace AppDevicesMedical.Controllers
{
    public class DispositivoReadDto
    {
        //IDs y Campos Primarios (para editar/eliminar y mostrar en la tabla)
        public int Id_dispositivo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion_detallada { get; set; }

        //Campos de Relación Expandida (para mostrar en la tabla)
        public string? NombreCategoria { get; set; } // <-- CATEGORÍA
        public string? NombreClaseRiesgo { get; set; } // <-- CLASE (Asumiendo que quieres el nombre aquí)

        // Campos que ya tienes en el DTO de creación, pero que se muestran
        public string Estado_regulatorio { get; set; } = string.Empty; // <-- ESTADO (Si se mapea desde el modelo)
        public DateTime Fecha_registro { get; set; } // <-- FECHA
    }
}
