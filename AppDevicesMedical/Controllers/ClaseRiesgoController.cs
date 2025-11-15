using AppDevicesMedical.Authorization;
using AppDevicesMedical.DTOs;
using AppDevicesMedical.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDevicesMedical.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClaseRiesgoController : ControllerBase // Usar ControllerBase es más apropiado para un API Controller
    {
        private readonly MedicalDevicesDbContext _context;

        public ClaseRiesgoController(MedicalDevicesDbContext context)
        {
            _context = context;
        }

        [Permiso("VER_CLASES_RIESGO")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaseRiesgoDto>>> GetClaseRiesgo()
        {
            try
            {
                var clases = await _context.ClaseRiesgo
                    .Include(c => c.Dispositivos)
                    .Select(c => new ClaseRiesgoDto
                    {
                        IdClaseRiesgo = c.Id_clase_riesgo,
                        NombreClase = c.Nombre_clase,
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

                return clases;
            }
            catch (Exception)
            {
                // Devolver 500 Internal Server Error en caso de problemas de DB o mapeo.
                return StatusCode(500, "Error al obtener las clases de riesgo. Intenta nuevamente o contacta al administrador.");
            }
        }

        [Permiso("VER_CLASE_RIESGO")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ClaseRiesgoDto>> GetClaseRiesgo(int id)
        {
            var clase = await _context.ClaseRiesgo
                .Include(c => c.Dispositivos)
                .Where(c => c.Id_clase_riesgo == id)
                .Select(c => new ClaseRiesgoDto
                {
                    IdClaseRiesgo = c.Id_clase_riesgo,
                    NombreClase = c.Nombre_clase,
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

            if (clase == null)
            {
                // Devolver 404 con mensaje detallado
                return NotFound($"No se encontró ninguna clase de riesgo con el ID {id}.");
            }

            return clase;
        }

        // PUT: api/ClaseRiesgo/5
        [Permiso("EDITAR_CLASE_RIESGO")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClaseRiesgo(int id, ClaseRiesgoCreateDto dto)
        {
            // Validación de entrada
            if (string.IsNullOrWhiteSpace(dto.NombreClase))
                return BadRequest("El nombre de la clase de riesgo no puede estar vacío.");

            if (dto.NombreClase.Length > 100)
                return BadRequest("El nombre de la clase de riesgo no debe exceder los 100 caracteres.");

            var claseRiesgo = await _context.ClaseRiesgo.FindAsync(id);
            if (claseRiesgo == null)
            {
                // Devolver 404
                return NotFound("Clase de riesgo no encontrada para actualizar.");
            }

            // Validación de unicidad (evita duplicados)
            var existeNombre = await _context.ClaseRiesgo
                .AnyAsync(t => t.Nombre_clase == dto.NombreClase && t.Id_clase_riesgo != id);

            if (existeNombre)
                // Devolver 409 Conflict
                return Conflict("Ya existe otra clase de riesgo con ese nombre.");

            // Actualización
            claseRiesgo.Nombre_clase = dto.NombreClase;
            claseRiesgo.Descripcion = dto.Descripcion;

            _context.Entry(claseRiesgo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // 204 No Content
            }
            catch (DbUpdateException)
            {
                // Manejo de errores de base de datos
                return StatusCode(500, "Error al actualizar la clase de riesgo. Intenta nuevamente o contacta al administrador.");
            }
        }

        // POST: api/ClaseRiesgo
        [Permiso("CREAR_CLASE_RIESGO")]
        [HttpPost]
        public async Task<ActionResult<ClaseRiesgoDto>> PostClaseRiesgo(ClaseRiesgoCreateDto dto)
        {
            // Validaciones de entrada
            if (string.IsNullOrWhiteSpace(dto.NombreClase))
                return BadRequest("El nombre de la clase de riesgo no puede estar vacío.");

            if (dto.NombreClase.Length > 100)
                return BadRequest("El nombre de la clase de riesgo no debe exceder los 100 caracteres.");

            // Validación de unicidad
            var existe = await _context.ClaseRiesgo
                .AnyAsync(t => t.Nombre_clase == dto.NombreClase);

            if (existe)
                // Devolver 409 Conflict
                return Conflict("Ya existe una clase de riesgo con ese nombre.");

            var nuevaClase = new ClaseRiesgo
            {
                Nombre_clase = dto.NombreClase,
                Descripcion = dto.Descripcion
            };

            try
            {
                _context.ClaseRiesgo.Add(nuevaClase);
                await _context.SaveChangesAsync();

                var resultDto = new ClaseRiesgoDto
                {
                    IdClaseRiesgo = nuevaClase.Id_clase_riesgo,
                    NombreClase = nuevaClase.Nombre_clase,
                    Descripcion = nuevaClase.Descripcion,
                    Dispositivos = new List<DispositivoDto>()
                };

                // Devolver 201 Created
                return CreatedAtAction(nameof(GetClaseRiesgo), new { id = nuevaClase.Id_clase_riesgo }, resultDto);
            }
            catch (DbUpdateException)
            {
                // Manejo de errores de base de datos
                return StatusCode(500, "Error al registrar la clase de riesgo. Verifica los datos o contacta al administrador.");
            }
        }

        // DELETE: api/ClaseRiesgo/5
        [Permiso("ELIMINAR_CLASE_RIESGO")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaseRiesgo(int id)
        {
            var claseRiesgo = await _context.ClaseRiesgo
                .Include(t => t.Dispositivos) // Incluir dispositivos para verificar dependencias
                .FirstOrDefaultAsync(t => t.Id_clase_riesgo == id);

            if (claseRiesgo == null)
            {
                // Devolver 404
                return NotFound($"No se encontró la clase de riesgo con ID {id} para eliminar.");
            }

            // Verificación de dependencias
            if (claseRiesgo.Dispositivos.Any())
            {
                // Devolver 409 Conflict
                return Conflict("No se puede eliminar: hay dispositivos que dependen de esta clase de riesgo.");
            }

            try
            {
                _context.ClaseRiesgo.Remove(claseRiesgo);
                await _context.SaveChangesAsync();

                return NoContent(); // 204 No Content
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Error al eliminar la clase de riesgo. Intenta nuevamente o contacta al administrador.");
            }
        }
    }
}