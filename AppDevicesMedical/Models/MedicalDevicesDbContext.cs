using Microsoft.EntityFrameworkCore;
using AppDevicesMedical.Models;

namespace AppDevicesMedical.Models
{
    public class MedicalDevicesDbContext : DbContext
    {
        public MedicalDevicesDbContext(DbContextOptions<MedicalDevicesDbContext> options)
            : base(options) { }

        // DbSets para tus tablas principales
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Especialidad> Especialidad { get; set; }
        public DbSet<Cuarto> Cuarto { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Normas> Normas { get; set; }
        public DbSet<Dispositivosdev> Dispositivodv { get; set; }
        public DbSet<Auditoria> Auditoria { get; set; }
        public DbSet<Examen> Examenes { get; set; }
        public DbSet<TipoDispositivo> TipoDispositivos { get; set; }
        public DbSet<ClaseRiesgo> ClaseRiesgo { get; set; }

        // ╔══════════════════════════════════════════╗
        // ║         CONFIGURACIÓN DE PERMISOS        ║
        // ╚══════════════════════════════════════════╝
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<RolPermiso> RolPermisos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración de unicidad en Usuario
            builder.Entity<Usuario>()
                .HasIndex(u => u.NumeroEmpleado)
                .IsUnique();

            // Evitar autogeneración de IDs en catálogos
            builder.Entity<Rol>()
                .Property(r => r.Id_rol)
                .ValueGeneratedNever();

            builder.Entity<Status>()
                .Property(s => s.Id_status)
                .ValueGeneratedNever();

            // ╔══════════════════════════════════════════╗
            // ║   CONFIGURACIÓN DE ROL-PERMISO (JOIN)    ║
            // ╚══════════════════════════════════════════╝
            builder.Entity<RolPermiso>()
                .HasKey(rp => new { rp.IdRol, rp.IdPermiso });

            builder.Entity<RolPermiso>()
                .HasOne(rp => rp.Rol)
                .WithMany(r => r.RolPermisos)
                .HasForeignKey(rp => rp.IdRol);

            builder.Entity<RolPermiso>()
                .HasOne(rp => rp.Permiso)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(rp => rp.IdPermiso);

            // IDs autogenerados en tablas operativas
            builder.Entity<Cuarto>()
                .Property(c => c.Id_cuarto)
                .ValueGeneratedOnAdd();

            builder.Entity<Dispositivosdev>()
                .Property(d => d.Id_dispositivo)
                .ValueGeneratedOnAdd();

            builder.Entity<TipoDispositivo>()
                .Property(t => t.Id_tipo)
                .ValueGeneratedOnAdd();

            // ╔══════════════════════════════════════════╗
            // ║   SEED DATA: ROLES BÁSICOS               ║
            // ╚══════════════════════════════════════════╝
            builder.Entity<Rol>().HasData(
                new Rol { Id_rol = 1, Nombre_rol = "Admin", Descripcion = "Administrador del sistema" },
                new Rol { Id_rol = 2, Nombre_rol = "Supervisor", Descripcion = "Supervisor de área" },
                new Rol { Id_rol = 3, Nombre_rol = "Tecnico", Descripcion = "Técnico de dispositivos" }
            );
        }
    }
}