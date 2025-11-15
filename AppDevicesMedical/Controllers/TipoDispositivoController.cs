using AppDevicesMedical.Authorization;
using AppDevicesMedical.DTOs;
using AppDevicesMedical.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDevicesMedical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoDispositivoController : ControllerBase
    {

        private readonly MedicalDevicesDbContext _context;

        public TipoDispositivoController(MedicalDevicesDbContext context)
        {
            _context = context;
        }
        [Permiso("VER_TIPOS_DISPOSITIVO")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoDispositivoDto>>> GetTipoDispositivos()
        {
            try
            {
                var tipos = await _context.TipoDispositivos
                    .Include(c => c.Dispositivos)
                    .Select(c => new TipoDispositivoDto
                    {
                        Id_tipo = c.Id_tipo,
                        Nombre_tipo = c.Nombre_tipo,
                        Descripcion = c.Descripcion,
                        Dispositivos = c.Dispositivos.Select(d => new DispositivoDto
                        {
                            IdDispositivo = d.Id_dispositivo,
                            Nombre = d.Nombre,
                            EsInvasivo = d.Es_invasivo,
                            MetodoEsterilizacionReq = d.Metodo_esterilizacion_req
                        }).ToList()
                    })
                    .ToListAsync();

                return tipos;
            }
            catch (Exception)
            {
                return StatusCode(500, "Error al obtener los tipos de dispositivo. Intenta nuevamente o contacta al administrador.");
            }
        }
        [Permiso("CREAR_TIPO_DISPOSITIVO")]
        [HttpPost]
        public async Task<ActionResult<TipoDispositivo>> PostTipoDispositivo(TipoDispositivoCreateDto dto)
        {
            // Validaciones de entrada
            if (string.IsNullOrWhiteSpace(dto.Nombre_tipo))
                return BadRequest("El nombre del tipo de dispositivo no puede estar vacío.");

            if (dto.Nombre_tipo.Length > 100)
                return BadRequest("El nombre del tipo de dispositivo no debe exceder los 100 caracteres.");

            // Validación de unicidad
            var existe = await _context.TipoDispositivos
                .AnyAsync(t => t.Nombre_tipo == dto.Nombre_tipo);

            if (existe)
                return Conflict("Ya existe un tipo de dispositivo con ese nombre.");

            var nuevoTipo = new TipoDispositivo
            {
                Nombre_tipo = dto.Nombre_tipo,
                Descripcion = dto.Descripcion
            };

            try
            {
                _context.TipoDispositivos.Add(nuevoTipo);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTipoDispositivo), new { id = nuevoTipo.Id_tipo }, nuevoTipo);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Error al registrar el tipo de dispositivo. Verifica los datos o contacta al administrador.");
            }
        }
        [Permiso("VER_TIPO_DISPOSITIVO")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoDispositivoDto>> GetTipoDispositivo(int id)
        {
            var tipo = await _context.TipoDispositivos
                .Include(c => c.Dispositivos)
                .Where(c => c.Id_tipo == id)
                .Select(c => new TipoDispositivoDto
                {
                    Id_tipo = c.Id_tipo,
                    Nombre_tipo = c.Nombre_tipo,
                    Descripcion = c.Descripcion,
                    Dispositivos = c.Dispositivos.Select(d => new DispositivoDto
                    {
                        IdDispositivo = d.Id_dispositivo,
                        Nombre = d.Nombre,
                        EsInvasivo = d.Es_invasivo,
                        MetodoEsterilizacionReq = d.Metodo_esterilizacion_req
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (tipo == null)
                return NotFound($"No se encontró ningún tipo de dispositivo con el ID {id}.");

            return tipo;
        }
        [Permiso("EDITAR_TIPO_DISPOSITIVO")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoDispositivo(int id, TipoDispositivoCreateDto dto)
        {
            // Validación de entrada
            if (string.IsNullOrWhiteSpace(dto.Nombre_tipo))
                return BadRequest("El nombre del tipo de dispositivo no puede estar vacío.");

            if (dto.Nombre_tipo.Length > 100)
                return BadRequest("El nombre del tipo de dispositivo no debe exceder los 100 caracteres.");

            var tipo = await _context.TipoDispositivos.FindAsync(id);
            if (tipo == null)
                return NotFound("Tipo de dispositivo no encontrado.");

            // Validación de unicidad (evita duplicados)
            var existeNombre = await _context.TipoDispositivos
                .AnyAsync(t => t.Nombre_tipo == dto.Nombre_tipo && t.Id_tipo != id);

            if (existeNombre)
                return Conflict("Ya existe otro tipo de dispositivo con ese nombre.");

            // Actualización
            tipo.Nombre_tipo = dto.Nombre_tipo;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Error al actualizar el tipo de dispositivo. Intenta nuevamente o contacta al administrador.");
            }
        }
        [Permiso("ELIMINAR_TIPO_DISPOSITIVO")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoDispositivo(int id)
        {
            var tipo = await _context.TipoDispositivos
                .Include(t => t.Dispositivos)
                .FirstOrDefaultAsync(t => t.Id_tipo == id);

            if (tipo == null)
                return NotFound();

            if (tipo.Dispositivos.Any())
                return Conflict("No se puede eliminar: hay dispositivos que dependen de este tipo.");

            _context.TipoDispositivos.Remove(tipo);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
