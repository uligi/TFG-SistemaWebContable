using System;

namespace CapaEntidad.CuentasPorPagar
{
    public class AbonoCuentaPorPagar
    {
        public string IdentificacionProveedor { get; set; }

        public string ProveedorNombre { get; set; }

        public string NumeroDocumento { get; set; }

        public int NumeroAbono { get; set; }

        public DateTime FechaAbono { get; set; }

        public decimal MontoAbono { get; set; }

        public string Observacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        // Campos auxiliares del JOIN con CuentaPorPagar
        public DateTime FechaEmision { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public string Concepto { get; set; }

        public decimal MontoOriginal { get; set; }

        public decimal SaldoActual { get; set; }

        public string EstadoCuenta { get; set; }

        public string ProveedorDescripcion
        {
            get
            {
                return (IdentificacionProveedor + " - " + ProveedorNombre).Trim();
            }
        }

        public string CuentaDescripcion
        {
            get
            {
                return (NumeroDocumento + " - " + ProveedorDescripcion).Trim();
            }
        }

        public string AbonoDescripcion
        {
            get
            {
                return NumeroDocumento + " / Abono #" + NumeroAbono.ToString();
            }
        }

        public bool CuentaPagada
        {
            get
            {
                return EstadoCuenta != null &&
                       EstadoCuenta.Trim().ToLower() == "pagada";
            }
        }

        public bool CuentaParcial
        {
            get
            {
                return EstadoCuenta != null &&
                       EstadoCuenta.Trim().ToLower() == "parcial";
            }
        }

        public bool CuentaPendiente
        {
            get
            {
                return EstadoCuenta != null &&
                       EstadoCuenta.Trim().ToLower() == "pendiente";
            }
        }

        public string MontoAbonoTexto
        {
            get
            {
                return MontoAbono.ToString("N2");
            }
        }

        public string MontoOriginalTexto
        {
            get
            {
                return MontoOriginal.ToString("N2");
            }
        }

        public string SaldoActualTexto
        {
            get
            {
                return SaldoActual.ToString("N2");
            }
        }
    }
}