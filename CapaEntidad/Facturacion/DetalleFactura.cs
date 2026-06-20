using System;

namespace CapaEntidad.Facturacion
{
    public class DetalleFactura
    {
        public string NumeroFactura { get; set; }

        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }

        public int IdImpuesto { get; set; }
        public string ImpuestoNombre { get; set; }

        public int IdDescuento { get; set; }
        public string DescuentoNombre { get; set; }

        public string DescripcionItem { get; set; }

        public decimal Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal PorcentajeImpuesto { get; set; }

        public decimal PorcentajeDescuento { get; set; }

        public decimal SubtotalLinea { get; set; }

        public decimal TotalLinea { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public string ProductoDescripcion
        {
            get
            {
                return (CodigoProducto + " - " + NombreProducto).Trim();
            }
        }

        public string DetalleDescripcion
        {
            get
            {
                return NumeroFactura + " - " + ProductoDescripcion;
            }
        }

        public decimal MontoDescuento
        {
            get
            {
                return Math.Round(SubtotalLinea * (PorcentajeDescuento / 100), 2);
            }
        }

        public decimal BaseImponible
        {
            get
            {
                return Math.Round(SubtotalLinea - MontoDescuento, 2);
            }
        }

        public decimal MontoImpuesto
        {
            get
            {
                return Math.Round(BaseImponible * (PorcentajeImpuesto / 100), 2);
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

        public string SubtotalLineaTexto
        {
            get
            {
                return SubtotalLinea.ToString("N2");
            }
        }

        public string TotalLineaTexto
        {
            get
            {
                return TotalLinea.ToString("N2");
            }
        }

        public bool TieneDescuento
        {
            get
            {
                return PorcentajeDescuento > 0;
            }
        }

        public bool TieneImpuesto
        {
            get
            {
                return PorcentajeImpuesto > 0;
            }
        }
    }
}