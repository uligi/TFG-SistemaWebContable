using System;

namespace CapaEntidad.Contabilidad
{
    public class DetalleAsientoContable
    {
        public string NumeroAsiento { get; set; }

        public int NumeroLinea { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public int IdTipoCuentaContable { get; set; }

        public string TipoCuentaContableNombre { get; set; }

        public int IdNaturalezaCuentaContable { get; set; }

        public string NaturalezaCuentaContableNombre { get; set; }

        public decimal Debe { get; set; }

        public decimal Haber { get; set; }

        public string DescripcionLinea { get; set; }

        public bool Activo { get; set; }

        public string CuentaDescripcion
        {
            get
            {
                return (CodigoCuenta + " - " + NombreCuenta).Trim();
            }
        }

        public string TipoMovimiento
        {
            get
            {
                if (Debe > 0 && Haber == 0)
                {
                    return "Debe";
                }

                if (Haber > 0 && Debe == 0)
                {
                    return "Haber";
                }

                return "Sin definir";
            }
        }

        public decimal Monto
        {
            get
            {
                if (Debe > 0)
                {
                    return Debe;
                }

                if (Haber > 0)
                {
                    return Haber;
                }

                return 0;
            }
        }

        public bool EsLineaValida
        {
            get
            {
                return (Debe > 0 && Haber == 0) || (Haber > 0 && Debe == 0);
            }
        }

        public string LineaDescripcion
        {
            get
            {
                return NumeroLinea.ToString() + " - " + CuentaDescripcion;
            }
        }
    }
}