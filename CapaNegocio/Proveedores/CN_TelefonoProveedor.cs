using CapaDatos.Proveedores;
using CapaEntidad.Proveedores;
using System.Collections.Generic;

namespace CapaNegocio.Proveedores
{
    public class CN_TelefonoProveedor
    {
        private CD_TelefonoProveedor objCapaDato = new CD_TelefonoProveedor();

        public List<TelefonoProveedor> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(TelefonoProveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.IdentificacionProveedor))
            {
                Mensaje = "Debe seleccionar un proveedor.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.NumeroTelefono))
            {
                Mensaje = "El número de teléfono es obligatorio.";
                return 0;
            }

            if (obj.NumeroTelefono.Length > 45)
            {
                Mensaje = "El número de teléfono no puede superar los 45 caracteres.";
                return 0;
            }

            if (obj.IdTipoTelefono == 0)
            {
                Mensaje = "Debe seleccionar un tipo de teléfono.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(string numeroAnterior, TelefonoProveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(numeroAnterior))
            {
                Mensaje = "Debe seleccionar un teléfono válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.IdentificacionProveedor))
            {
                Mensaje = "Debe seleccionar un proveedor.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.NumeroTelefono))
            {
                Mensaje = "El número de teléfono es obligatorio.";
                return false;
            }

            if (obj.NumeroTelefono.Length > 45)
            {
                Mensaje = "El número de teléfono no puede superar los 45 caracteres.";
                return false;
            }

            if (obj.IdTipoTelefono == 0)
            {
                Mensaje = "Debe seleccionar un tipo de teléfono.";
                return false;
            }

            return objCapaDato.Editar(numeroAnterior, obj, out Mensaje);
        }

        public bool Inactivar(string identificacionProveedor, string numeroTelefono, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionProveedor))
            {
                Mensaje = "Debe seleccionar un proveedor válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(numeroTelefono))
            {
                Mensaje = "Debe seleccionar un teléfono válido.";
                return false;
            }

            return objCapaDato.Inactivar(identificacionProveedor, numeroTelefono, out Mensaje);
        }
    }
}