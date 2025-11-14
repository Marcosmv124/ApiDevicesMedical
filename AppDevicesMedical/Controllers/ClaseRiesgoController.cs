using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppDevicesMedical.DTOs;
using AppDevicesMedical.Models;

namespace AppDevicesMedical.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class ClaseRiesgoController : Controller
    {
        private readonly MedicalDevicesDbContext _context;

        public ClaseRiesgoController(MedicalDevicesDbContext context)
        {
            _context = context;
        }

        // GET: api/ClaseRiesgo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaseRiesgoDto>>> GetClaseRiesgo()
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

        // GET: api/ClaseRiesgo/5
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
                return NotFound();
            }

            return clase;
        }

        // PUT: api/ClaseRiesgo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClaseRiesgo(int id, ClaseRiesgo claseRiesgo)
        {
            if (id != claseRiesgo.Id_clase_riesgo)
            {
                return BadRequest();
            }

            _context.Entry(claseRiesgo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaseRiesgoExists(id))
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

        // POST: api/ClaseRiesgo
        [HttpPost]
        public async Task<ActionResult<ClaseRiesgoDto>> PostClaseRiesgo(ClaseRiesgoCreateDto dto)
        {
            var claseRiesgo = new ClaseRiesgo
            {
                Nombre_clase = dto.NombreClase,
                Descripcion = dto.Descripcion
            };

            _context.ClaseRiesgo.Add(claseRiesgo);
            await _context.SaveChangesAsync();

            var resultDto = new ClaseRiesgoDto
            {
                IdClaseRiesgo = claseRiesgo.Id_clase_riesgo,
                NombreClase = claseRiesgo.Nombre_clase,
                Descripcion = claseRiesgo.Descripcion,
                Dispositivos = new List<DispositivoDto>()
            };

            return CreatedAtAction("GetClaseRiesgo", new { id = claseRiesgo.Id_clase_riesgo }, resultDto);
        }

        // DELETE: api/ClaseRiesgo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaseRiesgo(int id)
        {
            var claseRiesgo = await _context.ClaseRiesgo.FindAsync(id);
            if (claseRiesgo == null)
            {
                return NotFound();
            }

            _context.ClaseRiesgo.Remove(claseRiesgo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClaseRiesgoExists(int id)
        {
            return _context.ClaseRiesgo.Any(e => e.Id_clase_riesgo == id);
        }
    }
}
