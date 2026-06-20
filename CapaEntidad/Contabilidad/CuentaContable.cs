using System;

namespace CapaEntidad.Contabilidad
{
    public class CuentaContable
    {
        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public int IdTipoCuentaContable { get; set; }
        public string TipoCuentaContableNombre { get; set; }

        public int IdNaturalezaCuentaContable { get; set; }
        public string NaturalezaCuentaContableNombre { get; set; }

        public bool AceptaMovimientos { get; set; }

        public bool Activo { get; set; }

        public string CodigoCuentaPadre { get; set; }
        public string NombreCuentaPadre { get; set; }

        public string CodigoNombre
        {
            get
            {
                return (CodigoCuenta + " - " + NombreCuenta).Trim();
            }
        }

        public string CuentaPadreDescripcion
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CodigoCuentaPadre) || CodigoCuentaPadre == "0")
                {
                    return "Cuenta raíz";
                }

                return (CodigoCuentaPadre + " - " + NombreCuentaPadre).Trim();
            }
        }

        public bool EsCuentaRaiz
        {
            get
            {
                return CodigoCuenta == "0";
            }
        }

        public bool PuedeUsarseEnMovimientos
        {
            get
            {
                return Activo && AceptaMovimientos && CodigoCuenta != "0";
            }
        }
    }
}