using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    public class BusinessCase
    {
        [Key]
        public int IdBusinessCase { get; set; }

        public int IdTransferencia { get; set; }

        [ForeignKey("IdTransferencia")]
        public virtual Transferencia Transferencia { get; set; }

        // Inputs
        public int VolumenProduccion { get; set; }
        public int TiempoRampUpMeses { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoUnitarioOrigen { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoUnitarioDestino { get; set; }

        // Costos Proyecto
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoCapacidadInstalada { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoCalidadPreventiva { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FondoRiesgo { get; set; }

        // Resultados
        public bool AplicaNoRequerido { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AhorroTotalProyectado { get; set; }

        public bool EsViable { get; set; }
    }

}
