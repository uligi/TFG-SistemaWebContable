using CapaDatos.Facturacion;
using CapaEntidad.Facturacion;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Facturacion
{
    public class CN_DetalleFactura
    {
        private CD_DetalleFactura objCapaDato = new CD_DetalleFactura();

        public List<DetalleFactura> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<DetalleFactura> ListarPorFactura(string numeroFactura)
        {
            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();

            if (numeroFactura == "")
            {
                return new List<DetalleFactura>();
            }

            if (numeroFactura.Length > 45)
            {
                return new List<DetalleFactura>();
            }

            return objCapaDato.ListarPorFactura(numeroFactura);
        }

        public DetalleFactura Obtener(string numeroFactura, string codigoProducto)
        {
            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();
            codigoProducto = codigoProducto == null ? "" : codigoProducto.Trim();

            if (numeroFactura == "")
            {
                return null;
            }

            if (codigoProducto == "")
            {
                return null;
            }

            if (numeroFactura.Length > 45 || codigoProducto.Length > 45)
            {
                return null;
            }

            return objCapaDato.Obtener(numeroFactura, codigoProducto);
        }

        public int Registrar(DetalleFactura obj, out string Mensaje)
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

        public bool Editar(DetalleFactura obj, out string Mensaje)
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

        public bool Inactivar(string numeroFactura, string codigoProducto, out string Mensaje)
        {
            Mensaje = string.Empty;

            numeroFactura = numeroFactura == null ? "" : numeroFactura.Trim();
            codigoProducto = codigoProducto == null ? "" : codigoProducto.Trim();

            if (numeroFactura == "")
            {
                Mensaje = "Debe seleccionar una factura válida.";
                return false;
            }

            if (numeroFactura.Length > 45)
            {
                Mensaje = "El número de factura no puede superar los 45 caracteres.";
                return false;
            }

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

            return objCapaDato.Inactivar(numeroFactura, codigoProducto, out Mensaje);
        }

        private void PrepararDatos(DetalleFactura obj)
        {
            obj.NumeroFactura = obj.NumeroFactura == null ? "" : obj.NumeroFactura.Trim();

            obj.CodigoProducto = obj.CodigoProducto == null ? "" : obj.CodigoProducto.Trim();
            obj.NombreProducto = obj.NombreProducto == null ? "" : obj.NombreProducto.Trim();

            obj.ImpuestoNombre = obj.ImpuestoNombre == null ? "" : obj.ImpuestoNombre.Trim();
            obj.DescuentoNombre = obj.DescuentoNombre == null ? "" : obj.DescuentoNombre.Trim();

            obj.DescripcionItem = obj.DescripcionItem == null ? "" : obj.DescripcionItem.Trim();

            if (obj.PorcentajeImpuesto < 0)
            {
                obj.PorcentajeImpuesto = 0;
            }

            if (obj.PorcentajeDescuento < 0)
            {
                obj.PorcentajeDescuento = 0;
            }
        }

        private string Validar(DetalleFactura obj, bool esEdicion)
        {
            if (string.IsNullOrWhiteSpace(obj.NumeroFactura))
            {
                return "Debe seleccionar una factura válida.";
            }

            if (obj.NumeroFactura.Length > 45)
            {
                return "El número de factura no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.NumeroFactura, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "El número de factura solo puede contener letras, números, puntos, guiones y guion bajo.";
            }

            if (string.IsNullOrWhiteSpace(obj.CodigoProducto))
            {
                return "Debe seleccionar un producto.";
            }

            if (obj.CodigoProducto.Length > 45)
            {
                return "El código del producto no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.CodigoProducto, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "El código del producto solo puede contener letras, números, puntos, guiones y guion bajo.";
            }

            if (obj.IdImpuesto <= 0)
            {
                return "Debe seleccionar un impuesto.";
            }

            if (obj.IdDescuento <= 0)
            {
                return "Debe seleccionar un descuento.";
            }

            if (obj.DescripcionItem.Length > 150)
            {
                return "La descripción del item no puede superar los 150 caracteres.";
            }

            if (obj.DescripcionItem != "" &&
                !Regex.IsMatch(obj.DescripcionItem, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "La descripción del item solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.Cantidad <= 0)
            {
                return "La cantidad debe ser mayor a 0.";
            }

            if (obj.PrecioUnitario < 0)
            {
                return "El precio unitario no puede ser negativo.";
            }

            if (obj.PorcentajeImpuesto < 0 || obj.PorcentajeImpuesto > 100)
            {
                return "El porcentaje de impuesto debe estar entre 0 y 100.";
            }

            if (obj.PorcentajeDescuento < 0 || obj.PorcentajeDescuento > 100)
            {
                return "El porcentaje de descuento debe estar entre 0 y 100.";
            }

            if (obj.SubtotalLinea < 0)
            {
                return "El subtotal de la línea no puede ser negativo.";
            }

            if (obj.TotalLinea < 0)
            {
                return "El total de la línea no puede ser negativo.";
            }

            return "";
        }
    }
}