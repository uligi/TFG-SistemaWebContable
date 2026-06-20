using System;

namespace CapaEntidad.Consultas
{
    public class ConsultaProducto
    {
        public string CodigoProducto { get; set; }

        public string NombreProducto { get; set; }

        public string TipoProducto { get; set; }

        public decimal PrecioCompra { get; set; }

        public decimal PrecioVenta { get; set; }

        public decimal StockActual { get; set; }

        public decimal StockMinimo { get; set; }

        public decimal PorcentajeImpuesto { get; set; }

        public string ImpuestoNombre { get; set; }

        public bool Activo { get; set; }

        public string EstadoInventario { get; set; }

        public string ProductoDescripcion
        {
            get
            {
                return (CodigoProducto + " - " + NombreProducto).Trim();
            }
        }

        public bool EstaSinStock
        {
            get
            {
                return EstadoInventario != null &&
                       EstadoInventario.Trim().ToLower() == "sin stock";
            }
        }

        public bool EstaBajoStock
        {
            get
            {
                return EstadoInventario != null &&
                       EstadoInventario.Trim().ToLower() == "bajo stock";
            }
        }

        public string Recomendacion
        {
            get
            {
                if (EstaSinStock)
                {
                    return "Reabastecer producto de inmediato.";
                }

                if (EstaBajoStock)
                {
                    return "Revisar inventario y considerar compra.";
                }

                return "Stock disponible.";
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

        public string StockMinimoTexto
        {
            get
            {
                return StockMinimo.ToString("N2");
            }
        }

        public string ImpuestoTexto
        {
            get
            {
                return ImpuestoNombre + " (" + PorcentajeImpuesto.ToString("N2") + "%)";
            }
        }
    }
}