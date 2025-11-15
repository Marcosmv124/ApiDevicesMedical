using Microsoft.AspNetCore.Authorization;

namespace AppDevicesMedical
{
    public class PermisoRequerido : IAuthorizationRequirement
    {
        public string Permiso { get; }

        public PermisoRequerido(string permiso)
        {
            // El permiso lógico que se requiere (ej: "GESTIONAR_ROLES")
            Permiso = permiso;
        }
    }
}
