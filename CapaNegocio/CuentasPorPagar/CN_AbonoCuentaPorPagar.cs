using CapaDatos.CuentasPorPagar;
using CapaEntidad.CuentasPorPagar;
using System;
using System.Collections.Generic;

namespace CapaNegocio.CuentasPorPagar
{
    public class CN_AbonoCuentaPorPagar
    {
        private CD_AbonoCuentaPorPagar objCapaDato = new CD_AbonoCuentaPorPagar();

        public List<AbonoCuentaPorPagar> ListarPorCuenta(int idCuentaPorPagar)
        {
            return objCapaDato.ListarPorCuenta(idCuentaPorPagar);
        }

        public int Registrar(AbonoCuentaPorPagar obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdCuentaPorPagar == 0)
            {
                Mensaje = "Debe seleccionar una cuenta por pagar.";
                return 0;
            }

            if (obj.FechaAbono == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha del abono.";
                return 0;
            }

            if (obj.MontoAbono <= 0)
            {
                Mensaje = "El monto del abono debe ser mayor a cero.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.Observacion) && obj.Observacion.Length > 150)
            {
                Mensaje = "La observación no puede superar los 150 caracteres.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.Observacion))
            {
                obj.Observacion = obj.Observacion.Trim();
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(AbonoCuentaPorPagar obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdAbonoCuentaPorPagar == 0)
            {
                Mensaje = "Debe seleccionar un abono válido.";
                return false;
            }

            if (obj.FechaAbono == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha del abono.";
                return false;
            }

            if (obj.Activo && obj.MontoAbono <= 0)
            {
                Mensaje = "El monto del abono debe ser mayor a cero.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.Observacion) && obj.Observacion.Length > 150)
            {
                Mensaje = "La observación no puede superar los 150 caracteres.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.Observacion))
            {
                obj.Observacion = obj.Observacion.Trim();
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idAbonoCuentaPorPagar, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idAbonoCuentaPorPagar == 0)
            {
                Mensaje = "Debe seleccionar un abono válido.";
                return false;
            }

            return objCapaDato.Inactivar(idAbonoCuentaPorPagar, out Mensaje);
        }
    }
}