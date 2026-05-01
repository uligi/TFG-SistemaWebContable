using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Personas
{
    public class Empleado
    {
        public string Identificacion { get; set; }
        public string CodigoEmpleado { get; set; }
        public string Puesto { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public int IdRol { get; set; }
        public string NombreUsuario { get; set; }
        public string ClaveHash { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool RestablecerClave { get; set; }
        public bool Activo { get; set; }
    }
}
