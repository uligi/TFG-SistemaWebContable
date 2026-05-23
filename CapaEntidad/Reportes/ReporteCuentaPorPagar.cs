using System;

namespace CapaEntidad.Reportes
{
    public class ReporteCuentaPorPagar
    {
        public int IdCuentaPorPagar { get; set; }

        public string NumeroDocumento { get; set; }

        public string IdentificacionProveedor { get; set; }

        public string ProveedorNombre { get; set; }

        public DateTime FechaEmision { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public string Concepto { get; set; }

        public decimal MontoOriginal { get; set; }

        public decimal SaldoActual { get; set; }

        public string Estado { get; set; }

        public bool Activo { get; set; }
    }
}