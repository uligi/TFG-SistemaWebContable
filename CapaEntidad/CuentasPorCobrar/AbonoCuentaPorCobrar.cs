using System;

namespace CapaEntidad.Cuentas
{
    public class AbonoCuentaPorCobrar
    {
        public string IdentificacionCliente { get; set; }

        public string NumeroFactura { get; set; }

        public int NumeroAbono { get; set; }

        public DateTime FechaAbono { get; set; }

        public decimal MontoAbono { get; set; }

        public string Observacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        // Campos auxiliares del JOIN con CuentaPorCobrar / Persona
        public string ClienteNombre { get; set; }

        public DateTime FechaEmision { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public decimal MontoOriginal { get; set; }

        public decimal SaldoActual { get; set; }

        public string EstadoCuenta { get; set; }

        public string ClienteDescripcion
        {
            get
            {
                return (IdentificacionCliente + " - " + ClienteNombre).Trim();
            }
        }

        public string CuentaDescripcion
        {
            get
            {
                return NumeroFactura + " - " + ClienteDescripcion;
            }
        }

        public string AbonoDescripcion
        {
            get
            {
                return NumeroFactura + " / Abono #" + NumeroAbono.ToString();
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

        public bool CuentaPendiente
        {
            get
            {
                return EstadoCuenta != null &&
                       EstadoCuenta.Trim().ToLower() == "pendiente";
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

        public string MontoAbonoTexto
        {
            get
            {
                return MontoAbono.ToString("N2");
            }
        }

        public string SaldoActualTexto
        {
            get
            {
                return SaldoActual.ToString("N2");
            }
        }

        public string MontoOriginalTexto
        {
            get
            {
                return MontoOriginal.ToString("N2");
            }
        }
    }
}