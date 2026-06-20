using CapaDatos.Proveedores;
using CapaEntidad.Proveedores;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Proveedores
{
    public class CN_CorreoProveedor
    {
        private CD_CorreoProveedor objCapaDato = new CD_CorreoProveedor();

        public List<CorreoProveedor> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<CorreoProveedor> ListarPorProveedor(string identificacionProveedor)
        {
            identificacionProveedor = identificacionProveedor == null ? "" : identificacionProveedor.Trim();

            if (identificacionProveedor == "")
            {
                return new List<CorreoProveedor>();
            }

            return objCapaDato.ListarPorProveedor(identificacionProveedor);
        }

        public int Registrar(CorreoProveedor obj, out string Mensaje)
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

        public bool Editar(CorreoProveedor obj, out string Mensaje)
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

        public bool Inactivar(string identificacionProveedor, string direccionCorreo, out string Mensaje)
        {
            Mensaje = string.Empty;

            identificacionProveedor = identificacionProveedor == null ? "" : identificacionProveedor.Trim();
            direccionCorreo = direccionCorreo == null ? "" : direccionCorreo.Trim().ToLower();

            if (identificacionProveedor == "")
            {
                Mensaje = "Debe seleccionar un proveedor válido.";
                return false;
            }

            if (identificacionProveedor.Length > 45)
            {
                Mensaje = "La identificación del proveedor no puede superar los 45 caracteres.";
                return false;
            }

            if (direccionCorreo == "")
            {
                Mensaje = "Debe seleccionar un correo válido.";
                return false;
            }

            if (direccionCorreo.Length > 150)
            {
                Mensaje = "La dirección de correo no puede superar los 150 caracteres.";
                return false;
            }

            if (!ValidarCorreo(direccionCorreo))
            {
                Mensaje = "Debe ingresar una dirección de correo válida.";
                return false;
            }

            return objCapaDato.Inactivar(identificacionProveedor, direccionCorreo, out Mensaje);
        }

        private void PrepararDatos(CorreoProveedor obj)
        {
            obj.IdentificacionProveedor = obj.IdentificacionProveedor == null ? "" : obj.IdentificacionProveedor.Trim();
            obj.RazonSocial = obj.RazonSocial == null ? "" : obj.RazonSocial.Trim();

            obj.DireccionCorreo = obj.DireccionCorreo == null ? "" : obj.DireccionCorreo.Trim().ToLower();
            obj.DireccionCorreoAnterior = obj.DireccionCorreoAnterior == null ? "" : obj.DireccionCorreoAnterior.Trim().ToLower();

            obj.TipoCorreoNombre = obj.TipoCorreoNombre == null ? "" : obj.TipoCorreoNombre.Trim();
        }

        private string Validar(CorreoProveedor obj, bool esEdicion)
        {
            if (string.IsNullOrWhiteSpace(obj.IdentificacionProveedor))
            {
                return "Debe seleccionar un proveedor.";
            }

            if (obj.IdentificacionProveedor.Length > 45)
            {
                return "La identificación del proveedor no puede superar los 45 caracteres.";
            }

            if (esEdicion)
            {
                if (string.IsNullOrWhiteSpace(obj.DireccionCorreoAnterior))
                {
                    return "Debe seleccionar un correo válido.";
                }

                if (obj.DireccionCorreoAnterior.Length > 150)
                {
                    return "El correo anterior no puede superar los 150 caracteres.";
                }

                if (!ValidarCorreo(obj.DireccionCorreoAnterior))
                {
                    return "El correo anterior no tiene un formato válido.";
                }
            }

            if (string.IsNullOrWhiteSpace(obj.DireccionCorreo))
            {
                return "La dirección de correo es obligatoria.";
            }

            if (obj.DireccionCorreo.Length > 150)
            {
                return "La dirección de correo no puede superar los 150 caracteres.";
            }

            if (!ValidarCorreo(obj.DireccionCorreo))
            {
                return "Debe ingresar una dirección de correo válida.";
            }

            if (obj.IdTipoCorreo <= 0)
            {
                return "Debe seleccionar un tipo de correo.";
            }

            return "";
        }

        private bool ValidarCorreo(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                return false;
            }

            return Regex.IsMatch(
                correo,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
            );
        }
    }
}