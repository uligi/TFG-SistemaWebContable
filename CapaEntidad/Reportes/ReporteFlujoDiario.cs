using System;

namespace CapaEntidad.Reportes
{
    public class ReporteFlujoDiario
    {
        public DateTime Fecha { get; set; }

        public decimal TotalIngresos { get; set; }

        public decimal TotalGastos { get; set; }

        public decimal ResultadoDiario { get; set; }

        public string EstadoDia { get; set; }

        public bool EsPositivo
        {
            get
            {
                return EstadoDia != null &&
                       EstadoDia.Trim().ToLower() == "positivo";
            }
        }

        public bool EsNegativo
        {
            get
            {
                return EstadoDia != null &&
                       EstadoDia.Trim().ToLower() == "negativo";
            }
        }

        public bool EsEquilibrado
        {
            get
            {
                return EstadoDia != null &&
                       EstadoDia.Trim().ToLower() == "equilibrado";
            }
        }

        public string Recomendacion
        {
            get
            {
                if (EsNegativo)
                {
                    return "Revisar gastos del día y buscar oportunidades para aumentar ventas.";
                }

                if (EsPositivo)
                {
                    return "Identificar qué impulsó el buen resultado y replicarlo.";
                }

                if (EsEquilibrado)
                {
                    return "Mantener control de gastos y buscar mejorar ingresos.";
                }

                return "Dar seguimiento al flujo diario.";
            }
        }

        public string FechaTexto
        {
            get
            {
                return Fecha.ToString("yyyy-MM-dd");
            }
        }

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

        public string ResultadoDiarioTexto
        {
            get
            {
                return ResultadoDiario.ToString("N2");
            }
        }
    }
}