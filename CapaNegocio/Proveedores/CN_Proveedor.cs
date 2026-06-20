using CapaDatos.Proveedores;
using CapaEntidad.Proveedores;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Proveedores
{
    public class CN_Proveedor
    {
        private CD_Proveedor objCapaDato = new CD_Proveedor();

        public List<Proveedor> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<Proveedor> ListarActivos()
        {
            return objCapaDato.ListarActivos();
        }

        public int Registrar(Proveedor obj, out string Mensaje, out string IdentificacionProveedorGenerada)
        {
            Mensaje = string.Empty;
            IdentificacionProveedorGenerada = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje, out IdentificacionProveedorGenerada);
        }

        public bool Editar(Proveedor obj, out string Mensaje)
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

        public bool Inactivar(string identificacionProveedor, out string Mensaje)
        {
            Mensaje = string.Empty;

            identificacionProveedor = identificacionProveedor == null ? "" : identificacionProveedor.Trim();

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

            return objCapaDato.Inactivar(identificacionProveedor, out Mensaje);
        }

        private void PrepararDatos(Proveedor obj)
        {
            obj.IdentificacionProveedor = obj.IdentificacionProveedor == null ? "" : obj.IdentificacionProveedor.Trim();
            obj.RazonSocial = obj.RazonSocial == null ? "" : obj.RazonSocial.Trim();
            obj.NombreContacto = obj.NombreContacto == null ? "" : obj.NombreContacto.Trim();
            obj.DireccionExacta = obj.DireccionExacta == null ? "" : obj.DireccionExacta.Trim();

            obj.DistritoNombre = obj.DistritoNombre == null ? "" : obj.DistritoNombre.Trim();
            obj.CantonNombre = obj.CantonNombre == null ? "" : obj.CantonNombre.Trim();
            obj.ProvinciaNombre = obj.ProvinciaNombre == null ? "" : obj.ProvinciaNombre.Trim();

            if (obj.DiasCredito < 0)
            {
                obj.DiasCredito = 0;
            }
        }

        private string Validar(Proveedor obj, bool esEdicion)
        {
            if (esEdicion)
            {
                if (string.IsNullOrWhiteSpace(obj.IdentificacionProveedor))
                {
                    return "Debe seleccionar un proveedor válido.";
                }

                if (obj.IdentificacionProveedor.Length > 45)
                {
                    return "La identificación del proveedor no puede superar los 45 caracteres.";
                }
            }

            if (string.IsNullOrWhiteSpace(obj.RazonSocial))
            {
                return "La razón social es obligatoria.";
            }

            if (obj.RazonSocial.Length > 200)
            {
                return "La razón social no puede superar los 200 caracteres.";
            }

            if (!Regex.IsMatch(obj.RazonSocial, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "La razón social solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (string.IsNullOrWhiteSpace(obj.NombreContacto))
            {
                return "El nombre de contacto es obligatorio.";
            }

            if (obj.NombreContacto.Length > 100)
            {
                return "El nombre de contacto no puede superar los 100 caracteres.";
            }

            if (!Regex.IsMatch(obj.NombreContacto, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"))
            {
                return "El nombre de contacto solo puede contener letras y espacios.";
            }

            if (obj.CodigoDistrito <= 0)
            {
                return "Debe seleccionar un distrito.";
            }

            if (string.IsNullOrWhiteSpace(obj.DireccionExacta))
            {
                return "La dirección exacta es obligatoria.";
            }

            if (obj.DireccionExacta.Length > 250)
            {
                return "La dirección exacta no puede superar los 250 caracteres.";
            }

            if (!Regex.IsMatch(obj.DireccionExacta, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "La dirección exacta solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.DiasCredito < 0)
            {
                return "Los días de crédito no pueden ser negativos.";
            }

            return "";
        }
    }
}