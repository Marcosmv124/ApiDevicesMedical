namespace AppDevicesMedical.DTOs
{
    public class TransferenciaReporteDto
    {
        // --- CABECERA (Header del reporte) ---
        public int IdTransferencia { get; set; }
        public string Folio { get; set; }         // Ej: "TR-HIST-001"
        public string SolicitadoPor { get; set; } // Ej: "Admin Sistema"
        public string FechaCreacion { get; set; } // Ej: "2025-11-01"

        // --- CONFIGURACIÓN TÉCNICA (Cajas Roja y Verde) ---
        public string SitioOrigen { get; set; }          // Ej: "Planta Kyoto (Japón)"
        public string SistemaCalidadOrigen { get; set; } // Ej: "JP"

        public string SitioDestino { get; set; }         // Ej: "Planta Tijuana (México)"
        public string SistemaCalidadDestino { get; set; } // Ej: "MX"

        public string Comercializacion { get; set; }     // Ej: "US"

        // --- BUSINESS CASE (Sección Inferior) ---
        public int Volumen { get; set; }             // Campo "Volumen"
        public int RampUpMeses { get; set; }         // Campo "Ramp Up"

        public decimal CapacidadInversion { get; set; } // Campo "Capacidad" ($20000)
        public decimal AhorroProyectado { get; set; }   // Campo "Ahorro" ($150,000)

        // --- BARRA DE ESTATUS (Verde/Roja) ---
        public bool EsViable { get; set; }              // Para poner la barra verde o roja
        public string MensajeViabilidad { get; set; }   // Texto: "PROYECTO VIABLE"
    }
}
