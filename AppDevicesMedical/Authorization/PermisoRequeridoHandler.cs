using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using AppDevicesMedical.Models; // Necesario para MedicalDevicesDbContext

namespace AppDevicesMedical.Authorization
{
    public class PermisoRequeridoHandler : AuthorizationHandler<PermisoRequerido>
    {
        // El contexto de la DB se inyecta por constructor
        private readonly MedicalDevicesDbContext _context;

        public PermisoRequeridoHandler(MedicalDevicesDbContext context)
        {
            _context = context;
        }

        // ✅ FIRMA CORRECTA: AuthorizationHandlerContext es el tipo requerido por la clase base
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermisoRequerido requirement)
        {
            // 1. Obtener el ID del Rol del Claim (desde el token JWT)
            var idRolClaim = context.User.FindFirst("IdRolDB");

            if (idRolClaim == null || !int.TryParse(idRolClaim.Value, out int idRol))
            {
                return; // No se puede autorizar si no hay rol válido
            }

            // 2. CONSULTA DINÁMICA A LA BASE DE DATOS
            // Usamos _context para consultar RolPermisos y Permisos
            var tienePermiso = await _context.RolPermisos
                .Where(rp => rp.IdRol == idRol)
                .Join(_context.Permisos,
                      rp => rp.IdPermiso,
                      p => p.IdPermiso,
                      (rp, p) => p)
                .AnyAsync(p => p.Nombre == requirement.Permiso);

            // 3. Si se encuentra el permiso, se cumple el requisito
            if (tienePermiso)
            {
                context.Succeed(requirement);
            }
        }
    }
}