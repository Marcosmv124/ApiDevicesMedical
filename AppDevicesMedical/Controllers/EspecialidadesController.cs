using AppDevicesMedical.Authorization;
using AppDevicesMedical.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDevicesMedical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        private readonly MedicalDevicesDbContext _context;

        public EspecialidadesController(MedicalDevicesDbContext context)
        {
            _context = context;
        }

        // GET: api/Especialidades
        [Permiso("VER_ESPECIALIDADES")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Especialidad>>> GetEspecialidad()
        {
            return await _context.Especialidad.ToListAsync();
        }

        // GET: api/Especialidades/5
        [Permiso("VER_ESPECIALIDAD")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Especialidad>> GetEspecialidad(int id)
        {
            var especialidad = await _context.Especialidad.FindAsync(id);

            if (especialidad == null)
            {
                return NotFound();
            }

            return especialidad;
        }

        // PUT: api/Especialidades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Permiso("EDITAR_ESPECIALIDAD")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEspecialidad(int id, Especialidad especialidad)
        {
            if (id != especialidad.Id_Especialidad)
            {
                return BadRequest();
            }

            _context.Entry(especialidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EspecialidadExists(id))
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

        // POST: api/Especialidades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Permiso("CREAR_ESPECIALIDAD")]
        [HttpPost]
        public async Task<ActionResult<Especialidad>> PostEspecialidad(Especialidad especialidad)
        {
            _context.Especialidad.Add(especialidad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEspecialidad", new { id = especialidad.Id_Especialidad }, especialidad);
        }

        // DELETE: api/Especialidades/5
        [Permiso("ELIMINAR_ESPECIALIDAD")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEspecialidad(int id)
        {
            var especialidad = await _context.Especialidad.FindAsync(id);
            if (especialidad == null)
            {
                return NotFound();
            }

            _context.Especialidad.Remove(especialidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EspecialidadExists(int id)
        {
            return _context.Especialidad.Any(e => e.Id_Especialidad == id);
        }
    }
}
