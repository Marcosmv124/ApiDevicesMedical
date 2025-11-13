using AppDevicesMedical.DTOs;
using AppDevicesMedical.Models;
using AppDevicesMedical.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppDevicesMedical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var usuario = await authService.GetByIdAsync(id);
            if (usuario is null)
                return NotFound($"Usuario con ID {id} no encontrado.");

            return Ok(usuario);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Usuario>>> GetAll()
        {
            var usuarios = await authService.GetAllAsync();
            return Ok(usuarios);
        }
        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register(UsuarioDto request)
        {

            var user = await authService.RegisterAsync(request);
            if (user is null)
                return BadRequest("Este usuario ya existe");
            
            
            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            var token = await authService.LoginAsync(request); 
            if(token is null) return BadRequest("Invalido numero de empleado o Contrasena ");  

            return Ok(token);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioDto request)
        {
            // Validación extra: el id de la URL debe coincidir con el del cuerpo
            if (id != request.IdUsuario)
            {
                return BadRequest("El ID de la URL no coincide con el del cuerpo.");
            }

            // Llama al servicio
            var user = await authService.UpdateAsync(id, request);

            // Si el servicio devuelve null, es porque el usuario NO FUE ENCONTRADO
            if (user is null)
            {
                return NotFound($"Usuario con ID {id} no encontrado para modificar.");
            }

            // Modificación exitosa
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Llama al servicio
            var resultado = await authService.DeleteAsync(id);

            // Si el servicio devolvió null → el usuario no existía
            if (resultado is null)
            {
                return NotFound($"Usuario con ID {id} no encontrado para eliminar.");
            }

            // Si devolvió false → hubo un error al intentar eliminar
            if (resultado == false)
            {
                return BadRequest("No se pudo eliminar el usuario.");
            }

            // Si devolvió true → eliminación exitosa
            return NoContent(); // 204
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("Ya estas autenticado paul");
        }
    }
}
