using System;

namespace CapaEntidad.Movimientos
{
    public class AbonoPendienteIngreso
    {
        public string IdentificacionCliente { get; set; }

        public string NumeroFactura { get; set; }

        public int NumeroAbono { get; set; }

        public DateTime FechaAbono { get; set; }

        public decimal MontoAbono { get; set; }

        public string Observacion { get; set; }

        public string ClienteNombre { get; set; }

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

        public string AbonoDescripcion
        {
            get
            {
                return NumeroFactura + " / Abono #" + NumeroAbono.ToString() + " - " + ClienteDescripcion;
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