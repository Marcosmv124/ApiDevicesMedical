using System.Security.Claims;

namespace AppDevicesMedical.Helpers
{
    public static class PermisoHelper
    {
        public static bool TienePermiso(ClaimsPrincipal user, string permiso)
        {
            return user.Claims.Any(c => c.Type == "Permiso" && c.Value == permiso);
        }
    }

}
