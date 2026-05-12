using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos.Proveedores;
using CapaEntidad.Proveedores;


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

        public int Registrar(Proveedor obj, out string Mensaje, out string identificacionProveedorGenerada)
        {
            Mensaje = string.Empty;
            identificacionProveedorGenerada = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.RazonSocial))
            {
                Mensaje = "La razón social es obligatoria.";
                return 0;
            }

            if (obj.RazonSocial.Length > 200)
            {
                Mensaje = "La razón social no puede superar los 200 caracteres.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.NombreContacto) && obj.NombreContacto.Length > 100)
            {
                Mensaje = "El nombre de contacto no puede superar los 100 caracteres.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.DireccionExacta) && obj.DireccionExacta.Length > 250)
            {
                Mensaje = "La dirección exacta no puede superar los 250 caracteres.";
                return 0;
            }

            if (obj.DiasCredito.HasValue && obj.DiasCredito.Value < 0)
            {
                Mensaje = "Los días de crédito no pueden ser negativos.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje, out identificacionProveedorGenerada);
        }

        public bool Editar(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.IdentificacionProveedor))
            {
                Mensaje = "Debe seleccionar un proveedor válido.";
                return false;
            }

            if (obj.IdentificacionProveedor.Length > 45)
            {
                Mensaje = "La identificación del proveedor no puede superar los 45 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.RazonSocial))
            {
                Mensaje = "La razón social es obligatoria.";
                return false;
            }

            if (obj.RazonSocial.Length > 200)
            {
                Mensaje = "La razón social no puede superar los 200 caracteres.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.NombreContacto) && obj.NombreContacto.Length > 100)
            {
                Mensaje = "El nombre de contacto no puede superar los 100 caracteres.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.DireccionExacta) && obj.DireccionExacta.Length > 250)
            {
                Mensaje = "La dirección exacta no puede superar los 250 caracteres.";
                return false;
            }

            if (obj.DiasCredito.HasValue && obj.DiasCredito.Value < 0)
            {
                Mensaje = "Los días de crédito no pueden ser negativos.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(string identificacionProveedor, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(identificacionProveedor))
            {
                Mensaje = "Debe seleccionar un proveedor válido.";
                return false;
            }

            return objCapaDato.Inactivar(identificacionProveedor, out Mensaje);
        }
    }
}
