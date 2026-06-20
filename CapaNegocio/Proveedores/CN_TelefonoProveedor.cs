using CapaDatos.Proveedores;
using CapaEntidad.Proveedores;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Proveedores
{
    public class CN_TelefonoProveedor
    {
        private CD_TelefonoProveedor objCapaDato = new CD_TelefonoProveedor();

        public List<TelefonoProveedor> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<TelefonoProveedor> ListarPorProveedor(string identificacionProveedor)
        {
            identificacionProveedor = identificacionProveedor == null ? "" : identificacionProveedor.Trim();

            if (identificacionProveedor == "")
            {
                return new List<TelefonoProveedor>();
            }

            return objCapaDato.ListarPorProveedor(identificacionProveedor);
        }

        public int Registrar(TelefonoProveedor obj, out string Mensaje)
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

        public bool Editar(TelefonoProveedor obj, out string Mensaje)
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

        public bool Inactivar(string identificacionProveedor, string numeroTelefono, out string Mensaje)
        {
            Mensaje = string.Empty;

            identificacionProveedor = identificacionProveedor == null ? "" : identificacionProveedor.Trim();
            numeroTelefono = numeroTelefono == null ? "" : numeroTelefono.Trim();

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

            if (numeroTelefono == "")
            {
                Mensaje = "Debe seleccionar un teléfono válido.";
                return false;
            }

            if (!Regex.IsMatch(numeroTelefono, @"^[0-9]{8,15}$"))
            {
                Mensaje = "El teléfono debe contener solo números y tener entre 8 y 15 dígitos.";
                return false;
            }

            return objCapaDato.Inactivar(identificacionProveedor, numeroTelefono, out Mensaje);
        }

        private void PrepararDatos(TelefonoProveedor obj)
        {
            obj.IdentificacionProveedor = obj.IdentificacionProveedor == null ? "" : obj.IdentificacionProveedor.Trim();
            obj.RazonSocial = obj.RazonSocial == null ? "" : obj.RazonSocial.Trim();

            obj.NumeroTelefono = obj.NumeroTelefono == null ? "" : obj.NumeroTelefono.Trim();
            obj.NumeroTelefonoAnterior = obj.NumeroTelefonoAnterior == null ? "" : obj.NumeroTelefonoAnterior.Trim();

            obj.TipoTelefonoNombre = obj.TipoTelefonoNombre == null ? "" : obj.TipoTelefonoNombre.Trim();
        }

        private string Validar(TelefonoProveedor obj, bool esEdicion)
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
                if (string.IsNullOrWhiteSpace(obj.NumeroTelefonoAnterior))
                {
                    return "Debe seleccionar un teléfono válido.";
                }

                if (!Regex.IsMatch(obj.NumeroTelefonoAnterior, @"^[0-9]{8,15}$"))
                {
                    return "El teléfono anterior debe contener solo números y tener entre 8 y 15 dígitos.";
                }
            }

            if (string.IsNullOrWhiteSpace(obj.NumeroTelefono))
            {
                return "El número de teléfono es obligatorio.";
            }

            if (!Regex.IsMatch(obj.NumeroTelefono, @"^[0-9]{8,15}$"))
            {
                return "El teléfono debe contener solo números y tener entre 8 y 15 dígitos.";
            }

            if (obj.IdTipoTelefono <= 0)
            {
                return "Debe seleccionar un tipo de teléfono.";
            }

            return "";
        }
    }
}