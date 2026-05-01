using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Seguridad
{
    public class IntentoAcceso
    {
        public int IdIntentoAcceso { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaHoraIntento { get; set; }
        public bool Exitoso { get; set; }
        public string IPEquipo { get; set; }
        public string Observacion { get; set; }
        public string Identificacion { get; set; }
    }
}
