using AppDevicesMedical.Authorization;
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
        //[Permiso("Permiso:VER_USUARIO")]
        [Permiso("VER_USUARIO")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var usuario = await authService.GetByIdAsync(id);
            if (usuario is null)
                return NotFound($"Usuario con ID {id} no encontrado.");

            return Ok(usuario);
        }

        //[Authorize(Policy = "Permiso:VER_TODOS_USUARIOS")]
        [HttpGet("all")]
        public async Task<ActionResult<List<Usuario>>> GetAll()
        {
            var usuarios = await authService.GetAllAsync();
            return Ok(usuarios);
        }

        [Permiso("REGISTRAR_USUARIO")]
        [HttpPost("register")]
        public async Task<ActionResult<UsuarioDto>> Register(UsuarioDto request)
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
            if (token is null) return BadRequest("Invalido numero de empleado o Contrasena ");

            return Ok(token);
        }

        [Permiso("EDITAR_USUARIO")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioDto request)
        {
            if (id != request.IdUsuario)
            {
                return BadRequest("El ID de la URL no coincide con el del cuerpo.");
            }

            var user = await authService.UpdateAsync(id, request);

            if (user is null)
            {
                return NotFound($"Usuario con ID {id} no encontrado para modificar.");
            }

            return Ok(user);
        }

        [Permiso("ELIMINAR_USUARIO")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await authService.DeleteAsync(id);

            if (resultado is null)
            {
                return NotFound($"Usuario con ID {id} no encontrado para eliminar.");
            }

            if (resultado == false)
            {
                return BadRequest("No se pudo eliminar el usuario.");
            }

            return NoContent(); // 204
        }

        [Permiso("ENDPOINT_AUTENTICADO")]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("Ya estas autenticado paul");
        }
    }
}