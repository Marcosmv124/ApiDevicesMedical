using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // 👈 CAMBIO OBLIGATORIO: El nativo de .NET

namespace AppDevicesMedical.DTOs
{
    public class TransferenciaDto
    {
        // ==========================================
        // PARTE 1: DATOS DE LA TRANSFERENCIA
        // ==========================================

        [Required]
        public int IdDispositivo { get; set; }

        // Al usar System.Text.Json.Serialization, este atributo ahora SÍ funcionará
        [JsonIgnore]
        public string? UsuarioResponsable { get; set; }

        [Required]
        public string Estatus { get; set; }
        public string Prioridad { get; set; }
        public string? SitioOrigen { get; set; }
        public string? SitioDestino { get; set; }

        [Required]
        public string IdSistemaCalidadOrigen { get; set; }
        [Required]
        public string IdSistemaCalidadDestino { get; set; }

        public string? IdLugarComercializacion { get; set; }
        public DateTime? FechaInicioEstimada { get; set; }
        public DateTime? FechaFinEstimada { get; set; }

        public List<int> EstandaresSeleccionados { get; set; } = new List<int>();

        // ==========================================
        // PARTE 2: DATOS DEL BUSINESS CASE
        // ==========================================

        [Range(1, int.MaxValue, ErrorMessage = "El volumen debe ser mayor a 0")]
        public int VolumenProduccion { get; set; }

        public int TiempoRampUpMeses { get; set; }

        // Decimal es correcto para dinero
        public decimal CostoUnitarioOrigen { get; set; }
        public decimal CostoUnitarioDestino { get; set; }

        public decimal CostoCapacidadInstalada { get; set; }
        public decimal CostoCalidadPreventiva { get; set; }
        public decimal FondoRiesgo { get; set; }

        public bool AplicaNoRequerido { get; set; }
    }
}