using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Personas
{
    public class Telefono
    {
        public string NumeroTelefono { get; set; }
        public string Identificacion { get; set; }
        public int IdTipoTelefono { get; set; }
        public bool EsPrincipal { get; set; }
        public bool Activo { get; set; }
    }
}
