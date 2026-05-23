using CapaDatos.CuentasPorPagar;
using CapaEntidad.CuentasPorPagar;
using System;
using System.Collections.Generic;

namespace CapaNegocio.CuentasPorPagar
{
    public class CN_CuentaPorPagar
    {
        private CD_CuentaPorPagar objCapaDato = new CD_CuentaPorPagar();

        public List<CuentaPorPagar> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<CuentaPorPagar> ListarPendientes()
        {
            return objCapaDato.ListarPendientes();
        }

        public int Registrar(CuentaPorPagar obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.IdentificacionProveedor))
            {
                Mensaje = "Debe seleccionar un proveedor.";
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

            if (string.IsNullOrWhiteSpace(obj.Concepto))
            {
                Mensaje = "Debe ingresar el concepto.";
                return 0;
            }

            if (obj.Concepto.Length > 45)
            {
                Mensaje = "El concepto no puede superar los 45 caracteres.";
                return 0;
            }

            if (obj.MontoOriginal <= 0)
            {
                Mensaje = "El monto original debe ser mayor a cero.";
                return 0;
            }

            obj.IdentificacionProveedor = obj.IdentificacionProveedor.Trim();
           
            obj.Concepto = obj.Concepto.Trim();

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(CuentaPorPagar obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdCuentaPorPagar == 0)
            {
                Mensaje = "Debe seleccionar una cuenta por pagar válida.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.NumeroDocumento))
            {
                Mensaje = "Debe ingresar el número de documento.";
                return false;
            }

            if (obj.NumeroDocumento.Length > 45)
            {
                Mensaje = "El número de documento no puede superar los 45 caracteres.";
                return false;
            }

            if (obj.FechaVencimiento == DateTime.MinValue)
            {
                Mensaje = "Debe ingresar la fecha de vencimiento.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Concepto))
            {
                Mensaje = "Debe ingresar el concepto.";
                return false;
            }

            if (obj.Concepto.Length > 45)
            {
                Mensaje = "El concepto no puede superar los 45 caracteres.";
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

            obj.NumeroDocumento = obj.NumeroDocumento.Trim();
            obj.Concepto = obj.Concepto.Trim();

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idCuentaPorPagar, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idCuentaPorPagar == 0)
            {
                Mensaje = "Debe seleccionar una cuenta por pagar válida.";
                return false;
            }

            return objCapaDato.Inactivar(idCuentaPorPagar, out Mensaje);
        }
    }
}