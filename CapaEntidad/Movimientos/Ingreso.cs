using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Movimientos
{
    public class Ingreso
    {
        public int IdIngreso { get; set; }
        public int? IdFactura { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Descripcion { get; set; }
        public string TipoIngreso { get; set; }
        public string OrigenIngreso { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
