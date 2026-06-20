using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Ubicacion
{
    public class Canton
    {
        public int CodigoCanton { get; set; }
        public int CodigoProvincia { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        public string ProvinciaNombre { get; set; }
    }
}