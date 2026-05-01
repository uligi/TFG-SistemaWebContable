using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.CuentasPorCobrar
{
    public class AbonoCuentaPorCobrar
    {
        public int IdAbonoCuentaPorCobrar { get; set; }
        public int IdCuentaPorCobrar { get; set; }
        public DateTime FechaAbono { get; set; }
        public decimal MontoAbono { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
