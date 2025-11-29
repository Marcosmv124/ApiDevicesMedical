using AppDevicesMedical.DTOs;
using AppDevicesMedical.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppDevicesMedical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferenciasController : ControllerBase
    {
        private readonly MedicalDevicesDbContext _context;
        private readonly IConfiguration _configuration;

        public TransferenciasController(MedicalDevicesDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Transferencias
        [HttpGet]
        public async Task<IActionResult> GetTransferencias()
        {
            var transferencias = await _context.Transferencias
                .Include(t => t.Dispositivo)
                .Include(t => t.SistemaCalidadOrigen)
                .Include(t => t.SistemaCalidadDestino)
                .AsNoTracking() // ⚡ Optimización
                .Select(t => new
                {
                    t.IdTransferencia,
                    t.IdDispositivo,
                    NombreDispositivo = t.Dispositivo.Nombre, // Agregado para mejor visualización
                    t.UsuarioResponsable,
                    t.SitioOrigen,
                    t.SitioDestino,
                    t.FechaCreacion,
                    t.Estatus
                })
                .Take(100) // ⚡ Protección: Máximo 100 registros para no tumbar la BD
                .ToListAsync();

            return Ok(transferencias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransferenciaDetalleDto>> GetDetalle(int id)
        {
            var transferencia = await _context.Transferencias
                .AsNoTracking()
                .Include(t => t.Dispositivo)
                .Include(t => t.TransferenciaEstandares)
                .Include(t => t.BusinessCase)
                .FirstOrDefaultAsync(t => t.IdTransferencia == id);

            if (transferencia == null) return NotFound();

            var bc = transferencia.BusinessCase;

            var dto = new TransferenciaDetalleDto
            {
                Id = transferencia.IdTransferencia,
                IdDispositivo = transferencia.IdDispositivo,
                NombreDispositivo = transferencia.Dispositivo?.Nombre ?? "Desconocido",
                UsuarioResponsable = transferencia.UsuarioResponsable,
                Estatus = transferencia.Estatus,
                Prioridad = transferencia.Prioridad,
                SitioOrigen = transferencia.SitioOrigen,
                SitioDestino = transferencia.SitioDestino,
                IdSistemaCalidadOrigen = transferencia.IdSistemaCalidadOrigen,
                IdSistemaCalidadDestino = transferencia.IdSistemaCalidadDestino,
                IdLugarComercializacion = transferencia.IdLugarComercializacion,
                FechaInicioEstimada = transferencia.FechaInicioEstimada,
                FechaFinEstimada = transferencia.FechaFinEstimada,

                // Mapeo seguro del Business Case
                VolumenProduccion = bc?.VolumenProduccion ?? 0,
                TiempoRampUpMeses = bc?.TiempoRampUpMeses ?? 0,
                CostoUnitarioOrigen = bc?.CostoUnitarioOrigen ?? 0m,
                CostoUnitarioDestino = bc?.CostoUnitarioDestino ?? 0m,
                CostoCapacidadInstalada = bc?.CostoCapacidadInstalada ?? 0m,
                CostoCalidadPreventiva = bc?.CostoCalidadPreventiva ?? 0m,
                FondoRiesgo = bc?.FondoRiesgo ?? 0m,
                AplicaNoRequerido = bc?.AplicaNoRequerido ?? false,

                EstandaresSeleccionados = transferencia.TransferenciaEstandares
                                            .Select(te => te.IdEstandar)
                                            .ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTransferencia([FromBody] TransferenciaDto dto)
        {
            // 1. Validaciones
            var usuarioNombre = User.Claims.FirstOrDefault(c => c.Type == "Nombre")?.Value;
            if (string.IsNullOrEmpty(usuarioNombre)) return Unauthorized("No se encontró el usuario en el token.");

            var existeOrigen = await _context.SistemasCalidad.AnyAsync(s => s.IdSistema == dto.IdSistemaCalidadOrigen);
            var existeDestino = await _context.SistemasCalidad.AnyAsync(s => s.IdSistema == dto.IdSistemaCalidadDestino);
            if (!existeOrigen || !existeDestino) return BadRequest("Sistemas de calidad no válidos.");

            // 2. Crear Padre
            var transferencia = new Transferencia
            {
                IdDispositivo = dto.IdDispositivo,
                UsuarioResponsable = usuarioNombre,
                Estatus = dto.Estatus,
                Prioridad = dto.Prioridad,
                SitioOrigen = dto.SitioOrigen,
                SitioDestino = dto.SitioDestino,
                IdSistemaCalidadOrigen = dto.IdSistemaCalidadOrigen,
                IdSistemaCalidadDestino = dto.IdSistemaCalidadDestino,
                IdLugarComercializacion = dto.IdLugarComercializacion,
                FechaInicioEstimada = dto.FechaInicioEstimada,
                FechaFinEstimada = dto.FechaFinEstimada
            };

            _context.Transferencias.Add(transferencia);
            await _context.SaveChangesAsync(); // Genera ID

            // 3. Lógica Business Case (REFACTORIZADO) 💎
            // Creamos el objeto vacío y llamamos a la función mágica
            var businessCase = new BusinessCase { IdTransferencia = transferencia.IdTransferencia };
            RecalcularBusinessCase(businessCase, dto); // <--- AQUÍ LA LLAMADA
            _context.BusinessCases.Add(businessCase);

            // 4. Guardar Estándares
            if (dto.EstandaresSeleccionados?.Any() == true)
            {
                var listaEstandares = dto.EstandaresSeleccionados.Select(idEst => new TransferenciaEstandar
                {
                    IdTransferencia = transferencia.IdTransferencia,
                    IdEstandar = idEst
                });
                _context.TransferenciaEstandares.AddRange(listaEstandares);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Transferencia creada correctamente",
                IdTransferencia = transferencia.IdTransferencia,
                AnalisisFinanciero = new
                {
                    Resultado = businessCase.EsViable ? "VIABLE" : "NO VIABLE",
                    AhorroProyectado = businessCase.AhorroTotalProyectado
                }
            });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTransferencia(int id, [FromBody] TransferenciaDto dto)
        {
            // 1. Buscar
            var transferencia = await _context.Transferencias
                .Include(t => t.BusinessCase)
                .Include(t => t.TransferenciaEstandares)
                .FirstOrDefaultAsync(t => t.IdTransferencia == id);

            if (transferencia == null) return NotFound("Transferencia no encontrada.");

            // 2. Actualizar Padre
            transferencia.IdDispositivo = dto.IdDispositivo;
            transferencia.Estatus = dto.Estatus;
            transferencia.Prioridad = dto.Prioridad;
            transferencia.SitioOrigen = dto.SitioOrigen;
            transferencia.SitioDestino = dto.SitioDestino;
            transferencia.IdSistemaCalidadOrigen = dto.IdSistemaCalidadOrigen;
            transferencia.IdSistemaCalidadDestino = dto.IdSistemaCalidadDestino;
            transferencia.IdLugarComercializacion = dto.IdLugarComercializacion;
            transferencia.FechaInicioEstimada = dto.FechaInicioEstimada;
            transferencia.FechaFinEstimada = dto.FechaFinEstimada;

            // 3. Lógica Business Case (REFACTORIZADO) 💎
            if (transferencia.BusinessCase == null)
            {
                transferencia.BusinessCase = new BusinessCase { IdTransferencia = id };
            }
            RecalcularBusinessCase(transferencia.BusinessCase, dto); // <--- AQUÍ LA MISMA LLAMADA

            // 4. Actualizar Estándares
            _context.TransferenciaEstandares.RemoveRange(transferencia.TransferenciaEstandares);
            if (dto.EstandaresSeleccionados?.Any() == true)
            {
                foreach (var idEst in dto.EstandaresSeleccionados)
                {
                    transferencia.TransferenciaEstandares.Add(new TransferenciaEstandar
                    {
                        IdTransferencia = id,
                        IdEstandar = idEst
                    });
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Transferencia actualizada exitosamente",
                IdTransferencia = id,
                AnalisisFinanciero = new
                {
                    Resultado = transferencia.BusinessCase.EsViable ? "VIABLE" : "NO VIABLE",
                    AhorroProyectado = transferencia.BusinessCase.AhorroTotalProyectado
                }
            });
        }

        [HttpGet("reporte/{id}")]
        [Authorize]
        public async Task<IActionResult> GetReporteTransferencia(int id)
        {
            var transferencia = await _context.Transferencias
                .AsNoTracking()
                .Include(t => t.BusinessCase)
                .FirstOrDefaultAsync(t => t.IdTransferencia == id);

            if (transferencia == null) return NotFound();

            var reporte = new TransferenciaReporteDto
            {
                IdTransferencia = transferencia.IdTransferencia,
                Folio = $"TR-HIST-{transferencia.IdTransferencia:D3}",
                SolicitadoPor = transferencia.UsuarioResponsable,
                FechaCreacion = transferencia.FechaCreacion.ToString("yyyy-MM-dd"),
                SitioOrigen = transferencia.SitioOrigen,
                SistemaCalidadOrigen = transferencia.IdSistemaCalidadOrigen,
                SitioDestino = transferencia.SitioDestino,
                SistemaCalidadDestino = transferencia.IdSistemaCalidadDestino,
                Comercializacion = transferencia.IdLugarComercializacion,
                Volumen = transferencia.BusinessCase?.VolumenProduccion ?? 0,
                RampUpMeses = transferencia.BusinessCase?.TiempoRampUpMeses ?? 0,
                CapacidadInversion = transferencia.BusinessCase?.CostoCapacidadInstalada ?? 0,
                AhorroProyectado = transferencia.BusinessCase?.AhorroTotalProyectado ?? 0,
                EsViable = transferencia.BusinessCase?.EsViable ?? false,
                MensajeViabilidad = (transferencia.BusinessCase?.EsViable ?? false) ? "PROYECTO VIABLE" : "NO VIABLE"
            };

            return Ok(reporte);
        }

        // =================================================================================
        // MÉTODO PRIVADO: EL CEREBRO DE LA OPERACIÓN 🧠
        // Aquí centralizamos toda la lógica financiera.
        // =================================================================================
        private void RecalcularBusinessCase(BusinessCase bc, TransferenciaDto dto)
        {
            // 1. Obtener configuración
            decimal roiMinimo = _configuration.GetValue<decimal>("ReglasNegocio:RoiMinimo", 0.15m);

            // 2. Cálculos matemáticos
            decimal ahorroUnitario = dto.CostoUnitarioOrigen - dto.CostoUnitarioDestino;
            decimal ahorroTotal = ahorroUnitario * dto.VolumenProduccion;

            decimal inversionTotal = dto.CostoCapacidadInstalada
                                   + dto.CostoCalidadPreventiva
                                   + dto.FondoRiesgo;

            // 3. Determinar Viabilidad
            bool esViable = false;

            if (ahorroUnitario < 0)
            {
                esViable = false; // Pierdes dinero por unidad
            }
            else if (inversionTotal > 0)
            {
                decimal gananciaNeta = ahorroTotal - inversionTotal;
                decimal roi = gananciaNeta / inversionTotal;
                esViable = roi >= roiMinimo;
            }
            else
            {
                // Si no hay inversión, pero hay ahorro, es ganancia pura
                esViable = ahorroTotal > 0;
            }

            // 4. Asignar valores al objeto (paso por referencia)
            bc.VolumenProduccion = dto.VolumenProduccion;
            bc.TiempoRampUpMeses = dto.TiempoRampUpMeses;
            bc.CostoUnitarioOrigen = dto.CostoUnitarioOrigen;
            bc.CostoUnitarioDestino = dto.CostoUnitarioDestino;
            bc.CostoCapacidadInstalada = dto.CostoCapacidadInstalada;
            bc.CostoCalidadPreventiva = dto.CostoCalidadPreventiva;
            bc.FondoRiesgo = dto.FondoRiesgo;
            bc.AplicaNoRequerido = dto.AplicaNoRequerido;

            // Resultados calculados
            bc.AhorroTotalProyectado = ahorroTotal;
            bc.EsViable = esViable;
        }
        // DELETE: api/Transferencias/5
        [HttpDelete("{id}")]
        [Authorize] // Importante: Proteger esto
        public async Task<IActionResult> CancelarTransferencia(int id)
        {
            // 1. Buscamos la transferencia
            var transferencia = await _context.Transferencias.FindAsync(id);

            if (transferencia == null) return NotFound("Transferencia no encontrada.");

            // 2. VALIDACIÓN DE NEGOCIO (Muy Importante)
            // No deberías permitir cancelar algo que ya se terminó o ya está cancelado.
            if (transferencia.Estatus == "COMPLETADA" || transferencia.Estatus == "ENVIADA")
            {
                return BadRequest($"No se puede cancelar una transferencia con estatus '{transferencia.Estatus}'.");
            }

            if (transferencia.Estatus == "CANCELADA")
            {
                return BadRequest("La transferencia ya está cancelada.");
            }

            // 3. APLICAMOS LA "BAJA" (Cambio de Estatus)
            try
            {
                transferencia.Estatus = "CANCELADA";

                // Opcional: Podrías guardar la fecha o usuario que canceló si agregas esos campos
                // transferencia.FechaCancelacion = DateTime.Now; 

                await _context.SaveChangesAsync();

                return Ok(new { Mensaje = "Transferencia cancelada correctamente", NuevoEstatus = "CANCELADA" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al cancelar: {ex.Message}");
            }
        }

    }
}
#region
//using AppDevicesMedical.DTOs;
//using AppDevicesMedical.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Configuration;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace AppDevicesMedical.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TransferenciasController : ControllerBase
//    {
//        private readonly MedicalDevicesDbContext _context;
//        private readonly IConfiguration _configuration;

//        public TransferenciasController(MedicalDevicesDbContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//        }
//        // GET: api/Transferencias
//        [HttpGet]
//        public async Task<IActionResult> GetTransferencias()
//        {
//            var transferencias = await _context.Transferencias
//                .Include(t => t.Dispositivo) // Si Dispositivo es una entidad relacionada
//                .Include(t => t.SistemaCalidadOrigen) // Si necesitas detalles de los sistemas de calidad
//                .Include(t => t.SistemaCalidadDestino) // Para obtener el destino de calidad
//                .Select(t => new
//                {
//                    t.IdTransferencia,
//                    t.IdDispositivo,
//                    t.UsuarioResponsable,
//                    t.SitioOrigen,
//                    t.SitioDestino,
//                    t.FechaCreacion,
//                    t.Estatus
//                })
//                .ToListAsync();

//            return Ok(transferencias);
//        }
//        [HttpGet("{id}")]
//        public async Task<ActionResult<TransferenciaDetalleDto>> GetDetalle(int id)
//        {
//            var transferencia = await _context.Transferencias
//                .AsNoTracking()
//                .Include(t => t.Dispositivo)
//                .Include(t => t.TransferenciaEstandares)
//                .Include(t => t.BusinessCase) // <--- 1. IMPORTANTE: Traer la tabla hija
//                .FirstOrDefaultAsync(t => t.IdTransferencia == id); // <--- Corregido IdTransferencia

//            if (transferencia == null) return NotFound();

//            // Preparamos el objeto BusinessCase por si viene nulo (para evitar error 500)
//            var bc = transferencia.BusinessCase;

//            var dto = new TransferenciaDetalleDto
//            {
//                // === IDENTIDAD ===
//                Id = transferencia.IdTransferencia,

//                // === RELACIONES ===
//                IdDispositivo = transferencia.IdDispositivo,
//                NombreDispositivo = transferencia.Dispositivo?.Nombre ?? "Desconocido",
//                UsuarioResponsable = transferencia.UsuarioResponsable,

//                // === DATOS GENERALES ===
//                Estatus = transferencia.Estatus,
//                Prioridad = transferencia.Prioridad,
//                SitioOrigen = transferencia.SitioOrigen,
//                SitioDestino = transferencia.SitioDestino,

//                IdSistemaCalidadOrigen = transferencia.IdSistemaCalidadOrigen,
//                IdSistemaCalidadDestino = transferencia.IdSistemaCalidadDestino,
//                IdLugarComercializacion = transferencia.IdLugarComercializacion,

//                FechaInicioEstimada = transferencia.FechaInicioEstimada,
//                FechaFinEstimada = transferencia.FechaFinEstimada,

//                // === DATOS BUSINESS CASE (Corregido) ===
//                // Accedemos a través de la variable 'bc' y usamos '?? 0' por si no existe el business case aún.

//                VolumenProduccion = bc?.VolumenProduccion ?? 0,
//                TiempoRampUpMeses = bc?.TiempoRampUpMeses ?? 0,

//                CostoUnitarioOrigen = bc?.CostoUnitarioOrigen ?? 0m,
//                CostoUnitarioDestino = bc?.CostoUnitarioDestino ?? 0m,
//                CostoCapacidadInstalada = bc?.CostoCapacidadInstalada ?? 0m,
//                CostoCalidadPreventiva = bc?.CostoCalidadPreventiva ?? 0m,
//                FondoRiesgo = bc?.FondoRiesgo ?? 0m,

//                AplicaNoRequerido = bc?.AplicaNoRequerido ?? false,

//                // === LISTAS RELACIONADAS ===
//                EstandaresSeleccionados = transferencia.TransferenciaEstandares
//                                            .Select(te => te.IdEstandar)
//                                            .ToList()
//            };

//            return Ok(dto);
//        }
//        [HttpPost]
//        [Authorize]
//        public async Task<IActionResult> CreateTransferencia([FromBody] TransferenciaDto transferenciaDTO)
//        {
//            // ==============================================================================
//            // 1. VALIDACIONES INICIALES
//            // ==============================================================================
//            var usuarioNombre = User.Claims.FirstOrDefault(c => c.Type == "Nombre")?.Value;
//            if (string.IsNullOrEmpty(usuarioNombre)) return Unauthorized("No se encontró el usuario en el token.");

//            transferenciaDTO.UsuarioResponsable = usuarioNombre;

//            var existeOrigen = await _context.SistemasCalidad.AnyAsync(s => s.IdSistema == transferenciaDTO.IdSistemaCalidadOrigen);
//            var existeDestino = await _context.SistemasCalidad.AnyAsync(s => s.IdSistema == transferenciaDTO.IdSistemaCalidadDestino);

//            if (!existeOrigen) return BadRequest($"El sistema de origen '{transferenciaDTO.IdSistemaCalidadOrigen}' no existe.");
//            if (!existeDestino) return BadRequest($"El sistema de destino '{transferenciaDTO.IdSistemaCalidadDestino}' no existe.");

//            // ==============================================================================
//            // 2. CREAR Y GUARDAR LA TRANSFERENCIA (PADRE)
//            // ==============================================================================
//            var transferencia = new Transferencia
//            {
//                IdDispositivo = transferenciaDTO.IdDispositivo,
//                UsuarioResponsable = transferenciaDTO.UsuarioResponsable,
//                Estatus = transferenciaDTO.Estatus,
//                Prioridad = transferenciaDTO.Prioridad,
//                SitioOrigen = transferenciaDTO.SitioOrigen,
//                SitioDestino = transferenciaDTO.SitioDestino,
//                IdSistemaCalidadOrigen = transferenciaDTO.IdSistemaCalidadOrigen,
//                IdSistemaCalidadDestino = transferenciaDTO.IdSistemaCalidadDestino,
//                IdLugarComercializacion = transferenciaDTO.IdLugarComercializacion,
//                FechaInicioEstimada = transferenciaDTO.FechaInicioEstimada,
//                FechaFinEstimada = transferenciaDTO.FechaFinEstimada
//            };

//            _context.Transferencias.Add(transferencia);

//            // Guardamos para que SQL genere el ID (ej: 50)
//            await _context.SaveChangesAsync();

//            // ==============================================================================
//            // 3. LÓGICA DE NEGOCIO "SENIOR" (BUSINESS CASE + ROI)
//            // ==============================================================================

//            // A. Leemos la regla de negocio desde configuración (Sin números fijos en código)
//            // Si no encuentra el valor en el archivo, usa 0.15 (15%) como respaldo.
//            decimal roiMinimo = _configuration.GetValue<decimal>("ReglasNegocio:RoiMinimo", 0.15m);

//            // B. Cálculos Financieros
//            decimal ahorroUnitario = transferenciaDTO.CostoUnitarioOrigen - transferenciaDTO.CostoUnitarioDestino;
//            decimal ahorroTotal = ahorroUnitario * transferenciaDTO.VolumenProduccion;

//            decimal inversionTotal = transferenciaDTO.CostoCapacidadInstalada
//                                   + transferenciaDTO.CostoCalidadPreventiva
//                                   + transferenciaDTO.FondoRiesgo;

//            // C. Decisión de Viabilidad Inteligente
//            bool esViableCalc = false;
//            string motivo = "";

//            if (ahorroUnitario < 0)
//            {
//                esViableCalc = false;
//                motivo = "No viable: El costo unitario en destino es mayor.";
//            }
//            else if (inversionTotal > 0)
//            {
//                // Fórmula de ROI: (Ganancia - Inversión) / Inversión
//                decimal gananciaNeta = ahorroTotal - inversionTotal;
//                decimal roi = gananciaNeta / inversionTotal;

//                if (roi >= roiMinimo)
//                {
//                    esViableCalc = true;
//                    motivo = $"Rentable. ROI ({roi:P2}) supera el mínimo corporativo ({roiMinimo:P2}).";
//                }
//                else
//                {
//                    esViableCalc = false;
//                    motivo = $"Riesgo financiero. ROI ({roi:P2}) es menor al mínimo requerido ({roiMinimo:P2}).";
//                }
//            }
//            else
//            {
//                // Si no hay inversión y hay ahorro, es viable.
//                if (ahorroTotal > 0) esViableCalc = true;
//            }

//            // D. Guardar Business Case (Hijo 1)
//            var businessCase = new BusinessCase
//            {
//                IdTransferencia = transferencia.IdTransferencia, // 🔗 Enlace con el ID 50

//                VolumenProduccion = transferenciaDTO.VolumenProduccion,
//                TiempoRampUpMeses = transferenciaDTO.TiempoRampUpMeses,
//                CostoUnitarioOrigen = transferenciaDTO.CostoUnitarioOrigen,
//                CostoUnitarioDestino = transferenciaDTO.CostoUnitarioDestino,
//                CostoCapacidadInstalada = transferenciaDTO.CostoCapacidadInstalada,
//                CostoCalidadPreventiva = transferenciaDTO.CostoCalidadPreventiva,
//                FondoRiesgo = transferenciaDTO.FondoRiesgo,
//                AplicaNoRequerido = transferenciaDTO.AplicaNoRequerido,

//                AhorroTotalProyectado = ahorroTotal,
//                EsViable = esViableCalc
//            };
//            _context.BusinessCases.Add(businessCase);

//            // ==============================================================================
//            // 4. GUARDAR ESTÁNDARES (HIJO 2)
//            // ==============================================================================
//            if (transferenciaDTO.EstandaresSeleccionados != null && transferenciaDTO.EstandaresSeleccionados.Any())
//            {
//                var listaEstandares = new List<TransferenciaEstandar>();
//                foreach (var idEstandar in transferenciaDTO.EstandaresSeleccionados)
//                {
//                    listaEstandares.Add(new TransferenciaEstandar
//                    {
//                        IdTransferencia = transferencia.IdTransferencia, // 🔗 Enlace con el ID 50
//                        IdEstandar = idEstandar
//                    });
//                }
//                _context.TransferenciaEstandares.AddRange(listaEstandares);
//            }

//            // ==============================================================================
//            // 5. GUARDADO FINAL Y RESPUESTA
//            // ==============================================================================

//            // Guardamos los hijos (Business Case y Estándares)
//            await _context.SaveChangesAsync();

//            return Ok(new
//            {
//                Mensaje = "Transferencia creada correctamente",
//                IdTransferencia = transferencia.IdTransferencia,
//                Folio = transferencia.IdTransferencia,
//                Usuario = transferencia.UsuarioResponsable,

//                // Devolvemos el análisis para que el Front pueda mostrarlo en una alerta o modal
//                AnalisisFinanciero = new
//                {
//                    Resultado = esViableCalc ? "VIABLE" : "NO VIABLE",
//                    Motivo = motivo,
//                    AhorroProyectado = ahorroTotal
//                }
//            });
//        }

//        [HttpPut("{id}")]
//        [Authorize]
//        public async Task<IActionResult> UpdateTransferencia(int id, [FromBody] TransferenciaDto dto)
//        {
//            // ==============================================================================
//            // 1. BUSCAR LA TRANSFERENCIA EXISTENTE (CON SUS HIJOS)
//            // ==============================================================================
//            var transferencia = await _context.Transferencias
//                .Include(t => t.BusinessCase)
//                .Include(t => t.TransferenciaEstandares)
//                .FirstOrDefaultAsync(t => t.IdTransferencia == id);

//            if (transferencia == null) return NotFound("Transferencia no encontrada.");

//            // Validar sistemas de calidad
//            var existeOrigen = await _context.SistemasCalidad.AnyAsync(s => s.IdSistema == dto.IdSistemaCalidadOrigen);
//            var existeDestino = await _context.SistemasCalidad.AnyAsync(s => s.IdSistema == dto.IdSistemaCalidadDestino);
//            if (!existeOrigen || !existeDestino) return BadRequest("Sistemas de calidad no válidos.");

//            // ==============================================================================
//            // 2. ACTUALIZAR CAMPOS DEL PADRE (TRANSFERENCIA)
//            // ==============================================================================
//            transferencia.IdDispositivo = dto.IdDispositivo;
//            transferencia.Estatus = dto.Estatus;
//            transferencia.Prioridad = dto.Prioridad;
//            transferencia.SitioOrigen = dto.SitioOrigen;
//            transferencia.SitioDestino = dto.SitioDestino;
//            transferencia.IdSistemaCalidadOrigen = dto.IdSistemaCalidadOrigen;
//            transferencia.IdSistemaCalidadDestino = dto.IdSistemaCalidadDestino;
//            transferencia.IdLugarComercializacion = dto.IdLugarComercializacion;
//            transferencia.FechaInicioEstimada = dto.FechaInicioEstimada;
//            transferencia.FechaFinEstimada = dto.FechaFinEstimada;

//            // ==============================================================================
//            // 3. RECALCULAR LÓGICA DE NEGOCIO "SENIOR" (ROI)
//            // ==============================================================================
//            decimal roiMinimo = _configuration.GetValue<decimal>("ReglasNegocio:RoiMinimo", 0.15m);

//            decimal ahorroUnitario = dto.CostoUnitarioOrigen - dto.CostoUnitarioDestino;
//            decimal ahorroTotal = ahorroUnitario * dto.VolumenProduccion;
//            decimal inversionTotal = dto.CostoCapacidadInstalada
//                                   + dto.CostoCalidadPreventiva
//                                   + dto.FondoRiesgo;

//            bool esViableCalc = false;
//            string motivo = "";

//            if (ahorroUnitario < 0)
//            {
//                esViableCalc = false;
//                motivo = "No viable: El costo unitario en destino es mayor.";
//            }
//            else if (inversionTotal > 0)
//            {
//                decimal gananciaNeta = ahorroTotal - inversionTotal;
//                decimal roi = gananciaNeta / inversionTotal;

//                if (roi >= roiMinimo)
//                {
//                    esViableCalc = true;
//                    motivo = $"Rentable. ROI ({roi:P2}) supera el mínimo corporativo ({roiMinimo:P2}).";
//                }
//                else
//                {
//                    esViableCalc = false;
//                    motivo = $"Riesgo financiero. ROI ({roi:P2}) es menor al mínimo requerido ({roiMinimo:P2}).";
//                }
//            }
//            else
//            {
//                if (ahorroTotal > 0) esViableCalc = true;
//            }

//            // ==============================================================================
//            // 4. ACTUALIZAR HIJO 1 (BUSINESS CASE)
//            // ==============================================================================
//            if (transferencia.BusinessCase != null)
//            {
//                transferencia.BusinessCase.VolumenProduccion = dto.VolumenProduccion;
//                transferencia.BusinessCase.TiempoRampUpMeses = dto.TiempoRampUpMeses;
//                transferencia.BusinessCase.CostoUnitarioOrigen = dto.CostoUnitarioOrigen;
//                transferencia.BusinessCase.CostoUnitarioDestino = dto.CostoUnitarioDestino;
//                transferencia.BusinessCase.CostoCapacidadInstalada = dto.CostoCapacidadInstalada;
//                transferencia.BusinessCase.CostoCalidadPreventiva = dto.CostoCalidadPreventiva;
//                transferencia.BusinessCase.FondoRiesgo = dto.FondoRiesgo;
//                transferencia.BusinessCase.AplicaNoRequerido = dto.AplicaNoRequerido;

//                transferencia.BusinessCase.AhorroTotalProyectado = ahorroTotal;
//                transferencia.BusinessCase.EsViable = esViableCalc;
//            }
//            else
//            {
//                var nuevoBc = new BusinessCase
//                {
//                    IdTransferencia = id,
//                    VolumenProduccion = dto.VolumenProduccion,
//                    TiempoRampUpMeses = dto.TiempoRampUpMeses,
//                    CostoUnitarioOrigen = dto.CostoUnitarioOrigen,
//                    CostoUnitarioDestino = dto.CostoUnitarioDestino,
//                    CostoCapacidadInstalada = dto.CostoCapacidadInstalada,
//                    CostoCalidadPreventiva = dto.CostoCalidadPreventiva,
//                    FondoRiesgo = dto.FondoRiesgo,
//                    AplicaNoRequerido = dto.AplicaNoRequerido,
//                    AhorroTotalProyectado = ahorroTotal,
//                    EsViable = esViableCalc
//                };
//                _context.BusinessCases.Add(nuevoBc);
//            }

//            // ==============================================================================
//            // 5. ACTUALIZAR HIJO 2 (ESTÁNDARES)
//            // ==============================================================================
//            if (transferencia.TransferenciaEstandares != null)
//            {
//                _context.TransferenciaEstandares.RemoveRange(transferencia.TransferenciaEstandares);
//            }

//            if (dto.EstandaresSeleccionados != null && dto.EstandaresSeleccionados.Any())
//            {
//                foreach (var idEst in dto.EstandaresSeleccionados)
//                {
//                    transferencia.TransferenciaEstandares.Add(new TransferenciaEstandar
//                    {
//                        IdTransferencia = id,
//                        IdEstandar = idEst
//                    });
//                }
//            }

//            // ==============================================================================
//            // 6. GUARDAR CAMBIOS Y RESPONDER
//            // ==============================================================================
//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!_context.Transferencias.Any(e => e.IdTransferencia == id)) return NotFound();
//                else throw;
//            }

//            return Ok(new
//            {
//                Mensaje = "Transferencia actualizada y recalculada exitosamente",
//                IdTransferencia = id,
//                Folio = transferencia.IdTransferencia,

//                AnalisisFinanciero = new
//                {
//                    Resultado = esViableCalc ? "VIABLE" : "NO VIABLE",
//                    Motivo = motivo,
//                    AhorroProyectado = ahorroTotal
//                }
//            });
//        }

//        [HttpGet("reporte/{id}")]// Ruta: GET api/transferencias/reporte/11
//        [Authorize]
//        public async Task<IActionResult> GetReporteTransferencia(int id)
//        {
//            // 1. Buscamos la info completa usando Include para traer los hijos
//            var transferencia = await _context.Transferencias
//                .Include(t => t.BusinessCase) // 👈 OBLIGATORIO: Traer la tabla financiera
//                .FirstOrDefaultAsync(t => t.IdTransferencia == id);

//            if (transferencia == null) return NotFound("Transferencia no encontrada.");

//            // 2. Mapeamos los datos para que queden listos para la pantalla
//            var reporte = new TransferenciaReporteDto
//            {
//                IdTransferencia = transferencia.IdTransferencia,

//                // Generamos el Folio con formato (TR-HIST-001)
//                Folio = $"TR-HIST-{transferencia.IdTransferencia:D3}",

//                SolicitadoPor = transferencia.UsuarioResponsable,
//                // Formateamos la fecha corta como en la imagen
//                FechaCreacion = transferencia.FechaCreacion.ToString("yyyy-MM-dd"),

//                // Datos Técnicos
//                SitioOrigen = transferencia.SitioOrigen,
//                SistemaCalidadOrigen = transferencia.IdSistemaCalidadOrigen, // Muestra "JP" o "Calidad: JP"
//                SitioDestino = transferencia.SitioDestino,
//                SistemaCalidadDestino = transferencia.IdSistemaCalidadDestino, // Muestra "MX"
//                Comercializacion = transferencia.IdLugarComercializacion,

//                // Datos del Business Case (Usamos '?' por si acaso no tiene BC guardado)
//                Volumen = transferencia.BusinessCase?.VolumenProduccion ?? 0,
//                RampUpMeses = transferencia.BusinessCase?.TiempoRampUpMeses ?? 0,

//                // Aquí mapeamos la "Capacidad" (que es el Costo de Capacidad Instalada)
//                CapacidadInversion = transferencia.BusinessCase?.CostoCapacidadInstalada ?? 0,

//                // Aquí mapeamos el "Ahorro" (que calculamos al guardar)
//                AhorroProyectado = transferencia.BusinessCase?.AhorroTotalProyectado ?? 0,

//                // Lógica visual para la barra verde/roja
//                EsViable = transferencia.BusinessCase?.EsViable ?? false,
//                MensajeViabilidad = (transferencia.BusinessCase?.EsViable ?? false) ? "PROYECTO VIABLE" : "NO VIABLE"
//            };

//            return Ok(reporte);
//        }
//    }
//}
#endregion
