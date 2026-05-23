using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.CuentasPorCobrar
{
    public class CuentaPorCobrar
    {
        public int IdCuentaPorCobrar { get; set; }

        public string IdentificacionCliente { get; set; }
        public string ClienteNombre { get; set; }

        public int IdFactura { get; set; }
        public string NumeroFactura { get; set; }

        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }

        public decimal MontoOriginal { get; set; }
        public decimal SaldoActual { get; set; }

        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public bool Activo { get; set; }
    }
}
