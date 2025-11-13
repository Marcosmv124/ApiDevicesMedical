using AppDevicesMedical.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppDevicesMedical.Services
{
    //public class TokenService : ITokenService
    //{
    //    private readonly IConfiguration _config;

    //    public TokenService(IConfiguration config)
    //    {
    //        _config = config;
    //    }

    //    public string CreateToken(ApplicationUser user, List<string> roles)
    //    {
    //        // 1. Definición de Claims (Información dentro del Token)
    //        var claims = new List<Claim>
    //        {
    //            new Claim(ClaimTypes.NameIdentifier, user.Id),
    //            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
    //            new Claim("empleado_id", user.NumeroEmpleado) // Claim personalizado
    //        };

    //        // 2. Agregar roles al token
    //        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

    //        // 3. Configuración de la Firma
    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]
    //                    ?? throw new InvalidOperationException("JWT Key not configured")));

    //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //        // 4. Descriptor del Token
    //        // Usa SecurityTokenDescriptor directamente (asumiendo que eliminaste el paquete conflictivo)
    //        var tokenDescriptor = new SecurityTokenDescriptor
    //        {
    //            Subject = new ClaimsIdentity(claims),
    //            Expires = DateTime.Now.AddDays(7), // Token válido por 7 días
    //            SigningCredentials = creds,
    //            Issuer = _config["Jwt:Issuer"],
    //            Audience = _config["Jwt:Audience"]
    //        };

    //        // 5. Creación y Serialización
    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var token = tokenHandler.CreateToken(tokenDescriptor);

    //        // Retorna el token JWT como string
    //        return tokenHandler.WriteToken(token);
    //    }
    //}
}