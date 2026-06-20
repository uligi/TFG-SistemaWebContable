using System;

namespace CapaEntidad.Movimientos
{
    public class Ingreso
    {
        public string NumeroIngreso { get; set; }

        public DateTime FechaIngreso { get; set; }

        public string Descripcion { get; set; }

        public int IdTipoIngreso { get; set; }

        public string TipoIngresoNombre { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public string IdentificacionCliente { get; set; }

        public string NumeroFactura { get; set; }

        public string ClienteNombre { get; set; }

        public string IdentificacionClienteAbono { get; set; }

        public string NumeroFacturaAbono { get; set; }

        public int NumeroAbonoCuentaPorCobrar { get; set; }

        public DateTime FechaAbono { get; set; }

        public decimal MontoAbono { get; set; }

        public string OrigenIngreso { get; set; }

        public decimal Monto { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public string CuentaDescripcion
        {
            get
            {
                return (CodigoCuenta + " - " + NombreCuenta).Trim();
            }
        }

        public string ClienteDescripcion
        {
            get
            {
                return (IdentificacionCliente + " - " + ClienteNombre).Trim();
            }
        }

        public string FacturaDescripcion
        {
            get
            {
                return (NumeroFactura + " - " + ClienteDescripcion).Trim();
            }
        }

        public string AbonoDescripcion
        {
            get
            {
                return NumeroFacturaAbono + " / Abono #" + NumeroAbonoCuentaPorCobrar.ToString();
            }
        }

        public bool EsIngresoPorFactura
        {
            get
            {
                return OrigenIngreso != null &&
                       OrigenIngreso.Trim().ToLower() == "factura";
            }
        }

        public bool EsIngresoPorAbonoCxC
        {
            get
            {
                return OrigenIngreso != null &&
                       OrigenIngreso.Trim().ToLower() == "abono cxc";
            }
        }

        public bool EsIngresoManual
        {
            get
            {
                return OrigenIngreso != null &&
                       OrigenIngreso.Trim().ToLower() == "manual";
            }
        }

        public string MontoTexto
        {
            get
            {
                return Monto.ToString("N2");
            }
        }

        public string MontoAbonoTexto
        {
            get
            {
                return MontoAbono.ToString("N2");
            }
        }
    }
}