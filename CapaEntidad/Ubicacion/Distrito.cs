using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Ubicacion
{
    public class Distrito
    {
        public int IdDistrito { get; set; }
        public int IdCanton { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }


        public string CantonNombre { get; set; }
        public int IdProvincia { get; set; }
        public string ProvinciaNombre { get; set; }
    }
}
