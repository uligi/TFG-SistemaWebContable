using System;

namespace CapaEntidad.Consultas
{
    public class ConsultaFactura
    {
        public string NumeroFactura { get; set; }

        public DateTime FechaFactura { get; set; }

        public string IdentificacionCliente { get; set; }

        public string ClienteNombre { get; set; }

        public string IdentificacionEmpleado { get; set; }

        public string EmpleadoNombre { get; set; }

        public int IdTipoFactura { get; set; }

        public string TipoFacturaNombre { get; set; }

        public int IdTipoPago { get; set; }

        public string TipoPagoNombre { get; set; }

        public decimal SubtotalFactura { get; set; }

        public decimal ImpuestoFactura { get; set; }

        public decimal DescuentoFactura { get; set; }

        public decimal TotalFactura { get; set; }

        public string Estado { get; set; }

        public bool Activo { get; set; }

        public string ClienteDescripcion
        {
            get
            {
                return (IdentificacionCliente + " - " + ClienteNombre).Trim();
            }
        }

        public string EmpleadoDescripcion
        {
            get
            {
                return (IdentificacionEmpleado + " - " + EmpleadoNombre).Trim();
            }
        }

        public string FechaFacturaTexto
        {
            get
            {
                return FechaFactura.ToString("yyyy-MM-dd");
            }
        }

        public string SubtotalTexto
        {
            get
            {
                return SubtotalFactura.ToString("N2");
            }
        }

        public string ImpuestoTexto
        {
            get
            {
                return ImpuestoFactura.ToString("N2");
            }
        }

        public string DescuentoTexto
        {
            get
            {
                return DescuentoFactura.ToString("N2");
            }
        }

        public string TotalTexto
        {
            get
            {
                return TotalFactura.ToString("N2");
            }
        }

        public bool EstaEmitida
        {
            get
            {
                return Estado != null &&
                       Estado.Trim().ToLower() == "emitida";
            }
        }

        public bool EstaAnulada
        {
            get
            {
                return Estado != null &&
                       Estado.Trim().ToLower() == "anulada";
            }
        }

        public bool EsCredito
        {
            get
            {
                return TipoFacturaNombre != null &&
                       TipoFacturaNombre.Trim().ToLower().Contains("crédito");
            }
        }
    }
}