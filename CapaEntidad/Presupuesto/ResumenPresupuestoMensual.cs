using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Presupuesto
{
    public class ResumenPresupuestoMensual
    {
        public string TipoMovimiento { get; set; }

        public int IdCuentaContable { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public decimal MontoPresupuestado { get; set; }

        public decimal MontoReal { get; set; }

        public decimal Diferencia { get; set; }
    }
}