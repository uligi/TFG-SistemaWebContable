using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Inventario
{
    public class Producto
    {
        public int IdProducto { get; set; }
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
        public DateTime? FechaModificacion { get; set; }

        public bool Activo { get; set; }
    }
}
