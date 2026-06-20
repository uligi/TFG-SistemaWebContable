using System;

namespace CapaEntidad.Proveedores
{
    public class Proveedor
    {
        public string IdentificacionProveedor { get; set; }

        public string RazonSocial { get; set; }
        public string NombreContacto { get; set; }

        public int CodigoDistrito { get; set; }
        public string DistritoNombre { get; set; }

        public int CodigoCanton { get; set; }
        public string CantonNombre { get; set; }

        public int CodigoProvincia { get; set; }
        public string ProvinciaNombre { get; set; }

        public string DireccionExacta { get; set; }

        public int DiasCredito { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public string UbicacionCompleta
        {
            get
            {
                return (DistritoNombre + ", " + CantonNombre + ", " + ProvinciaNombre).Trim();
            }
        }
    }
}