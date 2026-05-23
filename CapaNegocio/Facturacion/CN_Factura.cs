using CapaDatos.Facturacion;
using CapaEntidad.Facturacion;
using System;
using System.Collections.Generic;

namespace CapaNegocio.Facturacion
{
    public class CN_Factura
    {
        private CD_Factura objCapaDato = new CD_Factura();

        public List<Factura> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Factura obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.IdentificacionCliente))
            {
                Mensaje = "Debe seleccionar un cliente.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.IdentificacionEmpleado))
            {
                Mensaje = "Debe seleccionar un empleado.";
                return 0;
            }

            if (obj.IdTipoPago == 0)
            {
                Mensaje = "Debe seleccionar un tipo de pago.";
                return 0;
            }

            if (obj.IdTipoFactura == 0)
            {
                Mensaje = "Debe seleccionar un tipo de factura.";
                return 0;
            }

            if (obj.FechaFactura == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha de la factura.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.Estado))
            {
                obj.Estado = "Borrador";
            }

            obj.Estado = obj.Estado.Trim();

            if (obj.Estado != "Borrador" && obj.Estado != "Emitida" && obj.Estado != "Anulada")
            {
                Mensaje = "El estado debe ser Borrador, Emitida o Anulada.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Factura obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdFactura == 0)
            {
                Mensaje = "Debe seleccionar una factura válida.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.IdentificacionCliente))
            {
                Mensaje = "Debe seleccionar un cliente.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.IdentificacionEmpleado))
            {
                Mensaje = "Debe seleccionar un empleado.";
                return false;
            }

            if (obj.IdTipoPago == 0)
            {
                Mensaje = "Debe seleccionar un tipo de pago.";
                return false;
            }

            if (obj.IdTipoFactura == 0)
            {
                Mensaje = "Debe seleccionar un tipo de factura.";
                return false;
            }

            if (obj.FechaFactura == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha de la factura.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Estado))
            {
                Mensaje = "Debe seleccionar un estado.";
                return false;
            }

            obj.Estado = obj.Estado.Trim();

            if (obj.Estado != "Borrador" && obj.Estado != "Emitida" && obj.Estado != "Anulada")
            {
                Mensaje = "El estado debe ser Borrador, Emitida o Anulada.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idFactura, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idFactura == 0)
            {
                Mensaje = "Debe seleccionar una factura válida.";
                return false;
            }

            return objCapaDato.Inactivar(idFactura, out Mensaje);
        }

        public bool Emitir(int idFactura, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idFactura == 0)
            {
                Mensaje = "Debe seleccionar una factura válida.";
                return false;
            }

            return objCapaDato.Emitir(idFactura, out Mensaje);
        }
    }
}