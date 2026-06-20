using CapaDatos.Cuentas;
using CapaEntidad.Cuentas;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Cuentas
{
    public class CN_AbonoCuentaPorCobrar
    {
        private CD_AbonoCuentaPorCobrar objCapaDato = new CD_AbonoCuentaPorCobrar();

        public List<AbonoCuentaPorCobrar> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<AbonoCuentaPorCobrar> ListarPorCuenta(string identificacionCliente, string numeroFactura)
        {
            identificacionCliente = identificacionCliente == null ? "" : identificacionCliente.Trim();
            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();

            if (identificacionCliente == "" || numeroFactura == "")
            {
                return new List<AbonoCuentaPorCobrar>();
            }

            if (identificacionCliente.Length > 45 || numeroFactura.Length > 45)
            {
                return new List<AbonoCuentaPorCobrar>();
            }

            return objCapaDato.ListarPorCuenta(identificacionCliente, numeroFactura);
        }

        public AbonoCuentaPorCobrar Obtener(string identificacionCliente, string numeroFactura, int numeroAbono)
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

            if (numeroAbono <= 0)
            {
                return null;
            }

            return objCapaDato.Obtener(identificacionCliente, numeroFactura, numeroAbono);
        }

        public int Registrar(AbonoCuentaPorCobrar obj, out string Mensaje, out int NumeroAbonoGenerado)
        {
            Mensaje = string.Empty;
            NumeroAbonoGenerado = 0;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje, out NumeroAbonoGenerado);
        }

        public bool Editar(AbonoCuentaPorCobrar obj, out string Mensaje)
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

        public bool Inactivar(string identificacionCliente, string numeroFactura, int numeroAbono, out string Mensaje)
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

            if (numeroAbono <= 0)
            {
                Mensaje = "Debe seleccionar un número de abono válido.";
                return false;
            }

            return objCapaDato.Inactivar(identificacionCliente, numeroFactura, numeroAbono, out Mensaje);
        }

        private void PrepararDatos(AbonoCuentaPorCobrar obj)
        {
            obj.IdentificacionCliente = obj.IdentificacionCliente == null ? "" : obj.IdentificacionCliente.Trim();
            obj.NumeroFactura = obj.NumeroFactura == null ? "" : obj.NumeroFactura.Trim();
            obj.ClienteNombre = obj.ClienteNombre == null ? "" : obj.ClienteNombre.Trim();
            obj.EstadoCuenta = obj.EstadoCuenta == null ? "" : obj.EstadoCuenta.Trim();
            obj.Observacion = obj.Observacion == null ? "" : obj.Observacion.Trim();

            if (obj.FechaAbono == DateTime.MinValue)
            {
                obj.FechaAbono = DateTime.Now;
            }

            if (obj.MontoAbono < 0)
            {
                obj.MontoAbono = 0;
            }
        }

        private string Validar(AbonoCuentaPorCobrar obj, bool esEdicion)
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

            if (esEdicion)
            {
                if (obj.NumeroAbono <= 0)
                {
                    return "Debe seleccionar un número de abono válido.";
                }
            }

            if (obj.FechaAbono == DateTime.MinValue)
            {
                return "Debe ingresar la fecha del abono.";
            }

            if (obj.MontoAbono <= 0)
            {
                return "El monto del abono debe ser mayor a cero.";
            }

            if (obj.Observacion.Length > 150)
            {
                return "La observación no puede superar los 150 caracteres.";
            }

            if (obj.Observacion != "" &&
                !Regex.IsMatch(obj.Observacion, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "La observación solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.MontoOriginal < 0)
            {
                return "El monto original no puede ser negativo.";
            }

            if (obj.SaldoActual < 0)
            {
                return "El saldo actual no puede ser negativo.";
            }

            return "";
        }
    }
}