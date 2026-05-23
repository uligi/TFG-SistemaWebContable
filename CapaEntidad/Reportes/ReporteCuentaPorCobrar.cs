using System;

namespace CapaEntidad.Reportes
{
    public class ReporteCuentaPorCobrar
    {
        public int IdCuentaPorCobrar { get; set; }

        public string NumeroFactura { get; set; }

        public string IdentificacionCliente { get; set; }

        public string ClienteNombre { get; set; }

        public DateTime FechaEmision { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public decimal MontoOriginal { get; set; }

        public decimal SaldoActual { get; set; }

        public string Estado { get; set; }

        public bool Activo { get; set; }
    }
}