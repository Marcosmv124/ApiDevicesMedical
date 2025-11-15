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
    public class ExamenController : ControllerBase
    {
        private readonly MedicalDevicesDbContext _context;

        public ExamenController(MedicalDevicesDbContext context)
        {
            _context = context;
        }

        // GET: api/Examen
        [Permiso("VER_EXAMENES")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Examen>>> GetExamenes()
        {
            return await _context.Examenes.ToListAsync();
        }

        // GET: api/Examen/5
        [Permiso("VER_EXAMEN")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Examen>> GetExamen(int id)
        {
            var examen = await _context.Examenes.FindAsync(id);

            if (examen == null)
            {
                return NotFound();
            }

            return examen;
        }

        // PUT: api/Examen/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Permiso("EDITAR_EXAMEN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamen(int id, Examen examen)
        {
            if (id != examen.IdExamen)
            {
                return BadRequest();
            }

            _context.Entry(examen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamenExists(id))
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

        // POST: api/Examen
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Permiso("CREAR_EXAMEN")]
        [HttpPost]
        public async Task<ActionResult<Examen>> PostExamen(Examen examen)
        {
            _context.Examenes.Add(examen);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExamen", new { id = examen.IdExamen }, examen);
        }

        // DELETE: api/Examen/5
        [Permiso("ELIMINAR_EXAMEN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamen(int id)
        {
            var examen = await _context.Examenes.FindAsync(id);
            if (examen == null)
            {
                return NotFound();
            }

            _context.Examenes.Remove(examen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamenExists(int id)
        {
            return _context.Examenes.Any(e => e.IdExamen == id);
        }
    }
}
