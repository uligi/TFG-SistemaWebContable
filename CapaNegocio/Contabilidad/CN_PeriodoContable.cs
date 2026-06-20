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

        public List<PeriodoContable> ListarAbiertos()
        {
            return objCapaDato.ListarAbiertos();
        }

        public PeriodoContable Obtener(int anio, int mes)
        {
            if (anio < 2000 || anio > 2100)
            {
                return null;
            }

            if (mes < 1 || mes > 12)
            {
                return null;
            }

            return objCapaDato.Obtener(anio, mes);
        }

        public int Registrar(PeriodoContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(PeriodoContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, true);

            if (error != "")
            {
                Mensaje = error;
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Cerrar(int anio, int mes, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (anio < 2000 || anio > 2100)
            {
                Mensaje = "Debe seleccionar un año válido.";
                return false;
            }

            if (mes < 1 || mes > 12)
            {
                Mensaje = "Debe seleccionar un mes válido.";
                return false;
            }

            return objCapaDato.Cerrar(anio, mes, out Mensaje);
        }

        public bool Inactivar(int anio, int mes, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (anio < 2000 || anio > 2100)
            {
                Mensaje = "Debe seleccionar un año válido.";
                return false;
            }

            if (mes < 1 || mes > 12)
            {
                Mensaje = "Debe seleccionar un mes válido.";
                return false;
            }

            return objCapaDato.Inactivar(anio, mes, out Mensaje);
        }

        private void PrepararDatos(PeriodoContable obj)
        {
            obj.Estado = obj.Estado == null ? "" : obj.Estado.Trim();

            if (obj.Estado == "")
            {
                obj.Estado = "Abierto";
            }
        }

        private string Validar(PeriodoContable obj, bool esEdicion)
        {
            if (obj.Anio < 2000 || obj.Anio > 2100)
            {
                return "Debe ingresar un año válido.";
            }

            if (obj.Mes < 1 || obj.Mes > 12)
            {
                return "Debe ingresar un mes válido.";
            }

            if (obj.FechaInicio == DateTime.MinValue)
            {
                return "Debe ingresar la fecha de inicio.";
            }

            if (obj.FechaFin == DateTime.MinValue)
            {
                return "Debe ingresar la fecha final.";
            }

            if (obj.FechaInicio > obj.FechaFin)
            {
                return "La fecha de inicio no puede ser mayor que la fecha final.";
            }

            if (obj.Estado != "Abierto" && obj.Estado != "Cerrado")
            {
                return "El estado debe ser Abierto o Cerrado.";
            }

            if (!esEdicion)
            {
                if (obj.Activo == false)
                {
                    obj.Activo = true;
                }
            }

            return "";
        }
    }
}