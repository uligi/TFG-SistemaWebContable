using CapaDatos.Catalogos;
using CapaEntidad.Catalogos;
using System.Collections.Generic;

namespace CapaNegocio.Catalogos
{
    public class CN_TipoIngreso
    {
        private CD_TipoIngreso objCapaDato = new CD_TipoIngreso();

        public List<TipoIngreso> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<TipoIngreso> ListarActivos()
        {
            return objCapaDato.ListarActivos();
        }

        public int Registrar(TipoIngreso obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de ingreso es obligatorio.";
                return 0;
            }

            obj.Nombre = obj.Nombre.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del tipo de ingreso no puede superar los 45 caracteres.";
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

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(TipoIngreso obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdTipoIngreso <= 0)
            {
                Mensaje = "Debe seleccionar un tipo de ingreso válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de ingreso es obligatorio.";
                return false;
            }

            obj.Nombre = obj.Nombre.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del tipo de ingreso no puede superar los 45 caracteres.";
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

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idTipoIngreso, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idTipoIngreso <= 0)
            {
                Mensaje = "Debe seleccionar un tipo de ingreso válido.";
                return false;
            }

            return objCapaDato.Inactivar(idTipoIngreso, out Mensaje);
        }
    }
}