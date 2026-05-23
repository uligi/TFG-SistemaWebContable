using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaEntidad.Presupuesto
{
    public class PresupuestoMensual
    {
        public int IdPresupuestoMensual { get; set; }

        public int Anio { get; set; }

        public int Mes { get; set; }

        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public bool Activo { get; set; }
    }
}
