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

        public int Registrar(TipoProducto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.TipoProductoNombre))
            {
                Mensaje = "El nombre del tipo de producto es obligatorio.";
                return 0;
            }

            if (obj.TipoProductoNombre.Length > 100)
            {
                Mensaje = "El nombre del tipo de producto no puede superar los 100 caracteres.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.Descripcion) && obj.Descripcion.Length > 150)
            {
                Mensaje = "La descripción no puede superar los 150 caracteres.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(TipoProducto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdTipoProducto == 0)
            {
                Mensaje = "Debe seleccionar un tipo de producto válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.TipoProductoNombre))
            {
                Mensaje = "El nombre del tipo de producto es obligatorio.";
                return false;
            }

            if (obj.TipoProductoNombre.Length > 100)
            {
                Mensaje = "El nombre del tipo de producto no puede superar los 100 caracteres.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.Descripcion) && obj.Descripcion.Length > 150)
            {
                Mensaje = "La descripción no puede superar los 150 caracteres.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int id, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (id == 0)
            {
                Mensaje = "Debe seleccionar un tipo de producto válido.";
                return false;
            }

            return objCapaDato.Inactivar(id, out Mensaje);
        }
    }
}