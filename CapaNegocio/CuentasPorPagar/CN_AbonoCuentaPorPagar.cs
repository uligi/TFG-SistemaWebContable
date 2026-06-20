using CapaDatos.CuentasPorPagar;
using CapaEntidad.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.CuentasPorPagar
{
    public class CN_AbonoCuentaPorPagar
    {
        private CD_AbonoCuentaPorPagar objCapaDato = new CD_AbonoCuentaPorPagar();

        public List<AbonoCuentaPorPagar> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<AbonoCuentaPorPagar> ListarPorCuenta(string identificacionProveedor, string numeroDocumento)
        {
            identificacionProveedor = identificacionProveedor == null ? "" : identificacionProveedor.Trim();
            numeroDocumento = numeroDocumento == null ? "" : numeroDocumento.Trim();

            if (identificacionProveedor == "" || numeroDocumento == "")
            {
                return new List<AbonoCuentaPorPagar>();
            }

            if (identificacionProveedor.Length > 45 || numeroDocumento.Length > 45)
            {
                return new List<AbonoCuentaPorPagar>();
            }

            return objCapaDato.ListarPorCuenta(identificacionProveedor, numeroDocumento);
        }

        public AbonoCuentaPorPagar Obtener(string identificacionProveedor, string numeroDocumento, int numeroAbono)
        {
            identificacionProveedor = identificacionProveedor == null ? "" : identificacionProveedor.Trim();
            numeroDocumento = numeroDocumento == null ? "" : numeroDocumento.Trim();

            if (identificacionProveedor == "" || numeroDocumento == "" || numeroAbono <= 0)
            {
                return null;
            }

            if (identificacionProveedor.Length > 45 || numeroDocumento.Length > 45)
            {
                return null;
            }

            return objCapaDato.Obtener(identificacionProveedor, numeroDocumento, numeroAbono);
        }

        public int Registrar(AbonoCuentaPorPagar obj, out string Mensaje, out int NumeroAbonoGenerado)
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

        public bool Editar(AbonoCuentaPorPagar obj, out string Mensaje)
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

        public bool Inactivar(string identificacionProveedor, string numeroDocumento, int numeroAbono, out string Mensaje)
        {
            Mensaje = string.Empty;

            identificacionProveedor = identificacionProveedor == null ? "" : identificacionProveedor.Trim();
            numeroDocumento = numeroDocumento == null ? "" : numeroDocumento.Trim();

            if (identificacionProveedor == "" || numeroDocumento == "" || numeroAbono <= 0)
            {
                Mensaje = "Debe seleccionar un abono válido.";
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

            return objCapaDato.Inactivar(identificacionProveedor, numeroDocumento, numeroAbono, out Mensaje);
        }

        private void PrepararDatos(AbonoCuentaPorPagar obj)
        {
            obj.IdentificacionProveedor = obj.IdentificacionProveedor == null ? "" : obj.IdentificacionProveedor.Trim();
            obj.ProveedorNombre = obj.ProveedorNombre == null ? "" : obj.ProveedorNombre.Trim();

            obj.NumeroDocumento = obj.NumeroDocumento == null ? "" : obj.NumeroDocumento.Trim();

            obj.Observacion = obj.Observacion == null ? "" : obj.Observacion.Trim();

            obj.Concepto = obj.Concepto == null ? "" : obj.Concepto.Trim();
            obj.EstadoCuenta = obj.EstadoCuenta == null ? "" : obj.EstadoCuenta.Trim();

            if (obj.FechaAbono == DateTime.MinValue)
            {
                obj.FechaAbono = DateTime.Now;
            }

            if (obj.MontoAbono < 0)
            {
                obj.MontoAbono = 0;
            }

            if (obj.NumeroAbono < 0)
            {
                obj.NumeroAbono = 0;
            }
        }

        private string Validar(AbonoCuentaPorPagar obj, bool esEdicion)
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

            if (string.IsNullOrWhiteSpace(obj.NumeroDocumento))
            {
                return "Debe seleccionar un número de documento.";
            }

            if (obj.NumeroDocumento.Length > 45)
            {
                return "El número de documento no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.NumeroDocumento, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "El número de documento solo puede contener letras, números, puntos, guiones y guion bajo.";
            }

            if (esEdicion && obj.NumeroAbono <= 0)
            {
                return "Debe seleccionar un abono válido.";
            }

            if (obj.FechaAbono == DateTime.MinValue)
            {
                return "Debe ingresar la fecha del abono.";
            }

            if (obj.MontoAbono <= 0 && obj.Activo)
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

            return "";
        }
    }
}