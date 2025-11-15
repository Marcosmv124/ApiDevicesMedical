using AppDevicesMedical.Authorization;
using AppDevicesMedical.DTOs;
using AppDevicesMedical.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDevicesMedical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuartosController : ControllerBase
    {
        private readonly MedicalDevicesDbContext _context;

        public CuartosController(MedicalDevicesDbContext context)
        {
            _context = context;
        }

        // GET: api/Cuartos
        [Permiso("VER_CUARTOS")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuartoDto>>> GetCuartos()
        {
            try
            {
                var cuartos = await _context.Cuarto
                    .OrderBy(c => c.Id_cuarto)
                    .ToListAsync();

                // Convertir a DTOs
                var cuartosDto = cuartos.Select(c => new CuartoDto(c));

                return Ok(cuartosDto);
            }
            catch (Exception)
            {
                // Manejo de Error 500 (Mensaje amigable)
                return StatusCode(500, "Error al obtener la lista de cuartos. Intenta nuevamente o contacta al administrador.");
            }
        }

        // GET: api/Cuartos/5
        [Permiso("VER_CUARTO")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CuartoDto>> GetCuarto(int id)
        {
            try
            {
                var cuarto = await _context.Cuarto.FindAsync(id);

                if (cuarto == null)
                    // Manejo de Error 404
                    return NotFound($"No se encontró el cuarto con ID {id}.");

                return Ok(new CuartoDto(cuarto));
            }
            catch (Exception)
            {
                // Manejo de Error 500
                return StatusCode(500, "Error al obtener el detalle del cuarto. Intenta nuevamente o contacta al administrador.");
            }
        }

        // POST: api/Cuartos
        [Permiso("CREAR_CUARTO")]
        [HttpPost]
        public async Task<ActionResult<CuartoDto>> PostCuarto(CreateCuartoDto createDto)
        {
            // --- VALIDACIONES DE DOMINIO/UNICIDAD (Patrón consistente) ---
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Corregido: Usar 'Nombre_cuarto' del DTO para validar nulidad/vacío
            if (string.IsNullOrWhiteSpace(createDto.Nombre_cuarto))
                return BadRequest("El nombre del cuarto no puede estar vacío.");

            // Corregido: Asumiendo que 'Capacidad_personas' es el campo a validar como positivo
            // Si necesitas validar otro campo como 'Capacidad_produccion', ajústalo aquí.
            if (createDto.Capacidad_personas == null || createDto.Capacidad_personas <= 0)
                return BadRequest("La capacidad de personas debe ser un número positivo.");

            // Corregido: Usar 'Nombre_cuarto' del DTO para validar unicidad
            var existeNombre = await _context.Cuarto.AnyAsync(c => c.Nombre_cuarto == createDto.Nombre_cuarto);
            if (existeNombre)
                // Manejo de Error 409 Conflict
                return Conflict("Ya existe un cuarto con ese nombre registrado.");
            // -----------------------------------------------------------

            try
            {
                var cuarto = createDto.ToEntity();

                _context.Cuarto.Add(cuarto);
                await _context.SaveChangesAsync();

                // Devolver DTO limpio (201 Created)
                var cuartoDto = new CuartoDto(cuarto);
                return CreatedAtAction(nameof(GetCuarto), new { id = cuarto.Id_cuarto }, cuartoDto);
            }
            catch (DbUpdateException)
            {
                // Manejo de Error 500 (DbUpdateException)
                return StatusCode(500, "Error al registrar el cuarto. Verifica los datos o contacta al administrador.");
            }
            catch (Exception)
            {
                // Manejo de Error 500 (Excepción genérica)
                return StatusCode(500, "Error interno del servidor al procesar la solicitud.");
            }
        }

        // PUT: api/Cuartos/5
        [Permiso("EDITAR_CUARTO")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuarto(int id, CreateCuartoDto updateDto)
        {
            // --- VALIDACIONES DE DOMINIO/UNICIDAD (Patrón consistente) ---
            // Corregido: Usar 'Nombre_cuarto' del DTO para validar nulidad/vacío
            if (string.IsNullOrWhiteSpace(updateDto.Nombre_cuarto))
                return BadRequest("El nombre del cuarto no puede estar vacío.");

            // Corregido: Asumiendo que 'Capacidad_personas' es el campo a validar como positivo
            if (updateDto.Capacidad_personas == null || updateDto.Capacidad_personas <= 0)
                return BadRequest("La capacidad de personas debe ser un número positivo.");
            // -------------------------------------------------------------

            try
            {
                var cuartoExistente = await _context.Cuarto.FindAsync(id);
                if (cuartoExistente == null)
                    return NotFound($"No se encontró el cuarto con ID {id} para actualizar.");

                // --- VALIDACIÓN DE UNICIDAD vs OTROS REGISTROS ---
                // Corregido: Usar 'Nombre_cuarto' del DTO para validar unicidad
                var existeOtroConMismoNombre = await _context.Cuarto
                    .AnyAsync(c => c.Nombre_cuarto == updateDto.Nombre_cuarto && c.Id_cuarto != id);

                if (existeOtroConMismoNombre)
                    // Manejo de Error 409 Conflict
                    return Conflict("Ya existe otro cuarto con ese nombre.");
                // ----------------------------------------------------

                // Convertir DTO a entidad
                var updatedEntity = updateDto.ToEntity();

                // Asegurarse de que no se sobrescriba la clave primaria
                updatedEntity.Id_cuarto = cuartoExistente.Id_cuarto;

                // Aplicar los cambios
                _context.Entry(cuartoExistente).CurrentValues.SetValues(updatedEntity);

                await _context.SaveChangesAsync();
                return NoContent(); // 204 No Content
            }
            catch (DbUpdateException)
            {
                // Manejo de Error 500 (DbUpdateException)
                return StatusCode(500, "Error al actualizar el cuarto. Intenta nuevamente o contacta al administrador.");
            }
            catch (Exception)
            {
                // Manejo de Error 500 (Excepción genérica)
                return StatusCode(500, "Error interno del servidor al procesar la solicitud.");
            }
        }
        // DELETE: api/Cuartos/5
        [Permiso("ELIMINAR_CUARTO")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuarto(int id)
        {
            try
            {
                var cuarto = await _context.Cuarto.FindAsync(id);

                if (cuarto == null)
                    return NotFound($"No se encontró el cuarto con ID {id} para eliminar.");

                // --- VERIFICACIÓN DE DEPENDENCIAS EXPLÍCITA (Patrón consistente) ---
                // Se verifica si algún Dispositivo está relacionado con este Cuarto
                var tieneDependencias = await _context.Dispositivodv
                    .AnyAsync(d => d.Id_cuarto_requerido == id); // Asumiendo que Dispositivo tiene una FK a Cuarto

                if (tieneDependencias)
                    // Manejo de Error 409 Conflict
                    return Conflict("No se puede eliminar: hay dispositivos asociados a este cuarto.");
                // -----------------------------------------------------------------

                _context.Cuarto.Remove(cuarto);
                await _context.SaveChangesAsync();

                // Devolver 204 No Content (Estándar para DELETE exitoso)
                return NoContent();
            }
            catch (DbUpdateException)
            {
                // Manejo de Error 500 (DbUpdateException)
                return StatusCode(500, "Error al eliminar el cuarto. Intenta nuevamente o contacta al administrador.");
            }
            catch (Exception)
            {
                // Manejo de Error 500 (Excepción genérica)
                return StatusCode(500, "Error interno del servidor al procesar la solicitud.");
            }
        }
    }
}