using System;

namespace CapaEntidad.Reportes
{
    public class ReporteResumenFinanciero
    {
        public decimal TotalIngresos { get; set; }

        public decimal TotalGastos { get; set; }

        public decimal UtilidadEstimada { get; set; }

        public decimal TotalCuentasPorCobrar { get; set; }

        public decimal TotalCuentasPorPagar { get; set; }

        public decimal MargenUtilidad { get; set; }

        public decimal CapitalNetoPendiente { get; set; }

        public string EstadoFinanciero { get; set; }

        public string TotalIngresosTexto
        {
            get
            {
                return TotalIngresos.ToString("N2");
            }
        }

        public string TotalGastosTexto
        {
            get
            {
                return TotalGastos.ToString("N2");
            }
        }

        public string UtilidadEstimadaTexto
        {
            get
            {
                return UtilidadEstimada.ToString("N2");
            }
        }

        public string TotalCuentasPorCobrarTexto
        {
            get
            {
                return TotalCuentasPorCobrar.ToString("N2");
            }
        }

        public string TotalCuentasPorPagarTexto
        {
            get
            {
                return TotalCuentasPorPagar.ToString("N2");
            }
        }

        public string MargenUtilidadTexto
        {
            get
            {
                return MargenUtilidad.ToString("N2") + "%";
            }
        }

        public string CapitalNetoPendienteTexto
        {
            get
            {
                return CapitalNetoPendiente.ToString("N2");
            }
        }

        public bool EsRentable
        {
            get
            {
                return EstadoFinanciero != null &&
                       EstadoFinanciero.Trim().ToLower() == "rentable";
            }
        }

        public bool EstaEnPerdida
        {
            get
            {
                return EstadoFinanciero != null &&
                       EstadoFinanciero.Trim().ToLower() == "pérdida";
            }
        }

        public bool EstaEquilibrado
        {
            get
            {
                return EstadoFinanciero != null &&
                       EstadoFinanciero.Trim().ToLower() == "equilibrado";
            }
        }

        public bool TieneCapitalPendientePositivo
        {
            get
            {
                return CapitalNetoPendiente > 0;
            }
        }

        public bool TieneMasPorPagarQuePorCobrar
        {
            get
            {
                return CapitalNetoPendiente < 0;
            }
        }

        public string RecomendacionGeneral
        {
            get
            {
                if (EstaEnPerdida)
                {
                    return "Revisar gastos, precios de venta y productos con bajo margen.";
                }

                if (TieneMasPorPagarQuePorCobrar)
                {
                    return "Priorizar recuperación de cuentas por cobrar y controlar pagos próximos.";
                }

                if (MargenUtilidad < 10 && TotalIngresos > 0)
                {
                    return "El margen es bajo; conviene revisar costos, descuentos y gastos operativos.";
                }

                if (EsRentable)
                {
                    return "El negocio muestra utilidad positiva en el periodo.";
                }

                return "Mantener seguimiento del flujo de ingresos, gastos y cuentas pendientes.";
            }
        }
    }
}