using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class DetalleAsientoContable
    {
        public int IdDetalleAsientoContable { get; set; }
        public int IdAsientoContable { get; set; }
        public int IdCuentaContable { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public string DescripcionLinea { get; set; }
        public bool Activo { get; set; }
    }
}
