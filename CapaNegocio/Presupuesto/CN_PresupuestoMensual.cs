using CapaDatos.Presupuesto;
using CapaEntidad.Presupuesto;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Presupuesto
{
    public class CN_PresupuestoMensual
    {
        private CD_PresupuestoMensual objCapaDato = new CD_PresupuestoMensual();

        public List<PresupuestoMensual> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(PresupuestoMensual obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Anio < 2000 || obj.Anio > 2100)
            {
                Mensaje = "Debe ingresar un año válido.";
                return 0;
            }

            if (obj.Mes < 1 || obj.Mes > 12)
            {
                Mensaje = "Debe seleccionar un mes válido.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(PresupuestoMensual obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdPresupuestoMensual == 0)
            {
                Mensaje = "Debe seleccionar un presupuesto válido.";
                return false;
            }

            if (obj.Anio < 2000 || obj.Anio > 2100)
            {
                Mensaje = "Debe ingresar un año válido.";
                return false;
            }

            if (obj.Mes < 1 || obj.Mes > 12)
            {
                Mensaje = "Debe seleccionar un mes válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Estado))
            {
                Mensaje = "Debe seleccionar un estado.";
                return false;
            }

            obj.Estado = obj.Estado.Trim();

            if (obj.Estado != "Borrador" &&
                obj.Estado != "Aprobado" &&
                obj.Estado != "Cerrado" &&
                obj.Estado != "Anulado")
            {
                Mensaje = "El estado debe ser Borrador, Aprobado, Cerrado o Anulado.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idPresupuestoMensual, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idPresupuestoMensual == 0)
            {
                Mensaje = "Debe seleccionar un presupuesto válido.";
                return false;
            }

            return objCapaDato.Inactivar(idPresupuestoMensual, out Mensaje);
        }

        public List<ResumenPresupuestoMensual> ResumenVsReal(int idPresupuestoMensual)
        {
            return objCapaDato.ResumenVsReal(idPresupuestoMensual);
        }
    }
}