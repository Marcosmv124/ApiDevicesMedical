namespace AppDevicesMedical.DTOs
{
    public class ClaseRiesgoDto
    {
        public int IdClaseRiesgo { get; set; }
        public string NombreClase { get; set; }
        public string? Descripcion { get; set; }

        public List<DispositivoDto> Dispositivos { get; set; } = new();
    }

}
