using CapaDatos.Facturacion;
using CapaEntidad.Facturacion;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Facturacion
{
    public class CN_Factura
    {
        private CD_Factura objCapaDato = new CD_Factura();

        public List<Factura> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<Factura> ListarActivas()
        {
            return objCapaDato.ListarActivas();
        }

        public List<Factura> ListarPorCliente(string identificacionCliente)
        {
            identificacionCliente = identificacionCliente == null ? "" : identificacionCliente.Trim();

            if (identificacionCliente == "")
            {
                return new List<Factura>();
            }

            if (identificacionCliente.Length > 45)
            {
                return new List<Factura>();
            }

            return objCapaDato.ListarPorCliente(identificacionCliente);
        }

        public Factura Obtener(string numeroFactura)
        {
            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();

            if (numeroFactura == "")
            {
                return null;
            }

            if (numeroFactura.Length > 45)
            {
                return null;
            }

            return objCapaDato.Obtener(numeroFactura);
        }

        public int Registrar(Factura obj, out string Mensaje, out string NumeroFacturaGenerado)
        {
            Mensaje = string.Empty;
            NumeroFacturaGenerado = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje, out NumeroFacturaGenerado);
        }

        public bool Editar(Factura obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, true);

            if (error != "")
            {
                Mensaje = error;
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool CambiarEstado(string numeroFactura, string estado, out string Mensaje)
        {
            Mensaje = string.Empty;

            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();
            estado = estado == null ? "" : estado.Trim();

            if (numeroFactura == "")
            {
                Mensaje = "Debe seleccionar una factura válida.";
                return false;
            }

            if (numeroFactura.Length > 45)
            {
                Mensaje = "El número de factura no puede superar los 45 caracteres.";
                return false;
            }

            if (estado == "")
            {
                Mensaje = "Debe ingresar el estado de la factura.";
                return false;
            }

            if (estado.Length > 45)
            {
                Mensaje = "El estado no puede superar los 45 caracteres.";
                return false;
            }

            if (!Regex.IsMatch(estado, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                Mensaje = "El estado solo puede contener letras, números, espacios y caracteres básicos.";
                return false;
            }

            return objCapaDato.CambiarEstado(numeroFactura, estado, out Mensaje);
        }

        public bool RecalcularTotales(string numeroFactura, out string Mensaje)
        {
            Mensaje = string.Empty;

            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();

            if (numeroFactura == "")
            {
                Mensaje = "Debe seleccionar una factura válida.";
                return false;
            }

            if (numeroFactura.Length > 45)
            {
                Mensaje = "El número de factura no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.RecalcularTotales(numeroFactura, out Mensaje);
        }

        public bool Inactivar(string numeroFactura, out string Mensaje)
        {
            Mensaje = string.Empty;

            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();

            if (numeroFactura == "")
            {
                Mensaje = "Debe seleccionar una factura válida.";
                return false;
            }

            if (numeroFactura.Length > 45)
            {
                Mensaje = "El número de factura no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.Inactivar(numeroFactura, out Mensaje);
        }

        private void PrepararDatos(Factura obj)
        {
            obj.NumeroFactura = obj.NumeroFactura == null ? "" : obj.NumeroFactura.Trim();

            obj.IdentificacionCliente = obj.IdentificacionCliente == null ? "" : obj.IdentificacionCliente.Trim();
            obj.ClienteNombre = obj.ClienteNombre == null ? "" : obj.ClienteNombre.Trim();

            obj.IdentificacionEmpleado = obj.IdentificacionEmpleado == null ? "" : obj.IdentificacionEmpleado.Trim();
            obj.EmpleadoNombre = obj.EmpleadoNombre == null ? "" : obj.EmpleadoNombre.Trim();

            obj.TipoPagoNombre = obj.TipoPagoNombre == null ? "" : obj.TipoPagoNombre.Trim();
            obj.TipoFacturaNombre = obj.TipoFacturaNombre == null ? "" : obj.TipoFacturaNombre.Trim();

            obj.Estado = obj.Estado == null ? "" : obj.Estado.Trim();

            if (obj.Estado == "")
            {
                obj.Estado = "Pendiente";
            }

            if (obj.FechaFactura == DateTime.MinValue)
            {
                obj.FechaFactura = DateTime.Now;
            }
        }

        private string Validar(Factura obj, bool esEdicion)
        {
            if (esEdicion)
            {
                if (string.IsNullOrWhiteSpace(obj.NumeroFactura))
                {
                    return "Debe seleccionar una factura válida.";
                }

                if (obj.NumeroFactura.Length > 45)
                {
                    return "El número de factura no puede superar los 45 caracteres.";
                }

                if (!Regex.IsMatch(obj.NumeroFactura, @"^[0-9A-Za-z.\-_]+$"))
                {
                    return "El número de factura solo puede contener letras, números, puntos, guiones y guion bajo.";
                }
            }

            if (string.IsNullOrWhiteSpace(obj.IdentificacionCliente))
            {
                return "Debe seleccionar un cliente.";
            }

            if (obj.IdentificacionCliente.Length > 45)
            {
                return "La identificación del cliente no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.IdentificacionCliente, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "La identificación del cliente tiene un formato inválido.";
            }

            if (string.IsNullOrWhiteSpace(obj.IdentificacionEmpleado))
            {
                return "Debe seleccionar un empleado.";
            }

            if (obj.IdentificacionEmpleado.Length > 45)
            {
                return "La identificación del empleado no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.IdentificacionEmpleado, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "La identificación del empleado tiene un formato inválido.";
            }

            if (obj.IdTipoPago <= 0)
            {
                return "Debe seleccionar un tipo de pago.";
            }

            if (obj.IdTipoFactura <= 0)
            {
                return "Debe seleccionar un tipo de factura.";
            }

            if (obj.FechaFactura == DateTime.MinValue)
            {
                return "Debe ingresar la fecha de la factura.";
            }

            if (string.IsNullOrWhiteSpace(obj.Estado))
            {
                return "Debe ingresar el estado de la factura.";
            }

            if (obj.Estado.Length > 45)
            {
                return "El estado no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.Estado, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "El estado solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.Subtotal < 0)
            {
                return "El subtotal no puede ser negativo.";
            }

            if (obj.TotalImpuesto < 0)
            {
                return "El total de impuesto no puede ser negativo.";
            }

            if (obj.TotalDescuento < 0)
            {
                return "El total de descuento no puede ser negativo.";
            }

            if (obj.TotalFactura < 0)
            {
                return "El total de factura no puede ser negativo.";
            }

            return "";
        }

        public bool Emitir(string numeroFactura, out string Mensaje)
        {
            Mensaje = string.Empty;

            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();

            if (numeroFactura == "")
            {
                Mensaje = "Debe seleccionar una factura válida.";
                return false;
            }

            if (numeroFactura.Length > 45)
            {
                Mensaje = "El número de factura no puede superar los 45 caracteres.";
                return false;
            }

            if (!Regex.IsMatch(numeroFactura, @"^[0-9A-Za-z.\-_]+$"))
            {
                Mensaje = "El número de factura solo puede contener letras, números, puntos, guiones y guion bajo.";
                return false;
            }

            return objCapaDato.Emitir(numeroFactura, out Mensaje);
        }
    }
}