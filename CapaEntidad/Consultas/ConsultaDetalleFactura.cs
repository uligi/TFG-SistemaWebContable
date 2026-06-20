using System;

namespace CapaEntidad.Consultas
{
    public class ConsultaDetalleFactura
    {
        public string NumeroFactura { get; set; }

        public string CodigoProducto { get; set; }

        public string NombreProducto { get; set; }

        public string DescripcionItem { get; set; }

        public decimal Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal PorcentajeImpuesto { get; set; }

        public decimal PorcentajeDescuento { get; set; }

        public decimal SubtotalLinea { get; set; }

        public decimal TotalLinea { get; set; }

        public bool Activo { get; set; }

        public string ProductoDescripcion
        {
            get
            {
                return (CodigoProducto + " - " + NombreProducto).Trim();
            }
        }

        public string CantidadTexto
        {
            get
            {
                return Cantidad.ToString("N2");
            }
        }

        public string PrecioUnitarioTexto
        {
            get
            {
                return PrecioUnitario.ToString("N2");
            }
        }

        public string ImpuestoTexto
        {
            get
            {
                return PorcentajeImpuesto.ToString("N2") + "%";
            }
        }

        public string DescuentoTexto
        {
            get
            {
                return PorcentajeDescuento.ToString("N2") + "%";
            }
        }

        public string SubtotalTexto
        {
            get
            {
                return SubtotalLinea.ToString("N2");
            }
        }

        public string TotalTexto
        {
            get
            {
                return TotalLinea.ToString("N2");
            }
        }
    }
}