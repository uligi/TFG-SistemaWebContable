using CapaDatos.CuentasPorCobrar;
using CapaEntidad.CuentasPorCobrar;
using System;
using System.Collections.Generic;

namespace CapaNegocio.CuentasPorCobrar
{
    public class CN_AbonoCuentaPorCobrar
    {
        private CD_AbonoCuentaPorCobrar objCapaDato = new CD_AbonoCuentaPorCobrar();

        public List<AbonoCuentaPorCobrar> ListarPorCuenta(int idCuentaPorCobrar)
        {
            return objCapaDato.ListarPorCuenta(idCuentaPorCobrar);
        }

        public int Registrar(AbonoCuentaPorCobrar obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdCuentaPorCobrar == 0)
            {
                Mensaje = "Debe seleccionar una cuenta por cobrar.";
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

        public bool Editar(AbonoCuentaPorCobrar obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdAbonoCuentaPorCobrar == 0)
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

        public bool Inactivar(int idAbonoCuentaPorCobrar, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idAbonoCuentaPorCobrar == 0)
            {
                Mensaje = "Debe seleccionar un abono válido.";
                return false;
            }

            return objCapaDato.Inactivar(idAbonoCuentaPorCobrar, out Mensaje);
        }
    }
}