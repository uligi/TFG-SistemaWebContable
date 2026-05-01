using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.CuentasPorPagar
{
    public class CuentaPorPagar
    {
        public int IdCuentaPorPagar { get; set; }
        public string IdentificacionProveedor { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Concepto { get; set; }
        public decimal MontoOriginal { get; set; }
        public decimal SaldoActual { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
