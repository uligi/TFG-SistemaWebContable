using CapaDatos.Movimientos;
using CapaEntidad.Movimientos;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Movimientos
{
    public class CN_Ingreso
    {
        private CD_Ingreso objCapaDato = new CD_Ingreso();

        public List<Ingreso> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<FacturaContadoPendiente> ListarFacturasContadoPendientes()
        {
            return objCapaDato.ListarFacturasContadoPendientes();
        }

        public List<AbonoPendienteIngreso> ListarAbonosPendientes()
        {
            return objCapaDato.ListarAbonosPendientes();
        }

        public int Registrar(Ingreso obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdFactura != null && obj.IdAbonoCuentaPorCobrar != null)
            {
                Mensaje = "El ingreso no puede estar asociado a una factura y a un abono al mismo tiempo.";
                return 0;
            }

            if (obj.FechaIngreso == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha del ingreso.";
                return 0;
            }

            if (obj.IdCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una cuenta contable.";
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

            if (obj.IdTipoIngreso == 0)
            {
                Mensaje = "Debe seleccionar un tipo de ingreso.";
                return 0;
            }

            if (obj.Monto <= 0)
            {
                Mensaje = "El monto del ingreso debe ser mayor a cero.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.OrigenIngreso) && obj.OrigenIngreso.Length > 45)
            {
                Mensaje = "El origen del ingreso no puede superar los 45 caracteres.";
                return 0;
            }

            obj.Descripcion = obj.Descripcion.Trim();

            if (!string.IsNullOrWhiteSpace(obj.OrigenIngreso))
            {
                obj.OrigenIngreso = obj.OrigenIngreso.Trim();
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Ingreso obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdIngreso == 0)
            {
                Mensaje = "Debe seleccionar un ingreso válido.";
                return false;
            }

            if (obj.FechaIngreso == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha del ingreso.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "Debe ingresar una descripción.";
                return false;
            }

            if (obj.IdCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una cuenta contable.";
                return false;
            }

            if (obj.Descripcion.Length > 150)
            {
                Mensaje = "La descripción no puede superar los 150 caracteres.";
                return false;
            }

            if (obj.IdTipoIngreso == 0)
            {
                Mensaje = "Debe seleccionar un tipo de ingreso.";
                return false;
            }

            if (obj.Activo && obj.Monto <= 0)
            {
                Mensaje = "El monto del ingreso debe ser mayor a cero.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.OrigenIngreso))
            {
                Mensaje = "Debe indicar el origen del ingreso.";
                return false;
            }

            if (obj.OrigenIngreso.Length > 45)
            {
                Mensaje = "El origen del ingreso no puede superar los 45 caracteres.";
                return false;
            }

            obj.Descripcion = obj.Descripcion.Trim();
            obj.OrigenIngreso = obj.OrigenIngreso.Trim();

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idIngreso, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idIngreso == 0)
            {
                Mensaje = "Debe seleccionar un ingreso válido.";
                return false;
            }

            return objCapaDato.Inactivar(idIngreso, out Mensaje);
        }
    }
}