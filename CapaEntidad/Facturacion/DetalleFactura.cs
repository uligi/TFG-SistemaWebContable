using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Facturacion
{
    public class DetalleFactura
    {
        public int IdDetalleFactura { get; set; }
        public int IdFactura { get; set; }
        public int IdProducto { get; set; }
        public string DescripcionItem { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal SubtotalLinea { get; set; }
        public decimal TotalLinea { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
