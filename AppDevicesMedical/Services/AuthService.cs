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

            return CrearToken(usuario); // ✅ Ahora sí coincide el tipo

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
            usuario.IdRol = request.IdRol; // Si decides permitir null aquí, cambia a int?
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
                return null; // ⬅️ Aquí sí tiene sentido devolver null
            }

            context.Usuarios.Remove(usuario);
            await context.SaveChangesAsync();
            return true; // Eliminación exitosa
        }

        public string CrearToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
             new Claim(ClaimTypes.Name, usuario.NumeroEmpleado),
              new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
             // Puedes agregar más claims como rol, especialidad, etc.
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
