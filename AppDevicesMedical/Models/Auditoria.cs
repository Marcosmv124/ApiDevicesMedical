using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class Auditoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Mapea a IDENTITY(1,1)
        public int Id_log { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        // El ID de usuario de ASP.NET Identity es un string largo (NVARCHAR(450) por defecto).
        [Required]
        [StringLength(450)]
        public string Id_usuario { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Accion { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Modulo { get; set; } = string.Empty;

        // Mapea a TEXT en SQL Server
        public string? Detalles { get; set; }

        [StringLength(50)]
        public string? Ip_address { get; set; }

        //// Propiedad de Navegación a la tabla de usuarios (AspNetUsers/ApplicationUser)
        //[ForeignKey("Id_usuario")]
        //public ApplicationUser? Usuario { get; set; }
    }
}
