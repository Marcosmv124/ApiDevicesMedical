using AppDevicesMedical.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDevicesMedical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogosController : ControllerBase
    {
        // 1. Inyección de Dependencia de la Base de Datos
        private readonly MedicalDevicesDbContext _context;

        public CatalogosController(MedicalDevicesDbContext context)
        {
            _context = context;
        }

        // ====================================================================
        // ENDPOINT UNIFICADO: Sistemas de Calidad
        // ====================================================================
        // Este único endpoint alimenta a:
        // 1. Dropdown "Sistema de Calidad Origen"
        // 2. Dropdown "Sistema de Calidad Destino"
        // 3. Dropdown "Lugar de Comercialización"
        // URL: GET /api/Catalogos/sistemas-calidad
        [HttpGet("sistemas-calidad")]
        public async Task<ActionResult> GetSistemasCalidad()
        {
            // Consultamos la tabla SistemasCalidad
            var lista = await _context.SistemasCalidad
                .AsNoTracking() // Optimización: Solo lectura, más rápido
                .Select(s => new
                {
                    // ID: Es lo que el Frontend enviará al guardar (ej: "SC001")
                    Id = s.IdSistema,

                    // Nombre: Es lo que el usuario verá en la lista (ej: "Planta México")
                    Nombre = s.NombreSistema, 
                    Pais = s.Pais
                })
                .ToListAsync();

            return Ok(lista);
        }
    }
}
