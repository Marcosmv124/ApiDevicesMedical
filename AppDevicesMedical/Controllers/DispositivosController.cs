using AppDevicesMedical.Authorization;
using AppDevicesMedical.DTOs;
using AppDevicesMedical.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDevicesMedical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispositivosController : Controller
    {
        private readonly MedicalDevicesDbContext _context;
        
        
        public DispositivosController(MedicalDevicesDbContext context)
        {
            _context = context;
        }
        [Permiso("VER_DISPOSITIVOS")]
        // GET: DispositivosController // GET: api/dispositivos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DispositivoCreateDto>>> GetDispositivos()
        {
            try
            {
                var dispositivos = await _context.Dispositivodv
                .OrderBy(c => c.Id_dispositivo)
                .ToListAsync();

                // Si tu DTO es idéntico al modelo, no necesitas mapear nada
                return Ok(dispositivos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        [Permiso("CREAR_DISPOSITIVOS")]
        // POST: api/Dispositivos
        [HttpPost]
        public async Task<ActionResult<DispositivoCreateDto>> PostDispositivo(DispositivoCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Convertir DTO a entidad
                var dispositivo = createDto.ToEntity();

                _context.Dispositivodv.Add(dispositivo);
                await _context.SaveChangesAsync();

                // (Opcional) devolver DTO limpio, sin ID ni navegación
                return Ok(createDto);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al crear el dispositivo: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}
