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

        public int IdTipoCuentaContable { get; set; }
        public string TipoCuentaContableNombre { get; set; }

        public int IdNaturalezaCuentaContable { get; set; }
        public string NaturalezaCuentaContableNombre { get; set; }

        public bool AceptaMovimientos { get; set; }
        public bool Activo { get; set; }

        public int? IdCuentaPadre { get; set; }
        public string CodigoCuentaPadre { get; set; }
        public string NombreCuentaPadre { get; set; }
    }
}
