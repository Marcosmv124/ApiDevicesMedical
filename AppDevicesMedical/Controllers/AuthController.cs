using AppDevicesMedical.Authorization;
using AppDevicesMedical.DTOs;
using AppDevicesMedical.Models;
using AppDevicesMedical.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppDevicesMedical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var usuario = await authService.GetByIdAsync(id);

            if (usuario is null)
                return NotFound($"Usuario con ID {id} no encontrado.");

            return Ok(usuario);
        }


        [Permiso("VER_TODOS_USUARIOS")]
        [HttpGet("all")]
        public async Task<ActionResult<List<Usuario>>> GetAll()
        {
            var usuarios = await authService.GetAllAsync();
            return Ok(usuarios);
        }
        ////[Permiso("Permiso:VER_USUARIO")]
        //[Permiso("VER_USUARIO")]
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Usuario>> GetById(int id)
        //{
        //    var usuario = await authService.GetByIdAsync(id);
        //    if (usuario is null)
        //        return NotFound($"Usuario con ID {id} no encontrado.");

        //    return Ok(usuario);
        //}

        //[Permiso("VER_TODOS_USUARIOS")]
        //[HttpGet("all")]
        //public async Task<ActionResult<List<Usuario>>> GetAll()
        //{
        //    var usuarios = await authService.GetAllAsync();
        //    return Ok(usuarios);
        //}

        //[Permiso("REGISTRAR_USUARIO")]
        [HttpPost("register")]
        public async Task<ActionResult<RegisterDto>> Register(RegisterDto request)
        {
            var user = await authService.RegisterAsync(request);
            if (user is null)
                return BadRequest("Este usuario ya existe");

            return Ok(user);
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            var errorMessage = await authService.LoginAsync(request);

            // Validar el error devuelto y responder con mensajes específicos
            if (errorMessage == "Empleado no encontrado")
                return BadRequest("Número de empleado inválido.");

            if (errorMessage == "Contraseña incorrecta")
                return BadRequest("Contraseña incorrecta.");

            // Si la autenticación fue exitosa, devolver el token
            return Ok(errorMessage);  // Devuelve el token generado (JWT)
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

        //[Permiso("ELIMINAR_USUARIO")]
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var resultado = await authService.DeleteAsync(id);

        //    if (resultado is null)
        //    {
        //        return NotFound($"Usuario con ID {id} no encontrado para eliminar.");
        //    }

        //    if (resultado == false)
        //    {
        //        return BadRequest("No se pudo eliminar el usuario.");
        //    }

        //    return NoContent(); // 204
        //}
        [Permiso("ELIMINAR_USUARIO")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // 1. Obtener el ID del usuario logueado desde los Claims
            // Nota: Ajusta "ClaimTypes.NameIdentifier" si usas otro nombre para el ID en tu token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2. Validar que no se esté borrando a sí mismo
            if (userIdClaim != null && int.Parse(userIdClaim) == id)
            {
                return BadRequest("No puedes eliminar tu propia cuenta mientras estás logueado.");
            }

            // --- El resto de tu código sigue igual ---
            var resultado = await authService.DeleteAsync(id);

            if (resultado is null)
            {
                return NotFound($"Usuario con ID {id} no encontrado para eliminar.");
            }

            if (resultado == false)
            {
                return BadRequest("No se pudo eliminar el usuario.");
            }

            return NoContent();
        }

        [Permiso("ENDPOINT_AUTENTICADO")]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("Ya estas autenticado paul");
        }
        [HttpGet("me")]
        [Authorize] // 🔒 Importante: Solo usuarios logueados con Token válido
        public async Task<IActionResult> GetMe()
        {
            // 1. Extraer el ID del usuario desde el Token (ClaimTypes.NameIdentifier)
            // Nota: En tu CrearToken usaste: new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                return Unauthorized("Token inválido");
            }

            int userId = int.Parse(userIdClaim.Value);

            // 2. Llamar al servicio
            var userProfile = await authService.GetLoggedUserAsync(userId);

            if (userProfile is null)
            {
                return NotFound("Usuario no encontrado");
            }

            // 3. Devolver el objeto completo al Front
            return Ok(userProfile);
        }
    }
}