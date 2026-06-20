using CapaDatos.Movimientos;
using CapaEntidad.Movimientos;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Movimientos
{
    public class CN_Ingreso
    {
        private CD_Ingreso objCapaDato = new CD_Ingreso();

        public List<Ingreso> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<Ingreso> ListarActivos()
        {
            return objCapaDato.ListarActivos();
        }

        public Ingreso Obtener(string numeroIngreso)
        {
            numeroIngreso = numeroIngreso == null ? "" : numeroIngreso.Trim();

            if (numeroIngreso == "")
            {
                return null;
            }

            if (numeroIngreso.Length > 45)
            {
                return null;
            }

            return objCapaDato.Obtener(numeroIngreso);
        }

        public int Registrar(Ingreso obj, out string Mensaje, out string NumeroIngresoGenerado)
        {
            Mensaje = string.Empty;
            NumeroIngresoGenerado = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje, out NumeroIngresoGenerado);
        }

        public bool Editar(Ingreso obj, out string Mensaje)
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

        public bool Inactivar(string numeroIngreso, out string Mensaje)
        {
            Mensaje = string.Empty;

            numeroIngreso = numeroIngreso == null ? "" : numeroIngreso.Trim();

            if (numeroIngreso == "")
            {
                Mensaje = "Debe seleccionar un ingreso válido.";
                return false;
            }

            if (numeroIngreso.Length > 45)
            {
                Mensaje = "El número de ingreso no puede superar los 45 caracteres.";
                return false;
            }

            if (!Regex.IsMatch(numeroIngreso, @"^[0-9A-Za-z.\-_]+$"))
            {
                Mensaje = "El número de ingreso solo puede contener letras, números, puntos, guiones y guion bajo.";
                return false;
            }

            return objCapaDato.Inactivar(numeroIngreso, out Mensaje);
        }

        public List<FacturaContadoPendiente> ListarFacturasContadoPendientes()
        {
            return objCapaDato.ListarFacturasContadoPendientes();
        }

        public List<AbonoPendienteIngreso> ListarAbonosPendientes()
        {
            return objCapaDato.ListarAbonosPendientes();
        }

        private void PrepararDatos(Ingreso obj)
        {
            obj.NumeroIngreso = obj.NumeroIngreso == null ? "" : obj.NumeroIngreso.Trim();

            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            obj.TipoIngresoNombre = obj.TipoIngresoNombre == null ? "" : obj.TipoIngresoNombre.Trim();

            obj.CodigoCuenta = obj.CodigoCuenta == null ? "" : obj.CodigoCuenta.Trim();
            obj.NombreCuenta = obj.NombreCuenta == null ? "" : obj.NombreCuenta.Trim();

            obj.IdentificacionCliente = obj.IdentificacionCliente == null ? "" : obj.IdentificacionCliente.Trim();
            obj.NumeroFactura = obj.NumeroFactura == null ? "" : obj.NumeroFactura.Trim();
            obj.ClienteNombre = obj.ClienteNombre == null ? "" : obj.ClienteNombre.Trim();

            obj.IdentificacionClienteAbono = obj.IdentificacionClienteAbono == null ? "" : obj.IdentificacionClienteAbono.Trim();
            obj.NumeroFacturaAbono = obj.NumeroFacturaAbono == null ? "" : obj.NumeroFacturaAbono.Trim();

            obj.OrigenIngreso = obj.OrigenIngreso == null ? "" : obj.OrigenIngreso.Trim();

            if (obj.FechaIngreso == DateTime.MinValue)
            {
                obj.FechaIngreso = DateTime.Now;
            }

            if (obj.OrigenIngreso == "")
            {
                obj.OrigenIngreso = "Manual";
            }

            if (obj.IdentificacionCliente == "")
            {
                obj.IdentificacionCliente = "000000000";
            }

            if (obj.NumeroFactura == "")
            {
                obj.NumeroFactura = "FAC-GENERAL-000000";
            }

            if (obj.IdentificacionClienteAbono == "")
            {
                obj.IdentificacionClienteAbono = "000000000";
            }

            if (obj.NumeroFacturaAbono == "")
            {
                obj.NumeroFacturaAbono = "FAC-GENERAL-000000";
            }

            if (obj.NumeroAbonoCuentaPorCobrar < 0)
            {
                obj.NumeroAbonoCuentaPorCobrar = 0;
            }

            if (obj.Monto < 0)
            {
                obj.Monto = 0;
            }
        }

        private string Validar(Ingreso obj, bool esEdicion)
        {
            if (esEdicion)
            {
                if (string.IsNullOrWhiteSpace(obj.NumeroIngreso))
                {
                    return "Debe seleccionar un ingreso válido.";
                }

                if (obj.NumeroIngreso.Length > 45)
                {
                    return "El número de ingreso no puede superar los 45 caracteres.";
                }

                if (!Regex.IsMatch(obj.NumeroIngreso, @"^[0-9A-Za-z.\-_]+$"))
                {
                    return "El número de ingreso solo puede contener letras, números, puntos, guiones y guion bajo.";
                }
            }

            if (obj.FechaIngreso == DateTime.MinValue)
            {
                return "Debe ingresar la fecha del ingreso.";
            }

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                return "Debe ingresar una descripción.";
            }

            if (obj.Descripcion.Length > 150)
            {
                return "La descripción no puede superar los 150 caracteres.";
            }

            if (!Regex.IsMatch(obj.Descripcion, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "La descripción solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.IdTipoIngreso <= 0)
            {
                return "Debe seleccionar un tipo de ingreso.";
            }

            if (string.IsNullOrWhiteSpace(obj.CodigoCuenta))
            {
                return "Debe seleccionar una cuenta contable.";
            }

            if (obj.CodigoCuenta.Length > 45)
            {
                return "El código de cuenta no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.CodigoCuenta, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "El código de cuenta solo puede contener letras, números, puntos, guiones y guion bajo.";
            }

            if (string.IsNullOrWhiteSpace(obj.OrigenIngreso))
            {
                return "Debe indicar el origen del ingreso.";
            }

            if (obj.OrigenIngreso.Length > 45)
            {
                return "El origen del ingreso no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.OrigenIngreso, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "El origen del ingreso solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.Monto <= 0)
            {
                return "El monto del ingreso debe ser mayor a cero.";
            }

            if (obj.OrigenIngreso == "Factura")
            {
                if (string.IsNullOrWhiteSpace(obj.IdentificacionCliente) ||
                    obj.IdentificacionCliente == "000000000")
                {
                    return "Debe seleccionar el cliente de la factura.";
                }

                if (string.IsNullOrWhiteSpace(obj.NumeroFactura) ||
                    obj.NumeroFactura == "FAC-GENERAL-000000")
                {
                    return "Debe seleccionar una factura de contado.";
                }

                if (obj.IdentificacionCliente.Length > 45)
                {
                    return "La identificación del cliente no puede superar los 45 caracteres.";
                }

                if (obj.NumeroFactura.Length > 45)
                {
                    return "El número de factura no puede superar los 45 caracteres.";
                }
            }

            if (obj.OrigenIngreso == "Abono CxC")
            {
                if (string.IsNullOrWhiteSpace(obj.IdentificacionClienteAbono) ||
                    obj.IdentificacionClienteAbono == "000000000")
                {
                    return "Debe seleccionar el cliente del abono.";
                }

                if (string.IsNullOrWhiteSpace(obj.NumeroFacturaAbono) ||
                    obj.NumeroFacturaAbono == "FAC-GENERAL-000000")
                {
                    return "Debe seleccionar la factura del abono.";
                }

                if (obj.NumeroAbonoCuentaPorCobrar <= 0)
                {
                    return "Debe seleccionar un abono válido.";
                }

                if (obj.IdentificacionClienteAbono.Length > 45)
                {
                    return "La identificación del cliente del abono no puede superar los 45 caracteres.";
                }

                if (obj.NumeroFacturaAbono.Length > 45)
                {
                    return "El número de factura del abono no puede superar los 45 caracteres.";
                }
            }

            return "";
        }
    }
}