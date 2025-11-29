using AppDevicesMedical.Authorization; // ← Importante para usar [Permiso]
using AppDevicesMedical.DTOs;
using AppDevicesMedical.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDevicesMedical.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolsController : ControllerBase
    {
        private readonly MedicalDevicesDbContext _context;

        public RolsController(MedicalDevicesDbContext context)
        {
            _context = context;
        }

        // ================= CRUD ROLES =================

        // GET: api/Rols
        [Permiso("VER_ROLES")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRol()
        {
            return await _context.Rol.ToListAsync();
        }

        // GET: api/Rols/5
        [Permiso("VER_ROLES")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            var rol = await _context.Rol.FindAsync(id);
            if (rol == null)
                return NotFound();
            return rol;
        }

        // PUT: api/Rols/5
        [Permiso("EDITAR_ROLES")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, Rol rol)
        {
            if (id != rol.Id_rol)
                return BadRequest();

            _context.Entry(rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Rols
        [Permiso("CREAR_ROLES")]
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol(Rol rol)
        {
            _context.Rol.Add(rol);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RolExists(rol.Id_rol))
                    return Conflict();
                else
                    throw;
            }

            return CreatedAtAction("GetRol", new { id = rol.Id_rol }, rol);
        }

        // DELETE: api/Rols/5
        [Permiso("ELIMINAR_ROLES")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Rol.FindAsync(id);
            if (rol == null)
                return NotFound();

            _context.Rol.Remove(rol);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ================= PERMISOS =================

        //// POST: api/Rols/{idRol}/permisos
        //[Permiso("GESTIONAR_PERMISOS")]
        //[HttpPost("{idRol}/permisos")]
        //public async Task<IActionResult> ActualizarPermisosDeRol(int idRol, [FromBody] AsignarPermisosDto permisosDto)
        //{
        //    var rol = await _context.Rol.FindAsync(idRol);
        //    if (rol == null)
        //        return NotFound();

        //    var permisosActuales = await _context.RolPermisos
        //        .Where(rp => rp.IdRol == idRol)
        //        .ToListAsync();

        //    var idsNuevos = permisosDto.IdPermisos.ToHashSet();

        //    // eliminar los que ya no están
        //    var aEliminar = permisosActuales
        //        .Where(rp => !idsNuevos.Contains(rp.IdPermiso))
        //        .ToList();
        //    _context.RolPermisos.RemoveRange(aEliminar);

        //    // agregar los nuevos
        //    var idsActuales = permisosActuales.Select(rp => rp.IdPermiso).ToHashSet();
        //    var aAgregar = idsNuevos
        //        .Where(id => !idsActuales.Contains(id))
        //        .Select(id => new RolPermiso { IdRol = idRol, IdPermiso = id });

        //    await _context.RolPermisos.AddRangeAsync(aAgregar);
        //    await _context.SaveChangesAsync();

        //    return Ok("Permisos actualizados correctamente.");
        //}

        // GET: api/Rols/{idRol}/permisos
        [Permiso("VER_PERMISOS")]
        [HttpGet("{idRol}/permisos")]
        public async Task<ActionResult<IEnumerable<Permiso>>> GetPermisosDeRol(int idRol)
        {
            var permisos = await _context.RolPermisos
                .Where(rp => rp.IdRol == idRol)
                .Select(rp => rp.Permiso)
                .ToListAsync();

            return Ok(permisos);
        }

        //// DELETE: api/Rols/{idRol}/permisos/{idPermiso}
        //[Permiso("GESTIONAR_PERMISOS")]
        //[HttpDelete("{idRol}/permisos/{idPermiso}")]
        //public async Task<IActionResult> RemoverPermisoDeRol(int idRol, int idPermiso)
        //{
        //    var rolPermiso = await _context.RolPermisos
        //        .FirstOrDefaultAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);

        //    if (rolPermiso == null)
        //        return NotFound();

        //    _context.RolPermisos.Remove(rolPermiso);
        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}

        // GET: api/Rols/permisos
        [Permiso("VER_PERMISOS")]
        [HttpGet("permisos")]
        public async Task<ActionResult<IEnumerable<Permiso>>> GetTodosLosPermisos()
        {
            var permisos = await _context.Permisos.ToListAsync();
            return Ok(permisos);
        }

        //// POST: api/Rols/{idRol}/permisos/todos
        //[Permiso("GESTIONAR_PERMISOS")]
        //[HttpPost("{idRol}/permisos/todos")]
        //public async Task<IActionResult> AsignarTodosLosPermisosARol(int idRol)
        //{
        //    var rol = await _context.Rol.FindAsync(idRol);
        //    if (rol == null)
        //        return NotFound();

        //    var permisosNoAsignados = await _context.Permisos
        //        .Where(p => !_context.RolPermisos
        //            .Any(rp => rp.IdRol == idRol && rp.IdPermiso == p.IdPermiso))
        //        .ToListAsync();

        //    if (!permisosNoAsignados.Any())
        //        return Ok();

        //    foreach (var permiso in permisosNoAsignados)
        //    {
        //        _context.RolPermisos.Add(new RolPermiso
        //        {
        //            IdRol = idRol,
        //            IdPermiso = permiso.IdPermiso
        //        });
        //    }

        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        // ================= NUEVO ENDPOINT: Crear/Editar rol con permisos usando booleans =================

        [Permiso("CREAR_ROLES")]
        [HttpPost("rol-con-permisos")]
        public async Task<IActionResult> CrearRolConPermisos([FromBody] GuardarRolConPermisosDto dto)
        {
            if (dto == null) return BadRequest("No se recibió ningún rol.");

            // Validar que el IdRol sea único si tu DB no es identity
            if (_context.Rol.Any(r => r.Id_rol == dto.IdRol))
                return Conflict("El IdRol ya existe.");

            // Crear rol
            var rol = new Rol
            {
                Id_rol = dto.IdRol.Value,
                Nombre_rol = dto.Nombre,
                Descripcion = dto.Descripcion
            };
            _context.Rol.Add(rol);
            await _context.SaveChangesAsync();

            var idRol = rol.Id_rol;

            // Asignar permisos
            var idsPermisos = new HashSet<int>();
            foreach (var p in dto.Permisos)
            {
                if (p.Activo)
                {
                    var permiso = await _context.Permisos.FirstOrDefaultAsync(x => x.Nombre == p.Nombre);
                    if (permiso != null) idsPermisos.Add(permiso.IdPermiso);
                }
            }

            var rolPermisos = idsPermisos.Select(id => new RolPermiso { IdRol = idRol, IdPermiso = id });
            await _context.RolPermisos.AddRangeAsync(rolPermisos);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                idRol,
                mensaje = "Rol creado y permisos asignados correctamente."
            });

        }
        [Permiso("GESTIONAR_PERMISOS")]
        [HttpPut("{idRol}/permisos")]
        public async Task<IActionResult> ActualizarPermisosDeRol(int idRol, [FromBody] List<PermisoRolDto> permisosDto)
        {
            var rol = await _context.Rol.FindAsync(idRol);
            if (rol == null) return NotFound("El rol no existe.");

            var permisosActuales = await _context.RolPermisos.Where(rp => rp.IdRol == idRol).ToListAsync();

            var idsNuevos = new HashSet<int>();
            foreach (var p in permisosDto)
            {
                if (p.Activo)
                {
                    var permiso = await _context.Permisos.FirstOrDefaultAsync(x => x.Nombre == p.Nombre);
                    if (permiso != null) idsNuevos.Add(permiso.IdPermiso);
                }
            }

            // Eliminar los permisos que ya no están
            var aEliminar = permisosActuales.Where(rp => !idsNuevos.Contains(rp.IdPermiso)).ToList();
            _context.RolPermisos.RemoveRange(aEliminar);

            // Agregar los permisos nuevos
            var idsActuales = permisosActuales.Select(rp => rp.IdPermiso).ToHashSet();
            var aAgregar = idsNuevos.Where(id => !idsActuales.Contains(id))
                                    .Select(id => new RolPermiso { IdRol = idRol, IdPermiso = id });

            await _context.RolPermisos.AddRangeAsync(aAgregar);
            await _context.SaveChangesAsync();

            return Ok("Permisos actualizados correctamente.");
        }

        // ================= FIN DEL CONTROLADOR =================

        private bool RolExists(int id)
        {
            return _context.Rol.Any(e => e.Id_rol == id);
        }

        // POST: api/Rols/{idRol}/permisos/todos
        [HttpPost("{idRol}/permisos/todos")]
        public async Task<IActionResult> AsignarTodosLosPermisosARol(int idRol)
        {
            var rol = await _context.Rol.FindAsync(idRol);
            if (rol == null)
                return NotFound($"No se encontró el rol con ID {idRol}.");

            // Selecciona todos los permisos que aún no están asignados
            var permisosNoAsignados = await _context.Permisos
                .Where(p => !_context.RolPermisos
                    .Any(rp => rp.IdRol == idRol && rp.IdPermiso == p.IdPermiso))
                .ToListAsync();

            if (!permisosNoAsignados.Any())
                return Ok("El rol ya tiene todos los permisos asignados.");

            foreach (var permiso in permisosNoAsignados)
            {
                _context.RolPermisos.Add(new RolPermiso
                {
                    IdRol = idRol,
                    IdPermiso = permiso.IdPermiso
                });
            }

            await _context.SaveChangesAsync();

            return Ok($"Se asignaron {permisosNoAsignados.Count} permisos al rol '{rol.Nombre_rol}'.");
        }
    }

}

