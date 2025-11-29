using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class Transferencia
    {
        [Key]
        public int IdTransferencia { get; set; }

        [Required]
        public int IdDispositivo { get; set; }

        // Relación con tu tabla existente de Dispositivos
        [ForeignKey("IdDispositivo")]
        public virtual Dispositivosdev Dispositivo { get; set; }

        [Required]
        [StringLength(150)]
        public string UsuarioResponsable { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Estatus { get; set; }

        [StringLength(20)]
        public string Prioridad { get; set; }

        // --- Configuración Sitios ---
        [Required]
        [StringLength(200)]
        public string SitioOrigen { get; set; }

        [Required]
        [StringLength(200)]
        public string SitioDestino { get; set; }

        // --- Claves Foráneas Sistemas Calidad ---
        [StringLength(5)]
        public string IdSistemaCalidadOrigen { get; set; }

        [ForeignKey("IdSistemaCalidadOrigen")]
        public virtual SistemaCalidad SistemaCalidadOrigen { get; set; }

        [StringLength(5)]
        public string IdSistemaCalidadDestino { get; set; }

        [ForeignKey("IdSistemaCalidadDestino")]
        public virtual SistemaCalidad SistemaCalidadDestino { get; set; }

        [StringLength(5)]
        public string IdLugarComercializacion { get; set; }

        [ForeignKey("IdLugarComercializacion")]
        public virtual SistemaCalidad LugarComercializacion { get; set; }

        public DateTime? FechaInicioEstimada { get; set; }
        public DateTime? FechaFinEstimada { get; set; }

        // --- Relaciones ---
        public virtual BusinessCase BusinessCase { get; set; }
        public virtual ICollection<TransferenciaEstandar> TransferenciaEstandares { get; set; }
    }
}
