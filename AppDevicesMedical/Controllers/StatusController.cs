using AppDevicesMedical.Authorization;
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
    public class StatusController : ControllerBase
    {
        private readonly MedicalDevicesDbContext _context;

        public StatusController(MedicalDevicesDbContext context)
        {
            _context = context;
        }

        // GET: api/Status
        [HttpGet]
        [Permiso("VER_STATUS")]
        public async Task<ActionResult<IEnumerable<Status>>> GetStatus()
        {
            return await _context.Status.ToListAsync();
        }

        // GET: api/Status/5
        [Permiso("VER_STATUS_DETALLE")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Status>> GetStatus(int id)
        {
            var status = await _context.Status.FindAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            return status;
        }

        // PUT: api/Status/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Permiso("EDITAR_STATUS")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatus(int id, Status status)
        {
            if (id != status.Id_status)
            {
                return BadRequest();
            }

            _context.Entry(status).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Status
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Permiso("CREAR_STATUS")]
        [HttpPost]
        public async Task<ActionResult<Status>> PostStatus(Status status)
        {
            _context.Status.Add(status);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StatusExists(status.Id_status))
                {
                    //Error de ID duplicado (Duplicidad de Clave Primaria)
                    // Devuelve un 409 Conflict. ActionResult.Conflict() devuelve un 409 vacío.
                    return Conflict();
                }
                else
                {
                    //Otro Error de Base de Datos (ej. Violación de unicidad en otro campo)
                    // Devuelve un 400 Bad Request o un 500 Internal Server Error, dependiendo de tu política.
                    // Usaremos 400 ya que generalmente es causado por datos inválidos del cliente.
                    return BadRequest(); // Devuelve un 400 vacío.

                    // Si prefieres indicar un fallo interno y no saber la causa específica:
                    // return StatusCode(StatusCodes.Status500InternalServerError); // Devuelve un 500 vacío.
                }
            }

            //Éxito: Devuelve 201 Created
            return CreatedAtAction("GetStatus", new { id = status.Id_status }, status);
        }

        // DELETE: api/Status/5
        [Permiso("ELIMINAR_STATUS")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var status = await _context.Status.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }

            _context.Status.Remove(status);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatusExists(int id)
        {
            return _context.Status.Any(e => e.Id_status == id);
        }
    }
}
