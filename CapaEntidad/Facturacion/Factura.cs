using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Facturacion
{
    public class Factura
    {
        public int IdFactura { get; set; }
        public string IdentificacionCliente { get; set; }
        public string IdentificacionEmpleado { get; set; }
        public string NumeroFactura { get; set; }
        public int IdTipoPago { get; set; }
        public int IdTipoFactura { get; set; }
        public DateTime FechaFactura { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalImpuesto { get; set; }
        public decimal TotalDescuento { get; set; }
        public decimal TotalFactura { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