//// PUT: api/Rols/{idRol}/permisos
//[Permiso("GESTIONAR_PERMISOS")]
//[HttpPut("{idRol}/permisos")]
//public async Task<IActionResult> ActualizarPermisosDeRol(int idRol, [FromBody] AsignarPermisosDto permisosDto)
//{
//    var rol = await _context.Rol.FindAsync(idRol);
//    if (rol == null)
//        return NotFound();

//    // permisos actuales
//    var permisosActuales = await _context.RolPermisos
//        .Where(rp => rp.IdRol == idRol)
//        .ToListAsync();

//    var idsNuevos = permisosDto.IdPermisos.ToHashSet();

//    // eliminar los que ya no están
//    var aEliminar = permisosActuales
//        .Where(rp => !idsNuevos.Contains(rp.IdPermiso))
//        .ToList();

//    _context.RolPermisos.RemoveRange(aEliminar);

//    // agregar los que faltan
//    var idsActuales = permisosActuales.Select(rp => rp.IdPermiso).ToHashSet();
//    var aAgregar = idsNuevos
//        .Where(id => !idsActuales.Contains(id))
//        .Select(id => new RolPermiso { IdRol = idRol, IdPermiso = id });

//    await _context.RolPermisos.AddRangeAsync(aAgregar);
//    await _context.SaveChangesAsync();

