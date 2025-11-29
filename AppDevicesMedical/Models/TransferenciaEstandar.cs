namespace AppDevicesMedical.Models
{
    public class TransferenciaEstandar
    {
        public int IdTransferencia { get; set; }
        public virtual Transferencia Transferencia { get; set; }

        public int IdEstandar { get; set; }
        public virtual Estandar Estandar { get; set; }
    }
}
