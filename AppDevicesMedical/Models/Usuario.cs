using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    /// <summary>
    /// Representa a un usuario del sistema médico.
    /// Mapeado a la tabla que contiene credenciales y datos generales.
    /// </summary>
    public class Usuario
    {
        // --- LLAVE PRIMARIA ---
        [Key] // Indica que es la Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-incremental en SQL
        [Column("id_usuario")] // Mapeo exacto al nombre de columna en BD
        public int IdUsuario { get; set; }

        // --- DATOS GENERALES ---
        [Column("nombres")]
        [Required] // NOT NULL
        [StringLength(100)] // VARCHAR(100)
        public string Nombres { get; set; } = string.Empty; // Inicialización para evitar warnings de nulos

        [Column("apellido_paterno")]
        [Required]
        [StringLength(50)]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [Column("apellido_materno")]
        [Required]
        [StringLength(50)]
        public string ApellidoMaterno { get; set; } = string.Empty;

        // --- CREDENCIALES ---
        [Column("numero_empleado")]
        [Required]
        [StringLength(20)] // VARCHAR(20) - Permite alfanuméricos y ceros a la izquierda
        public string NumeroEmpleado { get; set; } = string.Empty;

        [Column("password_hash")]
        [Required]
        [StringLength(256)] // Espacio suficiente para hashes largos (BCrypt/SHA)
        public string PasswordHash { get; set; } = string.Empty;

        // --- CLAVES FORÁNEAS (RELACIONES) ---
        
        // Relación con tabla Roles
        [Column("id_rol")]
        [ForeignKey("Rol")] // Vincula la propiedad 'IdRol' con la navegación 'Rol'
        public int IdRol { get; set; }

        // Relación con tabla Status (ej. Activo/Baja)
        [Column("id_status")]
        [ForeignKey("StatusUsuario")]
        public int IdStatus { get; set; }

        // Relación con tabla Especialidad (ej. Cardiología)
        [Column("id_especialidad")]
        [ForeignKey("Especialidad")]
        public int IdEspecialidad { get; set; }

        // --- AUDITORÍA Y SEGURIDAD ---
        [Column("fecha_creacion")]
        [Required]
        public DateTime FechaCreacion { get; set; }

        // Lógica para bloqueo de cuenta por intentos fallidos
        [Column("intentos_fallidos")]
        public int IntentosFallidos { get; set; }

        [Column("bloqueo_hasta")]
        public DateTime? BloqueoHasta { get; set; } // Nullable: si es null, no está bloqueado

        // --- PROPIEDADES DE NAVEGACIÓN (VIRTUALES RECOMENDADAS) ---
        // Permiten acceder a los datos de las tablas relacionadas (JOINS automáticos)
        public virtual Rol Rol { get; set; }
        public virtual Status StatusUsuario { get; set; }
        public virtual Especialidad Especialidad { get; set; }
    }
}
