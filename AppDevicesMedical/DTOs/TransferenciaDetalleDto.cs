namespace AppDevicesMedical.DTOs
{
    public class TransferenciaDetalleDto
    {
        // ==========================================
        // PARTE 0: IDENTIFICADOR PRINCIPAL
        // ==========================================
        public int Id { get; set; } // El ID de la tabla Transferencias (Primary Key)

        // ==========================================
        // PARTE 1: DATOS DE LA TRANSFERENCIA
        // ==========================================

        public int IdDispositivo { get; set; }
        public string? NombreDispositivo { get; set; } // <--- NUEVO: Para mostrar el nombre en el input/label

        public string? UsuarioResponsable { get; set; } // <--- YA NO TIENE JsonIgnore (necesitas verlo)

        public string Estatus { get; set; }
        public string Prioridad { get; set; }
        public string? SitioOrigen { get; set; }
        public string? SitioDestino { get; set; }

        public string IdSistemaCalidadOrigen { get; set; }
        public string IdSistemaCalidadDestino { get; set; }

        public string? IdLugarComercializacion { get; set; }
        public DateTime? FechaInicioEstimada { get; set; }
        public DateTime? FechaFinEstimada { get; set; }

        // Mantiene la lista de IDs para que el front marque los checkboxes automáticamente
        public List<int> EstandaresSeleccionados { get; set; } = new List<int>();

        // ==========================================
        // PARTE 2: DATOS DEL BUSINESS CASE
        // ==========================================

        // Nota: En los DTOs de salida (GET), los atributos de validación como [Range]
        // no son necesarios (porque no estás validando entrada), pero dejarlos no afecta.

        public int VolumenProduccion { get; set; }
        public int TiempoRampUpMeses { get; set; }

        public decimal CostoUnitarioOrigen { get; set; }
        public decimal CostoUnitarioDestino { get; set; }

        public decimal CostoCapacidadInstalada { get; set; }
        public decimal CostoCalidadPreventiva { get; set; }
        public decimal FondoRiesgo { get; set; }

        public bool AplicaNoRequerido { get; set; }
    }
}
