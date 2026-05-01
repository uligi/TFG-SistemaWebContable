using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.CuentasPorPagar
{
    public class AbonoCuentaPorPagar
    {
        public int IdAbonoCuentaPorPagar { get; set; }
        public int IdCuentaPorPagar { get; set; }
        public DateTime FechaAbono { get; set; }
        public decimal MontoAbono { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
