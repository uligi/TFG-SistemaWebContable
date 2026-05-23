using CapaDatos.CuentasPorCobrar;
using CapaEntidad.CuentasPorCobrar;
using System;
using System.Collections.Generic;

namespace CapaNegocio.CuentasPorCobrar
{
    public class CN_CuentaPorCobrar
    {
        private CD_CuentaPorCobrar objCapaDato = new CD_CuentaPorCobrar();

        public List<CuentaPorCobrar> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<CuentaPorCobrar> ListarPendientes()
        {
            return objCapaDato.ListarPendientes();
        }

        public int Registrar(CuentaPorCobrar obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.IdentificacionCliente))
            {
                Mensaje = "Debe seleccionar un cliente.";
                return 0;
            }

            if (obj.IdFactura == 0)
            {
                Mensaje = "Debe seleccionar una factura.";
                return 0;
            }

            if (obj.FechaEmision == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha de emisión.";
                return 0;
            }

            if (obj.FechaVencimiento == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha de vencimiento.";
                return 0;
            }

            if (obj.FechaVencimiento < obj.FechaEmision)
            {
                Mensaje = "La fecha de vencimiento no puede ser menor que la fecha de emisión.";
                return 0;
            }

            if (obj.MontoOriginal <= 0)
            {
                Mensaje = "El monto original debe ser mayor a cero.";
                return 0;
            }

            obj.IdentificacionCliente = obj.IdentificacionCliente.Trim();

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(CuentaPorCobrar obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdCuentaPorCobrar == 0)
            {
                Mensaje = "Debe seleccionar una cuenta por cobrar válida.";
                return false;
            }

            if (obj.FechaVencimiento == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha de vencimiento.";
                return false;
            }

            if (obj.MontoOriginal <= 0)
            {
                Mensaje = "El monto original debe ser mayor a cero.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Estado))
            {
                Mensaje = "Debe seleccionar un estado.";
                return false;
            }

            obj.Estado = obj.Estado.Trim();

            if (obj.Estado != "Pendiente" &&
                obj.Estado != "Parcial" &&
                obj.Estado != "Pagada" &&
                obj.Estado != "Vencida" &&
                obj.Estado != "Anulada")
            {
                Mensaje = "El estado debe ser Pendiente, Parcial, Pagada, Vencida o Anulada.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idCuentaPorCobrar, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idCuentaPorCobrar == 0)
            {
                Mensaje = "Debe seleccionar una cuenta por cobrar válida.";
                return false;
            }

            return objCapaDato.Inactivar(idCuentaPorCobrar, out Mensaje);
        }
    }
}