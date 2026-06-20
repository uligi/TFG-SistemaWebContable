using CapaDatos.Inventario;
using CapaEntidad.Inventario;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Inventario
{
    public class CN_Producto
    {
        private CD_Producto objCapaDato = new CD_Producto();

        public List<Producto> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<Producto> ListarActivos()
        {
            return objCapaDato.ListarActivos();
        }

        public Producto Obtener(string codigoProducto)
        {
            codigoProducto = codigoProducto == null ? "" : codigoProducto.Trim();

            if (codigoProducto == "")
            {
                return null;
            }

            if (codigoProducto.Length > 45)
            {
                return null;
            }

            return objCapaDato.Obtener(codigoProducto);
        }

        public int Registrar(Producto obj, out string Mensaje, out string CodigoProductoGenerado)
        {
            Mensaje = string.Empty;
            CodigoProductoGenerado = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje, out CodigoProductoGenerado);
        }

        public bool Editar(Producto obj, out string Mensaje)
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

        public bool AjustarStock(string codigoProducto, decimal cantidad, string tipoMovimiento, out string Mensaje)
        {
            Mensaje = string.Empty;

            codigoProducto = codigoProducto == null ? "" : codigoProducto.Trim();
            tipoMovimiento = tipoMovimiento == null ? "" : tipoMovimiento.Trim().ToUpper();

            if (codigoProducto == "")
            {
                Mensaje = "Debe seleccionar un producto válido.";
                return false;
            }

            if (codigoProducto.Length > 45)
            {
                Mensaje = "El código del producto no puede superar los 45 caracteres.";
                return false;
            }

            if (cantidad <= 0)
            {
                Mensaje = "La cantidad debe ser mayor a 0.";
                return false;
            }

            if (tipoMovimiento != "SUMAR" && tipoMovimiento != "RESTAR")
            {
                Mensaje = "El tipo de movimiento debe ser SUMAR o RESTAR.";
                return false;
            }

            return objCapaDato.AjustarStock(codigoProducto, cantidad, tipoMovimiento, out Mensaje);
        }

        public bool Inactivar(string codigoProducto, out string Mensaje)
        {
            Mensaje = string.Empty;

            codigoProducto = codigoProducto == null ? "" : codigoProducto.Trim();

            if (codigoProducto == "")
            {
                Mensaje = "Debe seleccionar un producto válido.";
                return false;
            }

            if (codigoProducto.Length > 45)
            {
                Mensaje = "El código del producto no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.Inactivar(codigoProducto, out Mensaje);
        }

        private void PrepararDatos(Producto obj)
        {
            obj.CodigoProducto = obj.CodigoProducto == null ? "" : obj.CodigoProducto.Trim();
            obj.NombreProducto = obj.NombreProducto == null ? "" : obj.NombreProducto.Trim();
            obj.TipoProductoNombre = obj.TipoProductoNombre == null ? "" : obj.TipoProductoNombre.Trim();
            obj.ImpuestoNombre = obj.ImpuestoNombre == null ? "" : obj.ImpuestoNombre.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();
        }

        private string Validar(Producto obj, bool esEdicion)
        {
            if (esEdicion)
            {
                if (string.IsNullOrWhiteSpace(obj.CodigoProducto))
                {
                    return "Debe seleccionar un producto válido.";
                }

                if (obj.CodigoProducto.Length > 45)
                {
                    return "El código del producto no puede superar los 45 caracteres.";
                }

                if (!Regex.IsMatch(obj.CodigoProducto, @"^[0-9A-Za-z.\-_]+$"))
                {
                    return "El código del producto solo puede contener letras, números, puntos, guiones y guion bajo.";
                }
            }

            if (string.IsNullOrWhiteSpace(obj.NombreProducto))
            {
                return "El nombre del producto es obligatorio.";
            }

            if (obj.NombreProducto.Length > 150)
            {
                return "El nombre del producto no puede superar los 150 caracteres.";
            }

            if (!Regex.IsMatch(obj.NombreProducto, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "El nombre del producto solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.IdTipoProducto <= 0)
            {
                return "Debe seleccionar un tipo de producto.";
            }

            if (obj.IdImpuesto <= 0)
            {
                return "Debe seleccionar un impuesto.";
            }

            if (obj.Descripcion.Length > 250)
            {
                return "La descripción no puede superar los 250 caracteres.";
            }

            if (obj.Descripcion != "" &&
                !Regex.IsMatch(obj.Descripcion, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "La descripción solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.PrecioVenta < 0)
            {
                return "El precio de venta no puede ser negativo.";
            }

            if (obj.StockActual < 0)
            {
                return "El stock actual no puede ser negativo.";
            }

            return "";
        }
    }
}