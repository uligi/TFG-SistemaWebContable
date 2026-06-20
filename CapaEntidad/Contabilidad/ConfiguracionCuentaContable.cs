using System;

namespace CapaEntidad.Contabilidad
{
    public class ConfiguracionCuentaContable
    {
        public string CodigoOperacion { get; set; }

        public string NombreOperacion { get; set; }

        public string Descripcion { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public bool AceptaMovimientos { get; set; }

        public bool CuentaActiva { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public string CuentaDescripcion
        {
            get
            {
                return (CodigoCuenta + " - " + NombreCuenta).Trim();
            }
        }

        public string EstadoCuenta
        {
            get
            {
                if (!CuentaActiva)
                {
                    return "Cuenta inactiva";
                }

                if (!AceptaMovimientos)
                {
                    return "No acepta movimientos";
                }

                return "Disponible";
            }
        }

        public bool ConfiguracionValida
        {
            get
            {
                return Activo && CuentaActiva && AceptaMovimientos;
            }
        }
    }
}