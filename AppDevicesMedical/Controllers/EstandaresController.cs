using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppDevicesMedical.Models;

namespace AppDevicesMedical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstandaresController : ControllerBase
    {
        private readonly MedicalDevicesDbContext _context;

        public EstandaresController(MedicalDevicesDbContext context)
        {
            _context = context;
        }

        // GET: api/Estandares
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estandar>>> GetEstandar()
        {
            return await _context.Estandares.ToListAsync();
        }

        // GET: api/Estandares/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estandar>> GetEstandar(int id)
        {
            var estandar = await _context.Estandares.FindAsync(id);

            if (estandar == null)
            {
                return NotFound();
            }

            return estandar;
        }

        // PUT: api/Estandares/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstandar(int id, Estandar estandar)
        {
            if (id != estandar.IdEstandar)
            {
                return BadRequest();
            }

            _context.Entry(estandar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstandarExists(id))
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

        // POST: api/Estandares
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estandar>> PostEstandar(Estandar estandar)
        {
            _context.Estandares.Add(estandar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstandar", new { id = estandar.IdEstandar }, estandar);
        }

        // DELETE: api/Estandares/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstandar(int id)
        {
            var estandar = await _context.Estandares.FindAsync(id);
            if (estandar == null)
            {
                return NotFound();
            }

            _context.Estandares.Remove(estandar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstandarExists(int id)
        {
            return _context.Estandares.Any(e => e.IdEstandar == id);
        }
    }
}
