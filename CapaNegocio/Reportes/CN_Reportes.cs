using CapaDatos.Reportes;
using CapaEntidad.Reportes;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Reportes
{
    public class CN_Reportes
    {
        private CD_Reportes objCapaDato = new CD_Reportes();

        public ReporteResumenFinanciero ObtenerResumenFinanciero(DateTime fechaInicio, DateTime fechaFin)
        {
            if (!ValidarRangoFechas(fechaInicio, fechaFin))
            {
                return new ReporteResumenFinanciero();
            }

            return objCapaDato.ObtenerResumenFinanciero(fechaInicio, fechaFin);
        }

        public List<ReporteIngreso> ReporteIngresos(DateTime fechaInicio, DateTime fechaFin)
        {
            if (!ValidarRangoFechas(fechaInicio, fechaFin))
            {
                return new List<ReporteIngreso>();
            }

            return objCapaDato.ReporteIngresos(fechaInicio, fechaFin);
        }

        public List<ReporteGasto> ReporteGastos(DateTime fechaInicio, DateTime fechaFin)
        {
            if (!ValidarRangoFechas(fechaInicio, fechaFin))
            {
                return new List<ReporteGasto>();
            }

            return objCapaDato.ReporteGastos(fechaInicio, fechaFin);
        }

        public List<ReporteCuentaPorCobrar> ReporteCuentasPorCobrar()
        {
            return objCapaDato.ReporteCuentasPorCobrar();
        }

        public List<ReporteCuentaPorPagar> ReporteCuentasPorPagar()
        {
            return objCapaDato.ReporteCuentasPorPagar();
        }

        public List<ReporteProductoMasVendido> ReporteProductosMasVendidos(DateTime fechaInicio, DateTime fechaFin)
        {
            if (!ValidarRangoFechas(fechaInicio, fechaFin))
            {
                return new List<ReporteProductoMasVendido>();
            }

            return objCapaDato.ReporteProductosMasVendidos(fechaInicio, fechaFin);
        }

        public List<ReporteClienteMasVenta> ReporteClientesMasVentas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (!ValidarRangoFechas(fechaInicio, fechaFin))
            {
                return new List<ReporteClienteMasVenta>();
            }

            return objCapaDato.ReporteClientesMasVentas(fechaInicio, fechaFin);
        }

        public List<ReporteFlujoDiario> ReporteFlujoDiario(DateTime fechaInicio, DateTime fechaFin)
        {
            if (!ValidarRangoFechas(fechaInicio, fechaFin))
            {
                return new List<ReporteFlujoDiario>();
            }

            return objCapaDato.ReporteFlujoDiario(fechaInicio, fechaFin);
        }

        public bool ValidarFechas(DateTime fechaInicio, DateTime fechaFin, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (fechaInicio == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha de inicio.";
                return false;
            }

            if (fechaFin == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha final.";
                return false;
            }

            if (fechaInicio > fechaFin)
            {
                Mensaje = "La fecha de inicio no puede ser mayor que la fecha final.";
                return false;
            }

            if (fechaInicio.Year < 2000 || fechaInicio.Year > 2100)
            {
                Mensaje = "La fecha de inicio no es válida.";
                return false;
            }

            if (fechaFin.Year < 2000 || fechaFin.Year > 2100)
            {
                Mensaje = "La fecha final no es válida.";
                return false;
            }

            return true;
        }

        private bool ValidarRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            string mensaje = string.Empty;
            return ValidarFechas(fechaInicio, fechaFin, out mensaje);
        }
    }
}