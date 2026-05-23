using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Contabilidad
{
    public class CN_PeriodoContable
    {
        private CD_PeriodoContable objCapaDato = new CD_PeriodoContable();

        public List<PeriodoContable> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<PeriodoContable> ListarActivos()
        {
            return objCapaDato.ListarActivos();
        }

        public List<PeriodoContable> ListarAbierto()
        {
            return objCapaDato.ListarAbierto();
        }

        public int Registrar(PeriodoContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Anio < 2000 || obj.Anio > 2100)
            {
                Mensaje = "Debe ingresar un año válido.";
                return 0;
            }

            if (obj.Mes < 1 || obj.Mes > 12)
            {
                Mensaje = "Debe ingresar un mes válido.";
                return 0;
            }

            if (obj.FechaInicio == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha de inicio.";
                return 0;
            }

            if (obj.FechaFin == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha final.";
                return 0;
            }

            if (obj.FechaInicio > obj.FechaFin)
            {
                Mensaje = "La fecha de inicio no puede ser mayor que la fecha final.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.Estado))
            {
                Mensaje = "Debe seleccionar el estado del período.";
                return 0;
            }

            obj.Estado = obj.Estado.Trim();

            if (obj.Estado != "Abierto" && obj.Estado != "Cerrado")
            {
                Mensaje = "El estado debe ser Abierto o Cerrado.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(PeriodoContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdPeriodoContable == 0)
            {
                Mensaje = "Debe seleccionar un período contable válido.";
                return false;
            }

            if (obj.Anio < 2000 || obj.Anio > 2100)
            {
                Mensaje = "Debe ingresar un año válido.";
                return false;
            }

            if (obj.Mes < 1 || obj.Mes > 12)
            {
                Mensaje = "Debe ingresar un mes válido.";
                return false;
            }

            if (obj.FechaInicio == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha de inicio.";
                return false;
            }

            if (obj.FechaFin == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha final.";
                return false;
            }

            if (obj.FechaInicio > obj.FechaFin)
            {
                Mensaje = "La fecha de inicio no puede ser mayor que la fecha final.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Estado))
            {
                Mensaje = "Debe seleccionar el estado del período.";
                return false;
            }

            obj.Estado = obj.Estado.Trim();

            if (obj.Estado != "Abierto" && obj.Estado != "Cerrado")
            {
                Mensaje = "El estado debe ser Abierto o Cerrado.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Cerrar(int idPeriodoContable, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idPeriodoContable == 0)
            {
                Mensaje = "Debe seleccionar un período contable válido.";
                return false;
            }

            return objCapaDato.Cerrar(idPeriodoContable, out Mensaje);
        }

        public bool Inactivar(int idPeriodoContable, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idPeriodoContable == 0)
            {
                Mensaje = "Debe seleccionar un período contable válido.";
                return false;
            }

            return objCapaDato.Inactivar(idPeriodoContable, out Mensaje);
        }
    }
}