using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Contabilidad
{
    public class CN_ConfiguracionCuentaContable
    {
        private CD_ConfiguracionCuentaContable objCapaDato = new CD_ConfiguracionCuentaContable();

        public List<ConfiguracionCuentaContable> Listar()
        {
            return objCapaDato.Listar();
        }

        public ConfiguracionCuentaContable ObtenerPorOperacion(string codigoOperacion)
        {
            codigoOperacion = codigoOperacion == null ? "" : codigoOperacion.Trim().ToUpper();

            if (codigoOperacion == "")
            {
                return null;
            }

            if (codigoOperacion.Length > 45)
            {
                return null;
            }

            return objCapaDato.ObtenerPorOperacion(codigoOperacion);
        }

        public ConfiguracionCuentaContable ObtenerActivaPorOperacion(string codigoOperacion)
        {
            codigoOperacion = codigoOperacion == null ? "" : codigoOperacion.Trim().ToUpper();

            if (codigoOperacion == "")
            {
                return null;
            }

            if (codigoOperacion.Length > 45)
            {
                return null;
            }

            return objCapaDato.ObtenerActivaPorOperacion(codigoOperacion);
        }

        public int Registrar(ConfiguracionCuentaContable obj, out string Mensaje)
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

        public bool Editar(ConfiguracionCuentaContable obj, out string Mensaje)
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

        public bool Inactivar(string codigoOperacion, out string Mensaje)
        {
            Mensaje = string.Empty;

            codigoOperacion = codigoOperacion == null ? "" : codigoOperacion.Trim().ToUpper();

            if (codigoOperacion == "")
            {
                Mensaje = "Debe seleccionar una configuración válida.";
                return false;
            }

            if (codigoOperacion.Length > 45)
            {
                Mensaje = "El código de operación no puede superar los 45 caracteres.";
                return false;
            }

            if (!Regex.IsMatch(codigoOperacion, @"^[0-9A-Z_\-]+$"))
            {
                Mensaje = "El código de operación solo puede contener letras mayúsculas, números, guiones y guion bajo.";
                return false;
            }

            return objCapaDato.Inactivar(codigoOperacion, out Mensaje);
        }

        private void PrepararDatos(ConfiguracionCuentaContable obj)
        {
            obj.CodigoOperacion = obj.CodigoOperacion == null ? "" : obj.CodigoOperacion.Trim().ToUpper();
            obj.NombreOperacion = obj.NombreOperacion == null ? "" : obj.NombreOperacion.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();
            obj.CodigoCuenta = obj.CodigoCuenta == null ? "" : obj.CodigoCuenta.Trim();

            obj.NombreCuenta = obj.NombreCuenta == null ? "" : obj.NombreCuenta.Trim();
        }

        private string Validar(ConfiguracionCuentaContable obj, bool esEdicion)
        {
            if (string.IsNullOrWhiteSpace(obj.CodigoOperacion))
            {
                return esEdicion
                    ? "Debe seleccionar una configuración válida."
                    : "El código de operación es obligatorio.";
            }

            if (obj.CodigoOperacion.Length > 45)
            {
                return "El código de operación no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.CodigoOperacion, @"^[0-9A-Z_\-]+$"))
            {
                return "El código de operación solo puede contener letras mayúsculas, números, guiones y guion bajo.";
            }

            if (string.IsNullOrWhiteSpace(obj.NombreOperacion))
            {
                return "El nombre de operación es obligatorio.";
            }

            if (obj.NombreOperacion.Length > 100)
            {
                return "El nombre de operación no puede superar los 100 caracteres.";
            }

            if (!Regex.IsMatch(obj.NombreOperacion, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "El nombre de operación solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.Descripcion.Length > 200)
            {
                return "La descripción no puede superar los 200 caracteres.";
            }

            if (obj.Descripcion != "" &&
                !Regex.IsMatch(obj.Descripcion, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "La descripción solo puede contener letras, números, espacios y caracteres básicos.";
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

            if (obj.CodigoCuenta == "0")
            {
                return "No se puede usar la cuenta raíz como configuración contable.";
            }

            return "";
        }
    }
}