using System;

namespace CapaEntidad.Presupuesto
{
    public class DetallePresupuestoMensual
    {
        public int Anio { get; set; }

        public int Mes { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public string TipoMovimiento { get; set; }

        public decimal MontoPresupuestado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        // Campo auxiliar del SP
        public string MesNombre { get; set; }

        public string PeriodoDescripcion
        {
            get
            {
                return MesNombre + " " + Anio.ToString();
            }
        }

        public string PeriodoCodigo
        {
            get
            {
                return Anio.ToString() + "-" + Mes.ToString("00");
            }
        }

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

        public string TipoMovimientoDescripcion
        {
            get
            {
                if (EsIngreso)
                {
                    return "Ingreso";
                }

                if (EsGasto)
                {
                    return "Gasto";
                }

                return TipoMovimiento;
            }
        }

        public string MontoPresupuestadoTexto
        {
            get
            {
                return MontoPresupuestado.ToString("N2");
            }
        }
    }
}