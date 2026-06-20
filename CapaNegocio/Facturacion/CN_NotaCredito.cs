using CapaDatos.Facturacion;
using CapaEntidad.Facturacion;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Facturacion
{
    public class CN_NotaCredito
    {
        private CD_NotaCredito objCapaDato = new CD_NotaCredito();

        public List<NotaCredito> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<NotaCredito> ListarActivas()
        {
            return objCapaDato.ListarActivas();
        }

        public List<NotaCredito> ListarPorFactura(string numeroFactura)
        {
            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();

            if (numeroFactura == "")
            {
                return new List<NotaCredito>();
            }

            if (numeroFactura.Length > 45)
            {
                return new List<NotaCredito>();
            }

            return objCapaDato.ListarPorFactura(numeroFactura);
        }

        public NotaCredito Obtener(string numeroNotaCredito)
        {
            numeroNotaCredito = numeroNotaCredito == null ? "" : numeroNotaCredito.Trim();

            if (numeroNotaCredito == "")
            {
                return null;
            }

            if (numeroNotaCredito.Length > 45)
            {
                return null;
            }

            return objCapaDato.Obtener(numeroNotaCredito);
        }

        public int Registrar(NotaCredito obj, out string Mensaje, out string NumeroNotaCreditoGenerado)
        {
            Mensaje = string.Empty;
            NumeroNotaCreditoGenerado = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje, out NumeroNotaCreditoGenerado);
        }

        public bool Editar(NotaCredito obj, out string Mensaje)
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

        public bool Inactivar(string numeroNotaCredito, out string Mensaje)
        {
            Mensaje = string.Empty;

            numeroNotaCredito = numeroNotaCredito == null ? "" : numeroNotaCredito.Trim();

            if (numeroNotaCredito == "")
            {
                Mensaje = "Debe seleccionar una nota de crédito válida.";
                return false;
            }

            if (numeroNotaCredito.Length > 45)
            {
                Mensaje = "El número de nota de crédito no puede superar los 45 caracteres.";
                return false;
            }

            if (!Regex.IsMatch(numeroNotaCredito, @"^[0-9A-Za-z.\-_]+$"))
            {
                Mensaje = "El número de nota de crédito solo puede contener letras, números, puntos, guiones y guion bajo.";
                return false;
            }

            return objCapaDato.Inactivar(numeroNotaCredito, out Mensaje);
        }

        private void PrepararDatos(NotaCredito obj)
        {
            obj.NumeroNotaCredito = obj.NumeroNotaCredito == null ? "" : obj.NumeroNotaCredito.Trim();
            obj.NumeroFactura = obj.NumeroFactura == null ? "" : obj.NumeroFactura.Trim();

            obj.IdentificacionCliente = obj.IdentificacionCliente == null ? "" : obj.IdentificacionCliente.Trim();
            obj.ClienteNombre = obj.ClienteNombre == null ? "" : obj.ClienteNombre.Trim();

            obj.IdentificacionEmpleado = obj.IdentificacionEmpleado == null ? "" : obj.IdentificacionEmpleado.Trim();
            obj.EmpleadoNombre = obj.EmpleadoNombre == null ? "" : obj.EmpleadoNombre.Trim();

            obj.Motivo = obj.Motivo == null ? "" : obj.Motivo.Trim();
            obj.Estado = obj.Estado == null ? "" : obj.Estado.Trim();

            obj.EstadoFactura = obj.EstadoFactura == null ? "" : obj.EstadoFactura.Trim();

            if (obj.Estado == "")
            {
                obj.Estado = "Aplicada";
            }

            if (obj.FechaNotaCredito == DateTime.MinValue)
            {
                obj.FechaNotaCredito = DateTime.Now;
            }

            if (obj.Subtotal < 0)
            {
                obj.Subtotal = 0;
            }

            if (obj.TotalImpuesto < 0)
            {
                obj.TotalImpuesto = 0;
            }

            if (obj.TotalNotaCredito < 0)
            {
                obj.TotalNotaCredito = 0;
            }
        }

        private string Validar(NotaCredito obj, bool esEdicion)
        {
            if (esEdicion)
            {
                if (string.IsNullOrWhiteSpace(obj.NumeroNotaCredito))
                {
                    return "Debe seleccionar una nota de crédito válida.";
                }

                if (obj.NumeroNotaCredito.Length > 45)
                {
                    return "El número de nota de crédito no puede superar los 45 caracteres.";
                }

                if (!Regex.IsMatch(obj.NumeroNotaCredito, @"^[0-9A-Za-z.\-_]+$"))
                {
                    return "El número de nota de crédito solo puede contener letras, números, puntos, guiones y guion bajo.";
                }
            }

            if (!esEdicion)
            {
                if (string.IsNullOrWhiteSpace(obj.NumeroFactura))
                {
                    return "Debe seleccionar una factura.";
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

            if (obj.FechaNotaCredito == DateTime.MinValue)
            {
                return "Debe ingresar la fecha de la nota de crédito.";
            }

            if (string.IsNullOrWhiteSpace(obj.Motivo))
            {
                return "Debe ingresar el motivo de la nota de crédito.";
            }

            if (obj.Motivo.Length > 250)
            {
                return "El motivo no puede superar los 250 caracteres.";
            }

            if (!Regex.IsMatch(obj.Motivo, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "El motivo solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (!esEdicion)
            {
                if (obj.Subtotal < 0)
                {
                    return "El subtotal no puede ser negativo.";
                }

                if (obj.TotalImpuesto < 0)
                {
                    return "El total de impuesto no puede ser negativo.";
                }

                if (obj.TotalNotaCredito <= 0)
                {
                    return "El total de la nota de crédito debe ser mayor a cero.";
                }

                if (obj.TotalNotaCredito < obj.Subtotal)
                {
                    return "El total de la nota de crédito no puede ser menor que el subtotal.";
                }
            }

            if (string.IsNullOrWhiteSpace(obj.Estado))
            {
                return "Debe ingresar el estado.";
            }

            if (obj.Estado.Length > 45)
            {
                return "El estado no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.Estado, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "El estado solo puede contener letras, números, espacios y caracteres básicos.";
            }

            return "";
        }
    }
}