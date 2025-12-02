using AppDevicesMedical.DTOs;
using AppDevicesMedical.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppDevicesMedical.Services
{
    public class AuthService(MedicalDevicesDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await context.Usuarios.FindAsync(id);
        }
        public async Task<UserProfileDto?> GetLoggedUserAsync(int userId)
        {
            var usuario = await context.Usuarios
                .Include(u => u.Rol) // Incluimos el rol para mostrar el nombre
                .FirstOrDefaultAsync(u => u.IdUsuario == userId);

            if (usuario is null) return null;

            // Mapeamos a DTO para seguridad
            return new UserProfileDto
            {
                IdUsuario = usuario.IdUsuario,
                NumeroEmpleado = usuario.NumeroEmpleado,
                Nombres = usuario.Nombres,
                ApellidoPaterno = usuario.ApellidoPaterno,
                ApellidoMaterno = usuario.ApellidoMaterno,
                NombreRol = usuario.Rol?.Nombre_rol ?? "Sin Rol"
            };
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await context.Usuarios.ToListAsync();
        }
      
        public async Task<string?> LoginAsync(LoginDto request)
        {
            var usuario = await context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.NumeroEmpleado == request.NumeroEmpleado);

            // 1. Validar si existe
            if (usuario is null) return "Empleado no encontrado";

            // 2. VALIDAR BLOQUEO TEMPORAL (Los 5 minutos)
            if (usuario.BloqueoHasta.HasValue && usuario.BloqueoHasta > DateTime.Now)
            {
                var segundosRestantes = (int)(usuario.BloqueoHasta.Value - DateTime.Now).TotalSeconds;
                return $"Usuario bloqueado temporalmente. Espera {segundosRestantes/60} Minutos.";
            }

            // 3. VALIDAR STATUS BLOQUEADO (Desde BD, ej: Baja o Suspendido)
            // Asumiendo que IdStatus != 1 significa que NO está activo
            if (usuario.IdStatus != 0/*STATUS 0 es el activo*/)
            {
                return "Tu cuenta está desactivada o dada de baja.";
            }

            // 4. Verificar Contraseña
            var verificationResult = new PasswordHasher<Usuario>()
                .VerifyHashedPassword(usuario, usuario.PasswordHash, request.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                // --- LÓGICA DE BLOQUEO ---
                usuario.IntentosFallidos++;

                if (usuario.IntentosFallidos >= 3) // A los 3 intentos...
                {
                    usuario.BloqueoHasta = DateTime.Now.AddMinutes(6); // Bloqueo 2 min
                    usuario.IntentosFallidos = 0; // Reiniciamos contador (opcional)
                }

                await context.SaveChangesAsync(); // Guardamos el fallo en BD
                return "Contraseña incorrecta";
            }

            // 5. ÉXITO: Limpiar bloqueos previos si entró bien
            if (usuario.IntentosFallidos > 0 || usuario.BloqueoHasta != null)
            {
                usuario.IntentosFallidos = 0;
                usuario.BloqueoHasta = null;
                await context.SaveChangesAsync();
            }

            // Obtener permisos y generar token
            var permisos = await context.RolPermisos
                .Where(rp => rp.IdRol == usuario.IdRol)
                .Select(rp => rp.Permiso.Nombre)
                .ToListAsync();

            return CrearToken(usuario, permisos);
        }

        public async Task<Usuario?> RegisterAsync(RegisterDto request)
        {
            if (await context.Usuarios.AnyAsync(u => u.NumeroEmpleado == request.NumeroEmpleado))
                return null;

            var usuario = new Usuario
            {
                Nombres = request.Nombres,
                ApellidoPaterno = request.ApellidoPaterno,
                ApellidoMaterno = request.ApellidoMaterno,
                NumeroEmpleado = request.NumeroEmpleado,
                IdRol = request.IdRol,
                IdEspecialidad = request.IdEspecialidad,
                IdStatus = request.IdStatus,
                FechaCreacion = DateTime.UtcNow
            };

            var passwordHasher = new PasswordHasher<Usuario>();
            usuario.PasswordHash = passwordHasher.HashPassword(usuario, request.Password!);

            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario?> UpdateAsync(int id, UsuarioDto request)
        {
            var usuario = await context.Usuarios.FindAsync(id);
            if (usuario is null)
                return null;

            usuario.Nombres = request.Nombres;
            usuario.ApellidoPaterno = request.ApellidoPaterno;
            usuario.ApellidoMaterno = request.ApellidoMaterno;
            usuario.NumeroEmpleado = request.NumeroEmpleado;
            usuario.IdRol = request.IdRol;
            usuario.IdStatus = request.IdStatus;
            usuario.IdEspecialidad = request.IdEspecialidad;

            await context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            var usuario = await context.Usuarios.FindAsync(id);
            if (usuario is null)
                return null;

            context.Usuarios.Remove(usuario);
            await context.SaveChangesAsync();
            return true;
        }
        public string CrearToken(Usuario usuario, List<string> permisos)
        {
            string nombreRol = usuario.Rol?.Nombre_rol ?? "SinRol";

            var claims = new List<Claim>
          {
              // Incluir el Nombre del usuario
               new Claim(ClaimTypes.Name, usuario.NumeroEmpleado), // NumeroEmpleado como el nombre de usuario
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()), // IdUsuario
                 new Claim(ClaimTypes.Role, nombreRol), // Rol del usuario
                   new Claim("IdRolDB", usuario.IdRol.ToString()), // IdRol de la base de datos
        
                  // Incluir el Nombre completo del usuario
                      new Claim("Nombre", usuario.Nombres)  // Agregar el Nombre completo aquí
            };

            // ✅ Agregar permisos como claims
            foreach (var permiso in permisos)
            {
                claims.Add(new Claim("Permiso", permiso));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration["AppSettings:Issuer"],
                audience: configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
#region
//public string CrearToken(Usuario usuario, List<string> permisos)
//{
//    string nombreRol = usuario.Rol?.Nombre_rol ?? "SinRol";

//    var claims = new List<Claim>
//    {
//        new Claim(ClaimTypes.Name, usuario.NumeroEmpleado),
//        new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
//        new Claim(ClaimTypes.Role, nombreRol),
//        new Claim("IdRolDB", usuario.IdRol.ToString())
//    };

//    // ✅ Agregar permisos como claims
//    foreach (var permiso in permisos)
//    {
//        claims.Add(new Claim("Permiso", permiso));
//    }

//    var key = new SymmetricSecurityKey(
//        Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

//    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

//    var tokenDescriptor = new JwtSecurityToken(
//        issuer: configuration["AppSettings:Issuer"],
//        audience: configuration["AppSettings:Audience"],
//        claims: claims,
//        expires: DateTime.UtcNow.AddHours(2),
//        signingCredentials: creds
//    );

//    return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
//}

//public async Task<Usuario?> RegisterAsync(RegisterDto request)
//{
//    Validar si ya existe el número de empleado
//    if (await context.Usuarios.AnyAsync(u => u.NumeroEmpleado == request.NumeroEmpleado))
//        return null;

//    var usuario = new Usuario
//    {
//        Nombres = request.Nombres,
//        ApellidoPaterno = request.ApellidoPaterno,
//        ApellidoMaterno = request.ApellidoMaterno,
//        NumeroEmpleado = request.NumeroEmpleado,
//        FechaCreacion = DateTime.UtcNow,

//        ---AQUÍ CREAMOS LAS DEPENDENCIAS AL VUELO-- -
//         Al instanciar "new Rol",
//        EF Core insertará primero el rol en la BD
//         y usará ese nuevo ID para el usuario.

//        Rol = new Rol
//        {
//            Nombre_rol = "Super Admin" // <--- Aquí definimos el nombre exacto
//        },

//        StatusUsuario = new Status
//        {
//             Revisa si en tu clase Status la propiedad se llama 'Nombre' o 'Descripcion'
//            Nombre_status = "Activo"
//        },

//        Especialidad = new Especialidad
//        {
//            Nom_Especialidad = "Sistemas"
//        }
//    };

//    Encriptar contraseña
//var passwordHasher = new PasswordHasher<Usuario>();
//    usuario.PasswordHash = passwordHasher.HashPassword(usuario, request.Password);

//    context.Usuarios.Add(usuario);
//    await context.SaveChangesAsync(); // Esto guarda Rol, Status, Especialidad y Usuario en una sola transacción.

//    return usuario;
//}

#endregion
#region
//public async Task<string?> LoginAsync(LoginDto request)
//{
//    var usuario = await context.Usuarios
//        .Include(u => u.Rol)
//        .FirstOrDefaultAsync(u => u.NumeroEmpleado == request.NumeroEmpleado);

//    // Si el usuario no existe
//    if (usuario is null)
//        return "Empleado no encontrado";  // Detalles de error específicos

//    // Verificar la contraseña
//    if (new PasswordHasher<Usuario>().VerifyHashedPassword(usuario, usuario.PasswordHash, request.Password)
//        == PasswordVerificationResult.Failed)
//        return "Contraseña incorrecta";  // Detalles de error específicos

//    // Obtener permisos del rol
//    var permisos = await context.RolPermisos
//        .Where(rp => rp.IdRol == usuario.IdRol)
//        .Select(rp => rp.Permiso.Nombre)
//        .ToListAsync();

//    // Generar el token JWT
//    return CrearToken(usuario, permisos);
//}
#endregion