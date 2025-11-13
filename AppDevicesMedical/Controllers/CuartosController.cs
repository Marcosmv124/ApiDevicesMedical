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
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/Cuartos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CuartoDto>> GetCuarto(int id)
        {
            try
            {
                var cuarto = await _context.Cuarto.FindAsync(id);
                if (cuarto == null)
                    return NotFound($"No se encontró el cuarto con ID {id}");

                return Ok(new CuartoDto(cuarto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/Cuartos
        [HttpPost]
        public async Task<ActionResult<CuartoDto>> PostCuarto(CreateCuartoDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var cuarto = createDto.ToEntity();

                _context.Cuarto.Add(cuarto);
                await _context.SaveChangesAsync();

                // Devolver DTO limpio
                var cuartoDto = new CuartoDto(cuarto);
                return CreatedAtAction(nameof(GetCuarto), new { id = cuarto.Id_cuarto }, cuartoDto);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al crear el cuarto: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/Cuartos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuarto(int id, CreateCuartoDto updateDto)
        {
            try
            {
                var cuartoExistente = await _context.Cuarto.FindAsync(id);
                if (cuartoExistente == null)
                    return NotFound($"No se encontró el cuarto con ID {id}");

                // Convertir DTO a entidad
                var updatedEntity = updateDto.ToEntity();

                // Asegurarse de que no se sobrescriba la clave primaria
                updatedEntity.Id_cuarto = cuartoExistente.Id_cuarto;

                // Aplicar los cambios
                _context.Entry(cuartoExistente).CurrentValues.SetValues(updatedEntity);

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar el cuarto: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }




        // DELETE: api/Cuartos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuarto(int id)
        {
            try
            {
                var cuarto = await _context.Cuarto.FindAsync(id);
                if (cuarto == null)
                    return NotFound($"No se encontró el cuarto con ID {id}");

                _context.Cuarto.Remove(cuarto);
                await _context.SaveChangesAsync();

                // Retornar mensaje de éxito
                return Ok(new { mensaje = $"El cuarto con ID {id} se eliminó correctamente." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al eliminar el cuarto: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
    //// GET: api/Cuartos/filtrar?clase=Clase9
    //[HttpGet("filtrar")]
    //public async Task<ActionResult<IEnumerable<Cuarto>>> FiltrarCuartos(
    //    [FromQuery] string? clase = null,
    //    [FromQuery] bool? controlTemperatura = null,
    //    [FromQuery] bool? controlHumedad = null,
    //    [FromQuery] int? capacidadMinima = null)
    //{
    //    try
    //    {
    //        var query = _context.Cuarto.AsQueryable();

    //        if (!string.IsNullOrEmpty(clase))
    //        {
    //            query = query.Where(c => c.Clase_cuarto.Contains(clase));
    //        }

    //        if (controlTemperatura.HasValue)
    //        {
    //            query = query.Where(c => c.Control_temperatura == controlTemperatura.Value);
    //        }

    //        if (controlHumedad.HasValue)
    //        {
    //            query = query.Where(c => c.Control_humedad == controlHumedad.Value);
    //        }

    //        if (capacidadMinima.HasValue)
    //        {
    //            query = query.Where(c => c.Capacidad_personas >= capacidadMinima.Value);
    //        }

    //        var resultados = await query.OrderBy(c => c.Clase_cuarto).ToListAsync();
    //        return resultados;
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, $"Error interno del servidor: {ex.Message}");
    //    }
    //}
    ///////////////////
    //// GET: api/Cuartos/estadisticas
    //[HttpGet("estadisticas")]
    //public async Task<ActionResult<object>> GetEstadisticas()
    //{
    //    try
    //    {
    //        var totalCuartos = await _context.Cuarto.CountAsync();
    //        var conControlTemperatura = await _context.Cuarto.CountAsync(c => c.Control_temperatura);
    //        var conControlHumedad = await _context.Cuarto.CountAsync(c => c.Control_humedad);
    //        var capacidadPromedio = await _context.Cuarto.AverageAsync(c => c.Capacidad_personas);

    //        var estadisticas = new
    //        {
    //            TotalCuartos = totalCuartos,
    //            ConControlTemperatura = conControlTemperatura,
    //            ConControlHumedad = conControlHumedad,
    //            CapacidadPromedio = Math.Round(capacidadPromedio, 2),
    //            PorcentajeControlTemperatura = totalCuartos > 0 ?
    //                Math.Round((double)conControlTemperatura / totalCuartos * 100, 2) : 0
    //        };

    //        return estadisticas;
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, $"Error interno del servidor: {ex.Message}");
    //    }
    //}
}