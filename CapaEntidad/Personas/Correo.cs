using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Personas
{
    public class Correo
    {
        public string Identificacion { get; set; }
        public string DireccionCorreo { get; set; }
        public int IdTipoCorreo { get; set; }
        public bool EsPrincipal { get; set; }
        public bool Activo { get; set; }
    }
}
