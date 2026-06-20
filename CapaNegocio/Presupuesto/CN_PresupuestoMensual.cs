using CapaDatos.Presupuesto;
using CapaEntidad.Presupuesto;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Presupuesto
{
    public class CN_PresupuestoMensual
    {
        private CD_PresupuestoMensual objCapaDato = new CD_PresupuestoMensual();

        public List<PresupuestoMensual> Listar()
        {
            return objCapaDato.Listar();
        }

        public PresupuestoMensual Obtener(int anio, int mes)
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

        public int Registrar(PresupuestoMensual obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(PresupuestoMensual obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj);

            if (error != "")
            {
                Mensaje = error;
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int anio, int mes, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (anio < 2000 || anio > 2100)
            {
                Mensaje = "Debe ingresar un año válido.";
                return false;
            }

            if (mes < 1 || mes > 12)
            {
                Mensaje = "Debe ingresar un mes válido.";
                return false;
            }

            return objCapaDato.Inactivar(anio, mes, out Mensaje);
        }

        public bool Cerrar(int anio, int mes, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (anio < 2000 || anio > 2100)
            {
                Mensaje = "Debe ingresar un año válido.";
                return false;
            }

            if (mes < 1 || mes > 12)
            {
                Mensaje = "Debe ingresar un mes válido.";
                return false;
            }

            return objCapaDato.Cerrar(anio, mes, out Mensaje);
        }

        public bool Reabrir(int anio, int mes, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (anio < 2000 || anio > 2100)
            {
                Mensaje = "Debe ingresar un año válido.";
                return false;
            }

            if (mes < 1 || mes > 12)
            {
                Mensaje = "Debe ingresar un mes válido.";
                return false;
            }

            return objCapaDato.Reabrir(anio, mes, out Mensaje);
        }

        public List<ResumenPresupuestoMensual> ResumenVsReal(int anio, int mes)
        {
            if (anio < 2000 || anio > 2100)
            {
                return new List<ResumenPresupuestoMensual>();
            }

            if (mes < 1 || mes > 12)
            {
                return new List<ResumenPresupuestoMensual>();
            }

            return objCapaDato.ResumenVsReal(anio, mes);
        }

        public ResumenPresupuestoMensual ResumenGeneral(int anio, int mes)
        {
            if (anio < 2000 || anio > 2100)
            {
                return new ResumenPresupuestoMensual();
            }

            if (mes < 1 || mes > 12)
            {
                return new ResumenPresupuestoMensual();
            }

            return objCapaDato.ResumenGeneral(anio, mes);
        }

        private void PrepararDatos(PresupuestoMensual obj)
        {
            obj.Estado = obj.Estado == null ? "" : obj.Estado.Trim();

            if (obj.Estado == "")
            {
                obj.Estado = "Abierto";
            }

            obj.MesNombre = obj.MesNombre == null ? "" : obj.MesNombre.Trim();
        }

        private string Validar(PresupuestoMensual obj)
        {
            if (obj.Anio < 2000 || obj.Anio > 2100)
            {
                return "Debe ingresar un año válido.";
            }

            if (obj.Mes < 1 || obj.Mes > 12)
            {
                return "Debe ingresar un mes válido.";
            }

            if (string.IsNullOrWhiteSpace(obj.Estado))
            {
                return "Debe ingresar el estado.";
            }

            if (obj.Estado.Length > 45)
            {
                return "El estado no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.Estado, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "El estado solo puede contener letras, números, espacios y caracteres básicos.";
            }

            return "";
        }
    }
}