using System;

namespace CapaEntidad.Contabilidad
{
    public class ConfiguracionCuentaContable
    {
        public int IdConfiguracionCuentaContable { get; set; }

        public string CodigoOperacion { get; set; }

        public string NombreOperacion { get; set; }

        public string Descripcion { get; set; }

        public int IdCuentaContable { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public bool Activo { get; set; }
    }
}