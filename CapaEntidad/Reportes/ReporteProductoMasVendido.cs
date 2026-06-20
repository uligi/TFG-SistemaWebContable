using System;

namespace CapaEntidad.Reportes
{
    public class ReporteProductoMasVendido
    {
        public string CodigoProducto { get; set; }

        public string NombreProducto { get; set; }

        public string TipoProducto { get; set; }

        public decimal CantidadVendida { get; set; }

        public decimal TotalVendido { get; set; }

        public int VecesFacturado { get; set; }

        public decimal StockActual { get; set; }

        public string Recomendacion { get; set; }

        public string ProductoDescripcion
        {
            get
            {
                return (CodigoProducto + " - " + NombreProducto).Trim();
            }
        }

        public bool EsProductoFuerte
        {
            get
            {
                return TotalVendido > 0 && CantidadVendida > 0;
            }
        }

        public bool RequiereRevisarInventario
        {
            get
            {
                return Recomendacion != null &&
                       Recomendacion.Trim().ToLower() == "revisar inventario";
            }
        }

        public bool EstaSinStock
        {
            get
            {
                return Recomendacion != null &&
                       Recomendacion.Trim().ToLower() == "sin stock";
            }
        }

        public string AccionSugerida
        {
            get
            {
                if (EstaSinStock)
                {
                    return "Reabastecer producto cuanto antes para no perder ventas.";
                }

                if (RequiereRevisarInventario)
                {
                    return "Revisar inventario y considerar aumentar el pedido al proveedor.";
                }

                if (CantidadVendida > 0)
                {
                    return "Mantener disponibilidad y evaluar promociones relacionadas.";
                }

                return "Dar seguimiento al movimiento del producto.";
            }
        }

        public string CantidadVendidaTexto
        {
            get
            {
                return CantidadVendida.ToString("N2");
            }
        }

        public string TotalVendidoTexto
        {
            get
            {
                return TotalVendido.ToString("N2");
            }
        }

        public string StockActualTexto
        {
            get
            {
                return StockActual.ToString("N2");
            }
        }
    }
}