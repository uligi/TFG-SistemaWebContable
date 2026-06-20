using CapaDatos.Catalogos;
using CapaEntidad.Catalogos;
using System.Collections.Generic;

namespace CapaNegocio.Catalogos
{
    public class CN_TipoProducto
    {
        private CD_TipoProducto objCapaDato = new CD_TipoProducto();

        public List<TipoProducto> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<TipoProducto> ListarActivos()
        {
            return objCapaDato.ListarActivos();
        }

        public int Registrar(TipoProducto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de producto es obligatorio.";
                return 0;
            }

            obj.Nombre = obj.Nombre.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            if (obj.Nombre.Length > 100)
            {
                Mensaje = "El nombre del tipo de producto no puede superar los 100 caracteres.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción es obligatoria.";
                return 0;
            }

            if (obj.Descripcion.Length > 150)
            {
                Mensaje = "La descripción no puede superar los 150 caracteres.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(TipoProducto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdTipoProducto <= 0)
            {
                Mensaje = "Debe seleccionar un tipo de producto válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de producto es obligatorio.";
                return false;
            }

            obj.Nombre = obj.Nombre.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            if (obj.Nombre.Length > 100)
            {
                Mensaje = "El nombre del tipo de producto no puede superar los 100 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción es obligatoria.";
                return false;
            }

            if (obj.Descripcion.Length > 150)
            {
                Mensaje = "La descripción no puede superar los 150 caracteres.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idTipoProducto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idTipoProducto <= 0)
            {
                Mensaje = "Debe seleccionar un tipo de producto válido.";
                return false;
            }

            return objCapaDato.Inactivar(idTipoProducto, out Mensaje);
        }
    }
}