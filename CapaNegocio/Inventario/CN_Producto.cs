using CapaDatos.Inventario;
using CapaEntidad.Inventario;
using CapaEntidad.Catalogos;
using System.Collections.Generic;

namespace CapaNegocio.Inventario
{
    public class CN_Producto
    {
        private CD_Producto objCapaDato = new CD_Producto();

        public List<Producto> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<TipoProducto> ListarTipoProductosActivos()
        {
            return objCapaDato.ListarTipoProductosActivos();
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

          

            if (string.IsNullOrWhiteSpace(obj.NombreProducto))
            {
                Mensaje = "El nombre del producto es obligatorio.";
                return 0;
            }

            if (obj.NombreProducto.Length > 150)
            {
                Mensaje = "El nombre del producto no puede superar los 150 caracteres.";
                return 0;
            }

            if (obj.IdTipoProducto == 0)
            {
                Mensaje = "Debe seleccionar un tipo de producto.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.Descripcion) && obj.Descripcion.Length > 250)
            {
                Mensaje = "La descripción no puede superar los 250 caracteres.";
                return 0;
            }

            if (obj.IdImpuesto == 0)
            {
                Mensaje = "Debe seleccionar un impuesto.";
                return 0;
            }

            if (obj.PrecioVenta < 0)
            {
                Mensaje = "El precio de venta no puede ser negativo.";
                return 0;
            }

            if (obj.StockActual < 0)
            {
                Mensaje = "El stock actual no puede ser negativo.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdProducto == 0)
            {
                Mensaje = "Debe seleccionar un producto válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.CodigoProducto))
            {
                Mensaje = "El código del producto es obligatorio.";
                return false;
            }

            if (obj.CodigoProducto.Length > 45)
            {
                Mensaje = "El código del producto no puede superar los 45 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.NombreProducto))
            {
                Mensaje = "El nombre del producto es obligatorio.";
                return false;
            }

            if (obj.NombreProducto.Length > 150)
            {
                Mensaje = "El nombre del producto no puede superar los 150 caracteres.";
                return false;
            }

            if (obj.IdTipoProducto == 0)
            {
                Mensaje = "Debe seleccionar un tipo de producto.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.Descripcion) && obj.Descripcion.Length > 250)
            {
                Mensaje = "La descripción no puede superar los 250 caracteres.";
                return false;
            }
            if (obj.IdImpuesto == 0)
            {
                Mensaje = "Debe seleccionar un impuesto.";
                return false;
            }

            if (obj.PrecioVenta < 0)
            {
                Mensaje = "El precio de venta no puede ser negativo.";
                return false;
            }

            if (obj.StockActual < 0)
            {
                Mensaje = "El stock actual no puede ser negativo.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int id, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (id == 0)
            {
                Mensaje = "Debe seleccionar un producto válido.";
                return false;
            }

            return objCapaDato.Inactivar(id, out Mensaje);
        }
    }
}