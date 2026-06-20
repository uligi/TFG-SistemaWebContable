using CapaDatos.Cuentas;
using CapaEntidad.Cuentas;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Cuentas
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

        public List<CuentaPorCobrar> ListarPorCliente(string identificacionCliente)
        {
            identificacionCliente = identificacionCliente == null ? "" : identificacionCliente.Trim();

            if (identificacionCliente == "")
            {
                return new List<CuentaPorCobrar>();
            }

            if (identificacionCliente.Length > 45)
            {
                return new List<CuentaPorCobrar>();
            }

            return objCapaDato.ListarPorCliente(identificacionCliente);
        }

        public CuentaPorCobrar Obtener(string identificacionCliente, string numeroFactura)
        {
            identificacionCliente = identificacionCliente == null ? "" : identificacionCliente.Trim();
            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();

            if (identificacionCliente == "" || numeroFactura == "")
            {
                return null;
            }

            if (identificacionCliente.Length > 45 || numeroFactura.Length > 45)
            {
                return null;
            }

            return objCapaDato.Obtener(identificacionCliente, numeroFactura);
        }

        public int Registrar(CuentaPorCobrar obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(CuentaPorCobrar obj, out string Mensaje)
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

        public bool RecalcularSaldo(string identificacionCliente, string numeroFactura, out string Mensaje)
        {
            Mensaje = string.Empty;

            identificacionCliente = identificacionCliente == null ? "" : identificacionCliente.Trim();
            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();

            if (identificacionCliente == "")
            {
                Mensaje = "Debe seleccionar un cliente.";
                return false;
            }

            if (numeroFactura == "")
            {
                Mensaje = "Debe seleccionar una factura.";
                return false;
            }

            if (identificacionCliente.Length > 45)
            {
                Mensaje = "La identificación del cliente no puede superar los 45 caracteres.";
                return false;
            }

            if (numeroFactura.Length > 45)
            {
                Mensaje = "El número de factura no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.RecalcularSaldo(identificacionCliente, numeroFactura, out Mensaje);
        }

        public bool Inactivar(string identificacionCliente, string numeroFactura, out string Mensaje)
        {
            Mensaje = string.Empty;

            identificacionCliente = identificacionCliente == null ? "" : identificacionCliente.Trim();
            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();

            if (identificacionCliente == "")
            {
                Mensaje = "Debe seleccionar un cliente.";
                return false;
            }

            if (numeroFactura == "")
            {
                Mensaje = "Debe seleccionar una factura.";
                return false;
            }

            if (identificacionCliente.Length > 45)
            {
                Mensaje = "La identificación del cliente no puede superar los 45 caracteres.";
                return false;
            }

            if (numeroFactura.Length > 45)
            {
                Mensaje = "El número de factura no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.Inactivar(identificacionCliente, numeroFactura, out Mensaje);
        }

        private void PrepararDatos(CuentaPorCobrar obj)
        {
            obj.IdentificacionCliente = obj.IdentificacionCliente == null ? "" : obj.IdentificacionCliente.Trim();
            obj.NumeroFactura = obj.NumeroFactura == null ? "" : obj.NumeroFactura.Trim();
            obj.ClienteNombre = obj.ClienteNombre == null ? "" : obj.ClienteNombre.Trim();
            obj.Estado = obj.Estado == null ? "" : obj.Estado.Trim();
            obj.EstadoFactura = obj.EstadoFactura == null ? "" : obj.EstadoFactura.Trim();

            if (obj.Estado == "")
            {
                obj.Estado = "Pendiente";
            }

            if (obj.FechaEmision == DateTime.MinValue)
            {
                obj.FechaEmision = DateTime.Now;
            }

            if (obj.FechaVencimiento == DateTime.MinValue)
            {
                obj.FechaVencimiento = obj.FechaEmision.AddDays(30);
            }
        }

        private string Validar(CuentaPorCobrar obj, bool esEdicion)
        {
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

            if (obj.FechaEmision == DateTime.MinValue)
            {
                return "Debe ingresar la fecha de emisión.";
            }

            if (obj.FechaVencimiento == DateTime.MinValue)
            {
                return "Debe ingresar la fecha de vencimiento.";
            }

            if (obj.FechaVencimiento < obj.FechaEmision)
            {
                return "La fecha de vencimiento no puede ser menor que la fecha de emisión.";
            }

            if (obj.MontoOriginal <= 0)
            {
                return "El monto original debe ser mayor a cero.";
            }

            if (obj.SaldoActual < 0)
            {
                return "El saldo actual no puede ser negativo.";
            }

            if (esEdicion && obj.SaldoActual > obj.MontoOriginal)
            {
                return "El saldo actual no puede ser mayor que el monto original.";
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