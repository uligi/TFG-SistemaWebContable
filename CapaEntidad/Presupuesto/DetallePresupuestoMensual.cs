using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Presupuesto
{
    public class DetallePresupuestoMensual
    {
        public int IdDetallePresupuestoMensual { get; set; }
        public int IdPresupuestoMensual { get; set; }
        public int IdCuentaContable { get; set; }
        public string TipoMovimiento { get; set; }
        public decimal MontoPresupuestado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
