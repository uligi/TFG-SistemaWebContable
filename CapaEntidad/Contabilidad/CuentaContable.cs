using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class CuentaContable
    {
        public int IdCuentaContable { get; set; }
        public string CodigoCuenta { get; set; }
        public string NombreCuenta { get; set; }
        public string TipoCuenta { get; set; }
        public string Naturaleza { get; set; }
        public bool AceptaMovimientos { get; set; }
        public bool Activo { get; set; }
        public int? IdCuentaPadre { get; set; }
    }
}
