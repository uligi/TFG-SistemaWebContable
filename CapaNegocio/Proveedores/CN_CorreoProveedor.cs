using CapaDatos.Proveedores;
using CapaEntidad.Proveedores;
using System.Collections.Generic;

namespace CapaNegocio.Proveedores
{
    public class CN_CorreoProveedor
    {
        private CD_CorreoProveedor objCapaDato = new CD_CorreoProveedor();

        public List<CorreoProveedor> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(CorreoProveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.IdentificacionProveedor))
            {
                Mensaje = "Debe seleccionar un proveedor.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.DireccionCorreo))
            {
                Mensaje = "La dirección de correo es obligatoria.";
                return 0;
            }

            if (obj.DireccionCorreo.Length > 150)
            {
                Mensaje = "La dirección de correo no puede superar los 150 caracteres.";
                return 0;
            }

            if (obj.IdTipoCorreo == 0)
            {
                Mensaje = "Debe seleccionar un tipo de correo.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(string correoAnterior, CorreoProveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(correoAnterior))
            {
                Mensaje = "Debe seleccionar un correo válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.IdentificacionProveedor))
            {
                Mensaje = "Debe seleccionar un proveedor.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.DireccionCorreo))
            {
                Mensaje = "La dirección de correo es obligatoria.";
                return false;
            }

            if (obj.DireccionCorreo.Length > 150)
            {
                Mensaje = "La dirección de correo no puede superar los 150 caracteres.";
                return false;
            }

            if (obj.IdTipoCorreo == 0)
            {
                Mensaje = "Debe seleccionar un tipo de correo.";
                return false;
            }

            return objCapaDato.Editar(correoAnterior, obj, out Mensaje);
        }

        public bool Inactivar(string identificacionProveedor, string direccionCorreo, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionProveedor))
            {
                Mensaje = "Debe seleccionar un proveedor válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(direccionCorreo))
            {
                Mensaje = "Debe seleccionar un correo válido.";
                return false;
            }

            return objCapaDato.Inactivar(identificacionProveedor, direccionCorreo, out Mensaje);
        }
    }
}