using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class AsientoContable
    {
        public int IdAsientoContable { get; set; }
        public int IdPeriodoContable { get; set; }
        public string NumeroAsiento { get; set; }
        public DateTime FechaAsiento { get; set; }
        public string TipoAsiento { get; set; }
        public string Concepto { get; set; }
        public decimal TotalDebe { get; set; }
        public decimal TotalHaber { get; set; }
        public bool Activo { get; set; }
    }
}
