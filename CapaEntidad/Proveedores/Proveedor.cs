using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CapaEntidad.Proveedores
{
    public class Proveedor
    {
        public string IdentificacionProveedor { get; set; }
        public string RazonSocial { get; set; }
        public string NombreContacto { get; set; }

        public int? IdDistrito { get; set; }
        public string DistritoNombre { get; set; }

        public int? IdCanton { get; set; }
        public string CantonNombre { get; set; }

        public int? IdProvincia { get; set; }
        public string ProvinciaNombre { get; set; }

        public string DireccionExacta { get; set; }
        public int? DiasCredito { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public bool Activo { get; set; }
    }
}
