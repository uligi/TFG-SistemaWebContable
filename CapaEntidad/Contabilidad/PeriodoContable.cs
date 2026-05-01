using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class PeriodoContable
    {
        public int IdPeriodoContable { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public bool Activo { get; set; }
    }
}
