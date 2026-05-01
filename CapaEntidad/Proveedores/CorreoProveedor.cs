using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Proveedores
{
    public class CorreoProveedor
    {
        public string IdentificacionProveedor { get; set; }
        public string DireccionCorreo { get; set; }
        public int IdTipoCorreo { get; set; }
        public bool EsPrincipal { get; set; }
        public bool Activo { get; set; }
    }
}
