using CapaDatos.Reportes;
using CapaEntidad.Reportes;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Reportes
{
    public class CN_Reportes
    {
        private CD_Reportes objCapaDato = new CD_Reportes();

        public ReporteResumenFinanciero ResumenFinanciero(DateTime fechaInicio, DateTime fechaFin, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (fechaInicio == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha de inicio.";
                return new ReporteResumenFinanciero();
            }

            if (fechaFin == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha final.";
                return new ReporteResumenFinanciero();
            }

            if (fechaFin < fechaInicio)
            {
                Mensaje = "La fecha final no puede ser menor que la fecha de inicio.";
                return new ReporteResumenFinanciero();
            }

            return objCapaDato.ResumenFinanciero(fechaInicio, fechaFin);
        }

        public List<ReporteIngreso> ReporteIngresos(DateTime fechaInicio, DateTime fechaFin, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (fechaInicio == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha de inicio.";
                return new List<ReporteIngreso>();
            }

            if (fechaFin == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha final.";
                return new List<ReporteIngreso>();
            }

            if (fechaFin < fechaInicio)
            {
                Mensaje = "La fecha final no puede ser menor que la fecha de inicio.";
                return new List<ReporteIngreso>();
            }

            return objCapaDato.ReporteIngresos(fechaInicio, fechaFin);
        }

        public List<ReporteGasto> ReporteGastos(DateTime fechaInicio, DateTime fechaFin, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (fechaInicio == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha de inicio.";
                return new List<ReporteGasto>();
            }

            if (fechaFin == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha final.";
                return new List<ReporteGasto>();
            }

            if (fechaFin < fechaInicio)
            {
                Mensaje = "La fecha final no puede ser menor que la fecha de inicio.";
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
    }
}