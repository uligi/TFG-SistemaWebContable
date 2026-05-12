using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Catalogos
{
    public class NaturalezaCuentaContable
    {
        public int IdNaturalezaCuentaContable { get; set; }
        public string Naturaleza { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
