namespace AppDevicesMedical.DTOs
{
    public class GuardarRolConPermisosDto
    {
        public int? IdRol { get; set; }    // null o 0 = nuevo rol
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public List<PermisoRolDto> Permisos { get; set; } = new();
    }
}
