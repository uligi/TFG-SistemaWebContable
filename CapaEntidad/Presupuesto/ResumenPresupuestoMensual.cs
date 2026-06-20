using System;

namespace CapaEntidad.Presupuesto
{
    public class ResumenPresupuestoMensual
    {
        public string TipoMovimiento { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public decimal MontoPresupuestado { get; set; }

        public decimal MontoReal { get; set; }

        public decimal Diferencia { get; set; }

        public decimal PorcentajeEjecucion { get; set; }

        public string EstadoEjecucion { get; set; }

        // Campos para resumen general
        public decimal TotalIngresoPresupuestado { get; set; }

        public decimal TotalIngresoReal { get; set; }

        public decimal TotalGastoPresupuestado { get; set; }

        public decimal TotalGastoReal { get; set; }

        public decimal UtilidadPresupuestada { get; set; }

        public decimal UtilidadReal { get; set; }

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

        public bool EstaExcedido
        {
            get
            {
                return EstadoEjecucion != null &&
                       EstadoEjecucion.Trim().ToLower() == "excedido";
            }
        }

        public bool EstaCumplido
        {
            get
            {
                return EstadoEjecucion != null &&
                       EstadoEjecucion.Trim().ToLower() == "cumplido";
            }
        }

        public bool EstaDentroPresupuesto
        {
            get
            {
                return EstadoEjecucion != null &&
                       EstadoEjecucion.Trim().ToLower() == "dentro del presupuesto";
            }
        }

        public bool EstaPorDebajo
        {
            get
            {
                return EstadoEjecucion != null &&
                       EstadoEjecucion.Trim().ToLower() == "por debajo";
            }
        }

        public string MontoPresupuestadoTexto
        {
            get
            {
                return MontoPresupuestado.ToString("N2");
            }
        }

        public string MontoRealTexto
        {
            get
            {
                return MontoReal.ToString("N2");
            }
        }

        public string DiferenciaTexto
        {
            get
            {
                return Diferencia.ToString("N2");
            }
        }

        public string PorcentajeEjecucionTexto
        {
            get
            {
                return PorcentajeEjecucion.ToString("N2") + "%";
            }
        }

        public string TotalIngresoPresupuestadoTexto
        {
            get
            {
                return TotalIngresoPresupuestado.ToString("N2");
            }
        }

        public string TotalIngresoRealTexto
        {
            get
            {
                return TotalIngresoReal.ToString("N2");
            }
        }

        public string TotalGastoPresupuestadoTexto
        {
            get
            {
                return TotalGastoPresupuestado.ToString("N2");
            }
        }

        public string TotalGastoRealTexto
        {
            get
            {
                return TotalGastoReal.ToString("N2");
            }
        }

        public string UtilidadPresupuestadaTexto
        {
            get
            {
                return UtilidadPresupuestada.ToString("N2");
            }
        }

        public string UtilidadRealTexto
        {
            get
            {
                return UtilidadReal.ToString("N2");
            }
        }
    }
}