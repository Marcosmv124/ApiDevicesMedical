using AppDevicesMedical.Models;
using Microsoft.AspNetCore.Identity;

namespace AppDevicesMedical.Services
{
    //public static class DbInitializer
    //{
    //    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    //    {
    //        // Obtener servicios necesarios
    //        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    //        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    //        // IMPORTANTE: Ajusta el namespace si tu DbContext no está en AppDevicesMedical.Data
    //        var context = serviceProvider.GetRequiredService<MedicalDevicesDbContext>();

    //        // =======================================================
    //        // 1. Inicializar Roles de IDENTITY (AspNetRoles)
    //        // =======================================================
    //        string[] identityRoles = { "Admin", "Consultor" };
    //        foreach (var roleName in identityRoles)
    //        {
    //            if (!await roleManager.RoleExistsAsync(roleName))
    //            {
    //                await roleManager.CreateAsync(new IdentityRole(roleName));
    //            }
    //        }

    //        // =======================================================
    //        // 2. Inicializar Tablas de Catálogo Manual (Status y Rol)
    //        // =======================================================
    //        if (!context.Status.Any())
    //        {
    //            context.Status.AddRange(
    //                new Status { Id_status = 1, Nombre_status = "Activo" },
    //                new Status { Id_status = 2, Nombre_status = "Inactivo" }
    //            );
    //        }
    //        if (!context.Rol.Any())
    //        {
    //            context.Rol.AddRange(
    //                new Rol { Id_rol = 1, Nombre_rol = "Administrador" },
    //                new Rol { Id_rol = 2, Nombre_rol = "Consultor" }
    //            );
    //        }

    //        await context.SaveChangesAsync();

    //        // =======================================================
    //        // 3. Crear un Usuario Administrador Inicial
    //        // =======================================================
    //        if (await userManager.FindByEmailAsync("admin@medical.com") == null)
    //        {
    //            var adminUser = new ApplicationUser
    //            {
    //                UserName = "admin@medical.com",
    //                Email = "admin@medical.com",
    //                Nombres = "Super",
    //                ApellidoPaterno = "Admin",
    //                ApellidoMaterno = "Global",
    //                NumeroEmpleado = "ADMN001",
    //                IdStatus = 1, // Usar el ID de 'Activo'
    //                FechaCreacion = DateTime.Today
    //            };

    //            var result = await userManager.CreateAsync(adminUser, "SecureAdminPass123!");
    //            if (result.Succeeded)
    //            {
    //                await userManager.AddToRoleAsync(adminUser, "Admin");
    //            }
    //        }
    //    }
    //}
}
