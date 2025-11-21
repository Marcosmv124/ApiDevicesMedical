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
        [Permiso("VER_DISPOSITIVOS")]
        // GET: api/Dispositivos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetDispositivo(int id)
        {
            try
            {
                var dispositivo = await _context.Dispositivodv
                    .Include(d => d.Categoria)
                    .Include(d => d.ClaseRiesgo)
                    .Include(d => d.TipoDispositivo)
                    .Include(d => d.CuartoRequerido)
                    .FirstOrDefaultAsync(c => c.Id_dispositivo == id);

                if (dispositivo is null)
                    return NotFound($"No se encontró el dispositivo con ID {id}");

                // Proyección con nombres de las relaciones
                var dto = new
                {
                    dispositivo.Id_dispositivo,
                    dispositivo.Nombre,
                    dispositivo.Descripcion_detallada,
                    dispositivo.Es_invasivo,
                    dispositivo.Requiere_biocompatibilidad,
                    dispositivo.Requiere_prueba_residuales,
                    dispositivo.Metodo_esterilizacion_req,
                    dispositivo.Estado_regulatorio,
                    dispositivo.Fecha_registro,

                    CategoriaNombre = dispositivo.Categoria?.Nombre_Categoria, 
                    ClaseRiesgoNombre = dispositivo.ClaseRiesgo?.Nombre_clase,
                    TipoDispositivoNombre = dispositivo.TipoDispositivo?.Nombre_tipo,
                    CuartoNombre = dispositivo.CuartoRequerido?.Nombre_cuarto
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
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
        [Permiso("CREAR_DISPOSITIVOS")]
        // PUT: api/Dispositivos/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DispositivoCreateDto>> PutDispositivo(int id, DispositivoCreateDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dispositivo = await _context.Dispositivodv.FindAsync(id);
                if (dispositivo is null)
                    return NotFound($"No se encontró el dispositivo con ID {id}");

                // Actualizar campos desde el DTO
                dispositivo.Nombre = updateDto.Nombre;
                dispositivo.Descripcion_detallada = updateDto.Descripcion_detallada;
                dispositivo.Id_categoria = updateDto.Id_categoria;
                dispositivo.Id_clase_riesgo = updateDto.Id_clase_riesgo;
                dispositivo.Id_tipo_dispositivo = updateDto.Id_tipo_dispositivo;
                dispositivo.Id_cuarto_requerido = updateDto.Id_cuarto_requerido;
                dispositivo.Es_invasivo = updateDto.Es_invasivo;
                dispositivo.Requiere_biocompatibilidad = updateDto.Requiere_biocompatibilidad;
                dispositivo.Requiere_prueba_residuales = updateDto.Requiere_prueba_residuales;
                dispositivo.Metodo_esterilizacion_req = updateDto.Metodo_esterilizacion_req;
                dispositivo.Estado_regulatorio = updateDto.Estado_regulatorio;
                dispositivo.Fecha_registro = updateDto.Fecha_registro;

                await _context.SaveChangesAsync();

                return Ok(updateDto);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar el dispositivo: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/Dispositivos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDispositivo(int id)
        {
            var dispositivo = await _context.Dispositivodv.FindAsync(id);
            if (dispositivo is null)
                return NotFound();

            try
            {
                _context.Dispositivodv.Remove(dispositivo);
                await _context.SaveChangesAsync();
                return NoContent(); // 204 sin cuerpo de respuesta
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode(500);
            }
        }


    }
}
