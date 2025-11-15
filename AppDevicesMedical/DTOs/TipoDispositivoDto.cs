namespace AppDevicesMedical.DTOs
{
    public class TipoDispositivoDto
    {
        public int Id_tipo { get; set; }
        public string Nombre_tipo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        public List<DispositivoDto> Dispositivos { get; set; } = new();
    }
}
