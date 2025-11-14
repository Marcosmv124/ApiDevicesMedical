namespace AppDevicesMedical.DTOs
{
    public class DispositivoDto
    {
        public int IdDispositivo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool EsInvasivo { get; set; }
        public string? MetodoEsterilizacionReq { get; set; }
    }
}