//    return Ok("Permisos actualizados correctamente.");
//}

//// -------------------------------
//// ASIGNACIÓN DE PERMISOS A ROLES
//// -------------------------------

//// POST: api/Rols/{idRol}/permisos
//[Permiso("GESTIONAR_PERMISOS")]
//[HttpPost("{idRol}/permisos")]
//public async Task<IActionResult> AsignarPermisoARol(int idRol, [FromBody] int idPermiso)
//{
//    var rol = await _context.Rol.FindAsync(idRol);
//    if (rol == null)
//        return NotFound($"No se encontró el rol con ID {idRol}.");

//    var permiso = await _context.Permisos.FindAsync(idPermiso);
//    if (permiso == null)
//        return NotFound($"No se encontró el permiso con ID {idPermiso}.");

//    var existe = await _context.RolPermisos
//        .AnyAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);

//    if (existe)
//        return Conflict("Ese permiso ya está asignado a este rol.");

//    _context.RolPermisos.Add(new RolPermiso { IdRol = idRol, IdPermiso = idPermiso });
//    await _context.SaveChangesAsync();

//    return Ok($"Permiso '{permiso.Nombre}' asignado al rol '{rol.Nombre_rol}'.");
//}

//// GET: api/Rols/{idRol}/permisos
//[Permiso("VER_PERMISOS")]
//[HttpGet("{idRol}/permisos")]
//public async Task<ActionResult<IEnumerable<Permiso>>> GetPermisosDeRol(int idRol)
//{
//    var permisos = await _context.RolPermisos
//        .Where(rp => rp.IdRol == idRol)
//        .Select(rp => rp.Permiso)
//        .ToListAsync();

//    return Ok(permisos);
//}

//// DELETE: api/Rols/{idRol}/permisos/{idPermiso}
//[Permiso("GESTIONAR_PERMISOS")]
//[HttpDelete("{idRol}/permisos/{idPermiso}")]
//public async Task<IActionResult> RemoverPermisoDeRol(int idRol, int idPermiso)
//{
//    var rolPermiso = await _context.RolPermisos
//        .FirstOrDefaultAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);

//    if (rolPermiso == null)
//        return NotFound("Ese permiso no está asignado a este rol.");

//    _context.RolPermisos.Remove(rolPermiso);
//    await _context.SaveChangesAsync();

//    return Ok("Permiso removido correctamente.");
//}
//// GET: api/Rols/permisos
//[Permiso("VER_PERMISOS")]
//[HttpGet("permisos")]
//public async Task<ActionResult<IEnumerable<Permiso>>> GetTodosLosPermisos()
//{
//    return await _context.Permisos.ToListAsync();
//}
//// POST: api/Rols/{idRol}/permisos/todos
//[HttpPost("{idRol}/permisos/todos")]
//public async Task<IActionResult> AsignarTodosLosPermisosARol(int idRol)
//{
//    var rol = await _context.Rol.FindAsync(idRol);
//    if (rol == null)
//        return NotFound($"No se encontró el rol con ID {idRol}.");

//    // Selecciona todos los permisos que aún no están asignados
//    var permisosNoAsignados = await _context.Permisos
//        .Where(p => !_context.RolPermisos
//            .Any(rp => rp.IdRol == idRol && rp.IdPermiso == p.IdPermiso))
//        .ToListAsync();

//    if (!permisosNoAsignados.Any())
//        return Ok("El rol ya tiene todos los permisos asignados.");

//    foreach (var permiso in permisosNoAsignados)
//    {
//        _context.RolPermisos.Add(new RolPermiso
//        {
//            IdRol = idRol,
//            IdPermiso = permiso.IdPermiso
//        });
//    }

//    await _context.SaveChangesAsync();

//    return Ok($"Se asignaron {permisosNoAsignados.Count} permisos al rol '{rol.Nombre_rol}'.");
//}
// -------------------------------
// ASIGNACIÓN DE PERMISOS A ROLES
// -------------------------------
