using CapaDatos.CuentasPorPagar;
using CapaEntidad.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        public List<CuentaPorPagar> ListarPorProveedor(string identificacionProveedor)
        {
            identificacionProveedor = identificacionProveedor == null ? "" : identificacionProveedor.Trim();

            if (identificacionProveedor == "")
            {
                return new List<CuentaPorPagar>();
            }

            if (identificacionProveedor.Length > 45)
            {
                return new List<CuentaPorPagar>();
            }

            return objCapaDato.ListarPorProveedor(identificacionProveedor);
        }

        public CuentaPorPagar Obtener(string identificacionProveedor, string numeroDocumento)
        {
            identificacionProveedor = identificacionProveedor == null ? "" : identificacionProveedor.Trim();
            numeroDocumento = numeroDocumento == null ? "" : numeroDocumento.Trim();

            if (identificacionProveedor == "" || numeroDocumento == "")
            {
                return null;
            }

            if (identificacionProveedor.Length > 45 || numeroDocumento.Length > 45)
            {
                return null;
            }

            return objCapaDato.Obtener(identificacionProveedor, numeroDocumento);
        }

        public int Registrar(CuentaPorPagar obj, out string Mensaje, out string NumeroDocumentoGenerado)
        {
            Mensaje = string.Empty;
            NumeroDocumentoGenerado = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje, out NumeroDocumentoGenerado);
        }

        public bool Editar(CuentaPorPagar obj, out string Mensaje)
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

        public bool RecalcularSaldo(string identificacionProveedor, string numeroDocumento, out string Mensaje)
        {
            Mensaje = string.Empty;

            identificacionProveedor = identificacionProveedor == null ? "" : identificacionProveedor.Trim();
            numeroDocumento = numeroDocumento == null ? "" : numeroDocumento.Trim();

            if (identificacionProveedor == "" || numeroDocumento == "")
            {
                Mensaje = "Debe seleccionar una cuenta por pagar válida.";
                return false;
            }

            if (identificacionProveedor.Length > 45)
            {
                Mensaje = "La identificación del proveedor no puede superar los 45 caracteres.";
                return false;
            }

            if (numeroDocumento.Length > 45)
            {
                Mensaje = "El número de documento no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.RecalcularSaldo(identificacionProveedor, numeroDocumento, out Mensaje);
        }

        public bool Inactivar(string identificacionProveedor, string numeroDocumento, out string Mensaje)
        {
            Mensaje = string.Empty;

            identificacionProveedor = identificacionProveedor == null ? "" : identificacionProveedor.Trim();
            numeroDocumento = numeroDocumento == null ? "" : numeroDocumento.Trim();

            if (identificacionProveedor == "" || numeroDocumento == "")
            {
                Mensaje = "Debe seleccionar una cuenta por pagar válida.";
                return false;
            }

            if (identificacionProveedor.Length > 45)
            {
                Mensaje = "La identificación del proveedor no puede superar los 45 caracteres.";
                return false;
            }

            if (numeroDocumento.Length > 45)
            {
                Mensaje = "El número de documento no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.Inactivar(identificacionProveedor, numeroDocumento, out Mensaje);
        }

        private void PrepararDatos(CuentaPorPagar obj)
        {
            obj.IdentificacionProveedor = obj.IdentificacionProveedor == null ? "" : obj.IdentificacionProveedor.Trim();
            obj.ProveedorNombre = obj.ProveedorNombre == null ? "" : obj.ProveedorNombre.Trim();

            obj.NumeroDocumento = obj.NumeroDocumento == null ? "" : obj.NumeroDocumento.Trim();

            obj.Concepto = obj.Concepto == null ? "" : obj.Concepto.Trim();
            obj.Estado = obj.Estado == null ? "" : obj.Estado.Trim();
            obj.EstadoCredito = obj.EstadoCredito == null ? "" : obj.EstadoCredito.Trim();

            if (obj.FechaEmision == DateTime.MinValue)
            {
                obj.FechaEmision = DateTime.Now;
            }

            if (obj.FechaVencimiento == DateTime.MinValue)
            {
                obj.FechaVencimiento = obj.FechaEmision;
            }

            if (obj.Estado == "")
            {
                obj.Estado = "Pendiente";
            }

            if (obj.MontoOriginal < 0)
            {
                obj.MontoOriginal = 0;
            }

            if (obj.SaldoActual < 0)
            {
                obj.SaldoActual = 0;
            }

            if (obj.SaldoActual == 0 && obj.MontoOriginal > 0 && obj.Estado == "Pendiente")
            {
                obj.SaldoActual = obj.MontoOriginal;
            }
        }

        private string Validar(CuentaPorPagar obj, bool esEdicion)
        {
            if (string.IsNullOrWhiteSpace(obj.IdentificacionProveedor))
            {
                return "Debe seleccionar un proveedor.";
            }

            if (obj.IdentificacionProveedor.Length > 45)
            {
                return "La identificación del proveedor no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.IdentificacionProveedor, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "La identificación del proveedor solo puede contener letras, números, puntos, guiones y guion bajo.";
            }

            if (esEdicion && string.IsNullOrWhiteSpace(obj.NumeroDocumento))
            {
                return "Debe seleccionar una cuenta por pagar válida.";
            }

            if (obj.NumeroDocumento.Length > 45)
            {
                return "El número de documento no puede superar los 45 caracteres.";
            }

            if (!string.IsNullOrWhiteSpace(obj.NumeroDocumento) &&
                !Regex.IsMatch(obj.NumeroDocumento, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "El número de documento solo puede contener letras, números, puntos, guiones y guion bajo.";
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

            if (string.IsNullOrWhiteSpace(obj.Concepto))
            {
                return "Debe ingresar el concepto de la cuenta por pagar.";
            }

            if (obj.Concepto.Length > 100)
            {
                return "El concepto no puede superar los 100 caracteres.";
            }

            if (!Regex.IsMatch(obj.Concepto, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "El concepto solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.MontoOriginal <= 0)
            {
                return "El monto original debe ser mayor a cero.";
            }

            if (esEdicion)
            {
                if (obj.SaldoActual < 0)
                {
                    return "El saldo actual no puede ser negativo.";
                }

                if (obj.SaldoActual > obj.MontoOriginal)
                {
                    return "El saldo actual no puede ser mayor que el monto original.";
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