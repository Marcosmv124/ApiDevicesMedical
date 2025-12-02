using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("nombres")]
        [Required]
        [StringLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Column("apellido_paterno")]
        [Required]
        [StringLength(50)]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [Column("apellido_materno")]
        [Required]
        [StringLength(50)]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [Column("numero_empleado")]
        [Required]
        [StringLength(20)] // ⬅️ ahora VARCHAR en BD
        public string NumeroEmpleado { get; set; } = string.Empty;

        [Column("password_hash")]
        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("id_rol")]
        [ForeignKey("Rol")]
        public int IdRol { get; set; }

        [Column("id_status")]
        [ForeignKey("StatusUsuario")]
        public int IdStatus { get; set; }

        [Column("id_especialidad")]
        [ForeignKey("Especialidad")]
        public int IdEspecialidad { get; set; }

        [Column("fecha_creacion")]
        [Required]
        public DateTime FechaCreacion { get; set; }

        [Column("intentos_fallidos")]
        public int IntentosFallidos { get; set; }

        [Column("bloqueo_hasta")]
        public DateTime? BloqueoHasta { get; set; }

        // Propiedades de navegación
        public Rol Rol { get; set; }
        public Status StatusUsuario { get; set; }
        public Especialidad Especialidad { get; set; }
    }
}