using System;

namespace CapaEntidad.Consultas
{
    public class ConsultaMovimientoContable
    {
        public string TipoMovimiento { get; set; }

        public string NumeroMovimiento { get; set; }

        public DateTime FechaMovimiento { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public string Descripcion { get; set; }

        public decimal Monto { get; set; }

        public string Origen { get; set; }

        public string Referencia { get; set; }

        public bool Activo { get; set; }

        public string CuentaDescripcion
        {
            get
            {
                return (CodigoCuenta + " - " + NombreCuenta).Trim();
            }
        }

        public bool EsIngreso
        {
            get
            {
                return TipoMovimiento != null &&
                       TipoMovimiento.Trim().ToLower() == "ingreso";
            }
        }

        public bool EsGasto
        {
            get
            {
                return TipoMovimiento != null &&
                       TipoMovimiento.Trim().ToLower() == "gasto";
            }
        }

        public string FechaMovimientoTexto
        {
            get
            {
                return FechaMovimiento.ToString("yyyy-MM-dd");
            }
        }

        public string MontoTexto
        {
            get
            {
                return Monto.ToString("N2");
            }
        }
    }
}