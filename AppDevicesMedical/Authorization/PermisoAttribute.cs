using Microsoft.AspNetCore.Authorization;

namespace AppDevicesMedical.Authorization
{
    public class PermisoAttribute : AuthorizeAttribute
    {
        public PermisoAttribute(string permiso)
        {
            Policy = $"Permiso:{permiso}";
        }
    }

}
