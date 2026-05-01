using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Proveedores
{
    public class TelefonoProveedor
    {
        public string IdentificacionProveedor { get; set; }
        public string NumeroTelefono { get; set; }
        public int IdTipoTelefono { get; set; }
        public bool EsPrincipal { get; set; }
        public bool Activo { get; set; }
    }
}
