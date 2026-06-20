using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Contabilidad
{
    public class CN_CuentaContable
    {
        private CD_CuentaContable objCapaDato = new CD_CuentaContable();

        public List<CuentaContable> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<CuentaContable> ListarActivas()
        {
            return objCapaDato.ListarActivas();
        }

        public List<CuentaContable> ListarParaMovimientos()
        {
            return objCapaDato.ListarParaMovimientos();
        }

        public CuentaContable Obtener(string codigoCuenta)
        {
            codigoCuenta = codigoCuenta == null ? "" : codigoCuenta.Trim();

            if (codigoCuenta == "")
            {
                return null;
            }

            if (codigoCuenta.Length > 45)
            {
                return null;
            }

            return objCapaDato.Obtener(codigoCuenta);
        }

        public int Registrar(CuentaContable obj, out string Mensaje)
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

        public bool Editar(CuentaContable obj, out string Mensaje)
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

        public bool Inactivar(string codigoCuenta, out string Mensaje)
        {
            Mensaje = string.Empty;

            codigoCuenta = codigoCuenta == null ? "" : codigoCuenta.Trim();

            if (codigoCuenta == "")
            {
                Mensaje = "Debe seleccionar una cuenta contable válida.";
                return false;
            }

            if (codigoCuenta.Length > 45)
            {
                Mensaje = "El código de cuenta no puede superar los 45 caracteres.";
                return false;
            }

            if (codigoCuenta == "0")
            {
                Mensaje = "No se puede inactivar la cuenta raíz del sistema.";
                return false;
            }

            return objCapaDato.Inactivar(codigoCuenta, out Mensaje);
        }

        public bool GenerarCodigo(string codigoCuentaPadre, out string CodigoGenerado, out string Mensaje)
        {
            CodigoGenerado = string.Empty;
            Mensaje = string.Empty;

            codigoCuentaPadre = codigoCuentaPadre == null ? "" : codigoCuentaPadre.Trim();

            if (codigoCuentaPadre == "")
            {
                codigoCuentaPadre = "0";
            }

            if (codigoCuentaPadre.Length > 45)
            {
                Mensaje = "El código de cuenta padre no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.GenerarCodigo(codigoCuentaPadre, out CodigoGenerado, out Mensaje);
        }

        private void PrepararDatos(CuentaContable obj)
        {
            obj.CodigoCuenta = obj.CodigoCuenta == null ? "" : obj.CodigoCuenta.Trim();
            obj.NombreCuenta = obj.NombreCuenta == null ? "" : obj.NombreCuenta.Trim();
            obj.TipoCuentaContableNombre = obj.TipoCuentaContableNombre == null ? "" : obj.TipoCuentaContableNombre.Trim();
            obj.NaturalezaCuentaContableNombre = obj.NaturalezaCuentaContableNombre == null ? "" : obj.NaturalezaCuentaContableNombre.Trim();
            obj.CodigoCuentaPadre = obj.CodigoCuentaPadre == null ? "" : obj.CodigoCuentaPadre.Trim();
            obj.NombreCuentaPadre = obj.NombreCuentaPadre == null ? "" : obj.NombreCuentaPadre.Trim();

            if (obj.CodigoCuentaPadre == "")
            {
                obj.CodigoCuentaPadre = "0";
            }
        }

        private string Validar(CuentaContable obj, bool esEdicion)
        {
            if (string.IsNullOrWhiteSpace(obj.CodigoCuenta))
            {
                return esEdicion
                    ? "Debe seleccionar una cuenta contable válida."
                    : "El código de cuenta es obligatorio.";
            }

            if (obj.CodigoCuenta.Length > 45)
            {
                return "El código de cuenta no puede superar los 45 caracteres.";
            }

            if (obj.CodigoCuenta == "0")
            {
                return esEdicion
                    ? "No se puede editar la cuenta raíz del sistema."
                    : "No se puede registrar otra cuenta con el código raíz 0.";
            }

            if (!Regex.IsMatch(obj.CodigoCuenta, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "El código de cuenta solo puede contener letras, números, puntos, guiones y guion bajo.";
            }

            if (string.IsNullOrWhiteSpace(obj.NombreCuenta))
            {
                return "El nombre de la cuenta es obligatorio.";
            }

            if (obj.NombreCuenta.Length > 100)
            {
                return "El nombre de la cuenta no puede superar los 100 caracteres.";
            }

            if (!Regex.IsMatch(obj.NombreCuenta, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "El nombre de la cuenta solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.IdTipoCuentaContable <= 0)
            {
                return "Debe seleccionar un tipo de cuenta contable.";
            }

            if (obj.IdNaturalezaCuentaContable <= 0)
            {
                return "Debe seleccionar una naturaleza de cuenta contable.";
            }

            if (string.IsNullOrWhiteSpace(obj.CodigoCuentaPadre))
            {
                obj.CodigoCuentaPadre = "0";
            }

            if (obj.CodigoCuentaPadre.Length > 45)
            {
                return "El código de cuenta padre no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.CodigoCuentaPadre, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "El código de cuenta padre solo puede contener letras, números, puntos, guiones y guion bajo.";
            }

            if (obj.CodigoCuentaPadre == obj.CodigoCuenta)
            {
                return "Una cuenta no puede ser su propia cuenta padre.";
            }

            return "";
        }
    }
}