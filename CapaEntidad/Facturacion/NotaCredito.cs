using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Facturacion
{
    public class NotaCredito
    {
        public string NumeroNotaCredito { get; set; }
        public int IdFactura { get; set; }
        public DateTime FechaNotaCredito { get; set; }
        public string Motivo { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalImpuesto { get; set; }
        public decimal TotalNotaCredito { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
