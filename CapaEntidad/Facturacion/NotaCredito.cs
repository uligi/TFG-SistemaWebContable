using System;

namespace CapaEntidad.Facturacion
{
    public class NotaCredito
    {
        public string NumeroNotaCredito { get; set; }

        public string NumeroFactura { get; set; }

        public string IdentificacionCliente { get; set; }

        public string ClienteNombre { get; set; }

        public string IdentificacionEmpleado { get; set; }

        public string EmpleadoNombre { get; set; }

        public DateTime FechaNotaCredito { get; set; }

        public string Motivo { get; set; }

        public decimal Subtotal { get; set; }

        public decimal TotalImpuesto { get; set; }

        public decimal TotalNotaCredito { get; set; }

        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        // Campos auxiliares del JOIN con Factura
        public decimal TotalFactura { get; set; }

        public string EstadoFactura { get; set; }

        public bool FacturaActiva { get; set; }

        public string NotaCreditoDescripcion
        {
            get
            {
                return (NumeroNotaCredito + " - Factura " + NumeroFactura).Trim();
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

        public bool EstaAplicada
        {
            get
            {
                return Estado != null &&
                       Estado.Trim().ToLower() == "aplicada";
            }
        }

        public bool PuedeEditar
        {
            get
            {
                return Activo && !EstaAnulada;
            }
        }

        public string SubtotalTexto
        {
            get
            {
                return Subtotal.ToString("N2");
            }
        }

        public string TotalImpuestoTexto
        {
            get
            {
                return TotalImpuesto.ToString("N2");
            }
        }

        public string TotalNotaCreditoTexto
        {
            get
            {
                return TotalNotaCredito.ToString("N2");
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