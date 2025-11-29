using AppDevicesMedical.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDevicesMedical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriasController : ControllerBase
    {
        private readonly MedicalDevicesDbContext _context;

        public AuditoriasController(MedicalDevicesDbContext context)
        {
            _context = context;
        }

        // GET: api/Auditorias
        [HttpGet]
        public IAsyncEnumerable<Auditoria> GetAuditoria(int maxRecords = 1000)
        {
            _context.Database.SetCommandTimeout(180); // 3 minutos

            // Consulta SQL directamente
            string sqlQuery = @"
            SELECT * FROM Auditoria
            ORDER BY Timestamp DESC
            LIMIT @maxRecords;";

            // Ejecutar la consulta SQL y mapear los resultados a Auditoria
            return _context.Auditoria
                .FromSqlRaw(sqlQuery, new SqlParameter("@maxRecords", maxRecords))
                .AsNoTracking()
                .AsAsyncEnumerable();
        }


        // GET: api/Auditorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Auditoria>> GetAuditoriaById(int id)
        {
            var auditoria = await _context.Auditoria.FindAsync(id);

            if (auditoria == null)
            {
                return NotFound();
            }

            return auditoria;
        }

        // PUT: api/Auditorias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuditoria(int id, Auditoria auditoria)
        {
            if (id != auditoria.Id_log)
            {
                return BadRequest();
            }

            _context.Entry(auditoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditoriaExists(id))
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

        // POST: api/Auditorias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Auditoria>> PostAuditoria(Auditoria auditoria)
        {
            _context.Auditoria.Add(auditoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuditoria", new { id = auditoria.Id_log }, auditoria);
        }

        // DELETE: api/Auditorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuditoria(int id)
        {
            var auditoria = await _context.Auditoria.FindAsync(id);
            if (auditoria == null)
            {
                return NotFound();
            }

            _context.Auditoria.Remove(auditoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuditoriaExists(int id)
        {
            return _context.Auditoria.Any(e => e.Id_log == id);
        }
    }
}
