using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDevicesMedical.Models
{
    /// <summary>
    /// Entidad que contiene el análisis financiero y de viabilidad de una Transferencia.
    /// Calcula si el traslado de manufactura es rentable comparando costos de origen vs destino.
    /// </summary>
    public class BusinessCase
    {
        // --- IDENTIFICADOR ---
        [Key]
        public int IdBusinessCase { get; set; }

        // --- RELACIÓN CON TRANSFERENCIA ---
        // Relación 1:1 (o 1:N). Un Business Case pertenece a una Transferencia específica.
        public int IdTransferencia { get; set; }

        [ForeignKey("IdTransferencia")]
        public virtual Transferencia Transferencia { get; set; }

        // --- INPUTS (DATOS DE ENTRADA) ---
        
        /// <summary>
        /// Cantidad de unidades a producir anualmente tras la transferencia.
        /// </summary>
        public int VolumenProduccion { get; set; }

        /// <summary>
        /// Tiempo estimado en meses para alcanzar la capacidad productiva total (Curva de aprendizaje).
        /// </summary>
        public int TiempoRampUpMeses { get; set; }

        /// <summary>
        /// Costo de fabricar una unidad en el sitio actual (Antes de transferir).
        /// Uso de 'decimal' para precisión financiera.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoUnitarioOrigen { get; set; }

        /// <summary>
        /// Costo estimado de fabricar una unidad en el nuevo sitio.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoUnitarioDestino { get; set; }

        // --- COSTOS DEL PROYECTO (INVERSIÓN) ---
        
        /// <summary>
        /// Inversión necesaria en maquinaria, edificio o adecuaciones (CAPEX).
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoCapacidadInstalada { get; set; }

        /// <summary>
        /// Costos asociados a validaciones, certificaciones y cumplimiento normativo.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoCalidadPreventiva { get; set; }

        /// <summary>
        /// Monto reservado para contingencias o imprevistos durante el proyecto.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal FondoRiesgo { get; set; }

        // --- RESULTADOS (SALIDAS CALCULADAS) ---
        
        /// <summary>
        /// Flag para indicar si el Business Case fue exento o no aplica.
        /// A veces, transferencias regulatorias se hacen por obligación, no por ahorro.
        /// </summary>
        public bool AplicaNoRequerido { get; set; }

        /// <summary>
        /// Resultado del cálculo: (Ahorro Unitario * Volumen) - Inversión.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal AhorroTotalProyectado { get; set; }

        /// <summary>
        /// Veredicto final: ¿El proyecto se aprueba financieramente?
        /// </summary>
        public bool EsViable { get; set; }
    }
}
