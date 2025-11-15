using AppDevicesMedical.DTOs;
using AppDevicesMedical.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
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

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await context.Usuarios.ToListAsync();
        }
        public async Task<string?> LoginAsync(LoginDto request)
        {
            // Buscar al usuario por número de empleado
            var usuario = await context.Usuarios
                // ✅ CAMBIO AÑADIDO: Incluir la propiedad de navegación Rol para acceder al Nombre_rol
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.NumeroEmpleado == request.NumeroEmpleado);

            if (usuario is null)
            {
                return null; // Usuario no encontrado
            }
            if (new PasswordHasher<Usuario>().VerifyHashedPassword(usuario, usuario.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null; // Contraseña incorrecta

            }

            return CrearToken(usuario);
        }

        public async Task<Usuario?> RegisterAsync(UsuarioDto request)
        {
            // Validación: evitar duplicados por número de empleado
            if (await context.Usuarios.AnyAsync(u => u.NumeroEmpleado == request.NumeroEmpleado))
            {
                return null; // Ya existe
            }

            // Crear instancia del modelo
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

            // Hashear la contraseña
            var passwordHasher = new PasswordHasher<Usuario>();
            usuario.PasswordHash = passwordHasher.HashPassword(usuario, request.Password!);

            // Guardar en base de datos
            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario?> UpdateAsync(int id, UsuarioDto request)
        {
            var usuario = await context.Usuarios.FindAsync(id);
            if (usuario is null)
            {
                return null; // Usuario no encontrado
            }

            // Actualizar campos básicos
            usuario.Nombres = request.Nombres;
            usuario.ApellidoPaterno = request.ApellidoPaterno;
            usuario.ApellidoMaterno = request.ApellidoMaterno;
            usuario.NumeroEmpleado = request.NumeroEmpleado;

            // Actualizar claves foráneas (permitiendo null)
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
            {
                return null;
            }

            context.Usuarios.Remove(usuario);
            await context.SaveChangesAsync();
            return true;
        }

        public string CrearToken(Usuario usuario)
        {
            // 1. Obtener el Nombre del Rol (ya cargado por el .Include en LoginAsync)
            // Si el rol es null por alguna razón, asignamos un valor seguro.
            string nombreRol = usuario.Rol?.Nombre_rol ?? "SinRol";

            var claims = new List<Claim>
            {
             new Claim(ClaimTypes.Name, usuario.NumeroEmpleado),
             new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),

             // ✅ CAMBIO AÑADIDO: Claim de Rol (para Autorización estándar [Authorize(Roles="...")] o Políticas)
             new Claim(ClaimTypes.Role, nombreRol),

             // ✅ CAMBIO AÑADIDO: Claim del ID del Rol (para Autorización basada en Permisos lógicos/DB)
             new Claim("IdRolDB", usuario.IdRol.ToString())
            };

            // Leer la clave desde AppSettings:Token
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