using System;

namespace CapaEntidad.Inventario
{
    public class Producto
    {
        public string CodigoProducto { get; set; }

        public string NombreProducto { get; set; }

        public int IdTipoProducto { get; set; }
        public string TipoProductoNombre { get; set; }

        public int IdImpuesto { get; set; }
        public string ImpuestoNombre { get; set; }
        public decimal PorcentajeImpuesto { get; set; }

        public string Descripcion { get; set; }

        public decimal PrecioVenta { get; set; }

        public decimal StockActual { get; set; }

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

        public string PrecioVentaTexto
        {
            get
            {
                return PrecioVenta.ToString("N2");
            }
        }

        public string StockActualTexto
        {
            get
            {
                return StockActual.ToString("N2");
            }
        }

        public bool TieneStock
        {
            get
            {
                return StockActual > 0;
            }
        }

        public decimal CalcularImpuestoUnitario()
        {
            return Math.Round(PrecioVenta * (PorcentajeImpuesto / 100), 2);
        }

        public decimal CalcularPrecioConImpuesto()
        {
            return Math.Round(PrecioVenta + CalcularImpuestoUnitario(), 2);
        }
    }
}