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

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await context.Usuarios.ToListAsync();
        }

        public async Task<string?> LoginAsync(LoginDto request)
        {
            var usuario = await context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.NumeroEmpleado == request.NumeroEmpleado);

            if (usuario is null)
                return null;

            if (new PasswordHasher<Usuario>().VerifyHashedPassword(usuario, usuario.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
                return null;

            // 🔑 Obtener permisos del rol
            var permisos = await context.RolPermisos
                .Where(rp => rp.IdRol == usuario.IdRol)
                .Select(rp => rp.Permiso.Nombre)
                .ToListAsync();

            return CrearToken(usuario, permisos);
        }

        public async Task<Usuario?> RegisterAsync(UsuarioDto request)
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
                new Claim(ClaimTypes.Name, usuario.NumeroEmpleado),
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, nombreRol),
                new Claim("IdRolDB", usuario.IdRol.ToString())
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