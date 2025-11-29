using AppDevicesMedical.DTOs;
using AppDevicesMedical.Models;

namespace AppDevicesMedical.Services
{
    public interface IAuthService
    {
        Task<Usuario?> RegisterAsync(RegisterDto request);       
        Task<string?> LoginAsync(LoginDto request);
        Task<Usuario?>UpdateAsync(int id, UsuarioDto request);
        Task<bool?> DeleteAsync(int id);
        // 🔍 Nuevos métodos para lectura
        Task<Usuario?> GetByIdAsync(int id);
        Task<List<Usuario>> GetAllAsync();
        Task<UserProfileDto?> GetLoggedUserAsync(int userId);


    }
}
