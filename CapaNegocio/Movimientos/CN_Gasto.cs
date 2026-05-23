using CapaDatos.Movimientos;
using CapaEntidad.Movimientos;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Movimientos
{
    public class CN_Gasto
    {
        private CD_Gasto objCapaDato = new CD_Gasto();

        public List<Gasto> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Gasto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.FechaGasto == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha del gasto.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "Debe ingresar una descripción.";
                return 0;
            }

            if (obj.Descripcion.Length > 150)
            {
                Mensaje = "La descripción no puede superar los 150 caracteres.";
                return 0;
            }

            if (obj.IdTipoGasto == 0)
            {
                Mensaje = "Debe seleccionar un tipo de gasto.";
                return 0;
            }

            if (obj.IdCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una cuenta contable.";
                return 0;
            }

            if (obj.Monto <= 0)
            {
                Mensaje = "El monto del gasto debe ser mayor a cero.";
                return 0;
            }

            obj.Descripcion = obj.Descripcion.Trim();

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Gasto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdGasto == 0)
            {
                Mensaje = "Debe seleccionar un gasto válido.";
                return false;
            }

            if (obj.FechaGasto == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha del gasto.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "Debe ingresar una descripción.";
                return false;
            }

            if (obj.Descripcion.Length > 150)
            {
                Mensaje = "La descripción no puede superar los 150 caracteres.";
                return false;
            }

            if (obj.IdCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una cuenta contable.";
                return false;
            }

            if (obj.IdTipoGasto == 0)
            {
                Mensaje = "Debe seleccionar un tipo de gasto.";
                return false;
            }

            if (obj.Activo && obj.Monto <= 0)
            {
                Mensaje = "El monto del gasto debe ser mayor a cero.";
                return false;
            }

            obj.Descripcion = obj.Descripcion.Trim();

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idGasto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idGasto == 0)
            {
                Mensaje = "Debe seleccionar un gasto válido.";
                return false;
            }

            return objCapaDato.Inactivar(idGasto, out Mensaje);
        }
    }
}