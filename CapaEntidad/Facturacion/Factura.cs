using System;

namespace CapaEntidad.Facturacion
{
    public class Factura
    {
        public string NumeroFactura { get; set; }

        public string IdentificacionCliente { get; set; }
        public string ClienteNombre { get; set; }

        public string IdentificacionEmpleado { get; set; }
        public string EmpleadoNombre { get; set; }

        public int IdTipoPago { get; set; }
        public string TipoPagoNombre { get; set; }

        public int IdTipoFactura { get; set; }
        public string TipoFacturaNombre { get; set; }

        public DateTime FechaFactura { get; set; }

        public decimal Subtotal { get; set; }

        public decimal TotalImpuesto { get; set; }

        public decimal TotalDescuento { get; set; }

        public decimal TotalFactura { get; set; }

        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public string FacturaDescripcion
        {
            get
            {
                return (NumeroFactura + " - " + ClienteNombre).Trim();
            }
        }

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

        public bool EstaAnulada
        {
            get
            {
                return Estado != null &&
                       Estado.Trim().ToLower() == "anulada";
            }
        }

        public bool EstaCancelada
        {
            get
            {
                return Estado != null &&
                       Estado.Trim().ToLower() == "cancelada";
            }
        }

        public bool PuedeEditar
        {
            get
            {
                return Activo && !EstaAnulada && !EstaCancelada;
            }
        }

        public string TotalFacturaTexto
        {
            get
            {
                return TotalFactura.ToString("N2");
            }
        }
    }
}