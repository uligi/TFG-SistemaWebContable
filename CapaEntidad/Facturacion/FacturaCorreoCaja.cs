using System.Collections.Generic;

namespace CapaEntidad.Facturacion
{
    public class FacturaCorreoCaja
    {
        public string CorreoDestino { get; set; }
        public string NumeroFactura { get; set; }
        public string Cliente { get; set; }
        public string Fecha { get; set; }
        public string Cajero { get; set; }

        public string Subtotal { get; set; }
        public string Impuesto { get; set; }
        public string Descuento { get; set; }
        public string Total { get; set; }

        public List<FacturaCorreoCajaDetalle> Detalles { get; set; }

        public FacturaCorreoCaja()
        {
            Detalles = new List<FacturaCorreoCajaDetalle>();
        }
    }

    public class FacturaCorreoCajaDetalle
    {
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Cantidad { get; set; }
        public string Precio { get; set; }
        public string TotalLinea { get; set; }
    }
}