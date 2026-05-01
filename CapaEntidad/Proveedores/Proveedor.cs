using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Proveedores
{
    public class Proveedor
    {
        public string IdentificacionProveedor { get; set; }
        public string RazonSocial { get; set; }
        public string NombreContacto { get; set; }
        public int? IdDistrito { get; set; }
        public string DireccionExacta { get; set; }
        public int? DiasCredito { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
