using CapaDatos.Facturacion;
using CapaEntidad.Facturacion;
using System.Collections.Generic;

namespace CapaNegocio.Facturacion
{
    public class CN_DetalleFactura
    {
        private CD_DetalleFactura objCapaDato = new CD_DetalleFactura();

        public List<DetalleFactura> ListarPorFactura(int idFactura)
        {
            return objCapaDato.ListarPorFactura(idFactura);
        }

        public int Registrar(DetalleFactura obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdFactura == 0)
            {
                Mensaje = "Debe seleccionar una factura.";
                return 0;
            }

            if (obj.IdProducto == 0)
            {
                Mensaje = "Debe seleccionar un producto.";
                return 0;
            }

            if (obj.Cantidad <= 0)
            {
                Mensaje = "La cantidad debe ser mayor a cero.";
                return 0;
            }

            if (obj.PrecioUnitario < 0)
            {
                Mensaje = "El precio unitario no puede ser negativo.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.DescripcionItem) && obj.DescripcionItem.Length > 150)
            {
                Mensaje = "La descripción del ítem no puede superar los 150 caracteres.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.DescripcionItem))
            {
                obj.DescripcionItem = obj.DescripcionItem.Trim();
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(DetalleFactura obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdDetalleFactura == 0)
            {
                Mensaje = "Debe seleccionar un detalle válido.";
                return false;
            }

            if (obj.IdProducto == 0)
            {
                Mensaje = "Debe seleccionar un producto.";
                return false;
            }

            if (obj.Activo && obj.Cantidad <= 0)
            {
                Mensaje = "La cantidad debe ser mayor a cero.";
                return false;
            }

            if (obj.PrecioUnitario < 0)
            {
                Mensaje = "El precio unitario no puede ser negativo.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.DescripcionItem) && obj.DescripcionItem.Length > 150)
            {
                Mensaje = "La descripción del ítem no puede superar los 150 caracteres.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.DescripcionItem))
            {
                obj.DescripcionItem = obj.DescripcionItem.Trim();
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idDetalleFactura, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idDetalleFactura == 0)
            {
                Mensaje = "Debe seleccionar un detalle válido.";
                return false;
            }

            return objCapaDato.Inactivar(idDetalleFactura, out Mensaje);
        }
    }
}