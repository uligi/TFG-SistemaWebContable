using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Contabilidad
{
    public class CN_AsientoContable
    {
        private CD_AsientoContable objCapaDato = new CD_AsientoContable();

        public List<AsientoContable> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(AsientoContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdPeriodoContable == 0)
            {
                Mensaje = "Debe seleccionar un período contable.";
                return 0;
            }

            if (obj.FechaAsiento == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha del asiento.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.TipoAsiento))
            {
                Mensaje = "Debe ingresar el tipo de asiento.";
                return 0;
            }

            if (obj.TipoAsiento.Length > 45)
            {
                Mensaje = "El tipo de asiento no puede superar los 45 caracteres.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.Concepto))
            {
                Mensaje = "Debe ingresar el concepto del asiento.";
                return 0;
            }

            if (obj.Concepto.Length > 45)
            {
                Mensaje = "El concepto no puede superar los 45 caracteres.";
                return 0;
            }

            obj.TipoAsiento = obj.TipoAsiento.Trim();
            obj.Concepto = obj.Concepto.Trim();

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(AsientoContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdAsientoContable == 0)
            {
                Mensaje = "Debe seleccionar un asiento contable válido.";
                return false;
            }

            if (obj.IdPeriodoContable == 0)
            {
                Mensaje = "Debe seleccionar un período contable.";
                return false;
            }

            if (obj.FechaAsiento == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha del asiento.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.TipoAsiento))
            {
                Mensaje = "Debe ingresar el tipo de asiento.";
                return false;
            }

            if (obj.TipoAsiento.Length > 45)
            {
                Mensaje = "El tipo de asiento no puede superar los 45 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Concepto))
            {
                Mensaje = "Debe ingresar el concepto del asiento.";
                return false;
            }

            if (obj.Concepto.Length > 45)
            {
                Mensaje = "El concepto no puede superar los 45 caracteres.";
                return false;
            }

            obj.TipoAsiento = obj.TipoAsiento.Trim();
            obj.Concepto = obj.Concepto.Trim();

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idAsientoContable, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idAsientoContable == 0)
            {
                Mensaje = "Debe seleccionar un asiento contable válido.";
                return false;
            }

            return objCapaDato.Inactivar(idAsientoContable, out Mensaje);
        }

        public bool ValidarCuadre(int idAsientoContable, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idAsientoContable == 0)
            {
                Mensaje = "Debe seleccionar un asiento contable válido.";
                return false;
            }

            return objCapaDato.ValidarCuadre(idAsientoContable, out Mensaje);
        }
    }
}