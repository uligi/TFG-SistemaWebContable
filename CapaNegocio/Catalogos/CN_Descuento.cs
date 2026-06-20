using CapaDatos.Catalogos;
using CapaEntidad.Catalogos;
using System.Collections.Generic;

namespace CapaNegocio.Catalogos
{
    public class CN_Descuento
    {
        private CD_Descuento objCapaDato = new CD_Descuento();

        public List<Descuento> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<Descuento> ListarActivos()
        {
            return objCapaDato.ListarActivos();
        }

        public int Registrar(Descuento obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del descuento es obligatorio.";
                return 0;
            }

            obj.Nombre = obj.Nombre.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            if (obj.Nombre.Length > 100)
            {
                Mensaje = "El nombre del descuento no puede superar los 100 caracteres.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción es obligatoria.";
                return 0;
            }

            if (obj.Descripcion.Length > 255)
            {
                Mensaje = "La descripción no puede superar los 255 caracteres.";
                return 0;
            }

            if (obj.Porcentaje < 0 || obj.Porcentaje > 100)
            {
                Mensaje = "El porcentaje del descuento debe estar entre 0 y 100.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Descuento obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdDescuento <= 0)
            {
                Mensaje = "Debe seleccionar un descuento válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del descuento es obligatorio.";
                return false;
            }

            obj.Nombre = obj.Nombre.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            if (obj.Nombre.Length > 100)
            {
                Mensaje = "El nombre del descuento no puede superar los 100 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción es obligatoria.";
                return false;
            }

            if (obj.Descripcion.Length > 255)
            {
                Mensaje = "La descripción no puede superar los 255 caracteres.";
                return false;
            }

            if (obj.Porcentaje < 0 || obj.Porcentaje > 100)
            {
                Mensaje = "El porcentaje del descuento debe estar entre 0 y 100.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idDescuento, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idDescuento <= 0)
            {
                Mensaje = "Debe seleccionar un descuento válido.";
                return false;
            }

            return objCapaDato.Inactivar(idDescuento, out Mensaje);
        }
    }
}