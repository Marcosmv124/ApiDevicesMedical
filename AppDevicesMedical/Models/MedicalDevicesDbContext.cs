using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using AppDevicesMedical.Models;

namespace AppDevicesMedical.Models
{
    // Hereda las propiedades de login: Id, UserName, PasswordHash
    public class MedicalDevicesDbContext : DbContext
    {
        public MedicalDevicesDbContext(DbContextOptions<MedicalDevicesDbContext> options)
            : base(options) { }

        // DbSets para tus tablas
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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Ya no es necesario llamar a base.OnModelCreating(builder) de Identity

            // Configuraciones de unicidad
            builder.Entity<Usuario>()
                .HasIndex(u => u.NumeroEmpleado)
                .IsUnique();

            // Configuraciones para evitar autogeneración de IDs (si son IDs de catálogos)
            builder.Entity<Rol>()
                .Property(r => r.Id_rol)
                .ValueGeneratedNever();

            builder.Entity<Status>()
                .Property(s => s.Id_status)
                .ValueGeneratedNever();
            // ✅ CONFIGURACIÓN FALTANTE PARA CUARTO
            builder.Entity<Cuarto>()
                .Property(c => c.Id_cuarto)
                .ValueGeneratedOnAdd(); // Esto hace que el ID sea auto-incremental
            builder.Entity<Dispositivosdev>()
               .Property(c => c.Id_dispositivo)
               .ValueGeneratedOnAdd(); // Esto hace que el ID sea auto-incremental

            builder.Entity<Rol>().HasData(
            new Rol { Id_rol = 1, Nombre_rol = "Admin", Descripcion = "Administrador del sistema" },
            new Rol { Id_rol = 2, Nombre_rol = "Supervisor", Descripcion = "Supervisor de área" },
             new Rol { Id_rol = 3, Nombre_rol = "Tecnico", Descripcion = "Técnico de dispositivos" }
            );
        }
    // Hereda las propiedades de login: Id, UserName, PasswordHash
public DbSet<AppDevicesMedical.Models.ClaseRiesgo> ClaseRiesgo { get; set; } = default!;
    }
}
