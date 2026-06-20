using CapaDatos.Catalogos;
using CapaEntidad.Catalogos;
using System.Collections.Generic;

namespace CapaNegocio.Catalogos
{
    public class CN_TipoGasto
    {
        private CD_TipoGasto objCapaDato = new CD_TipoGasto();

        public List<TipoGasto> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<TipoGasto> ListarActivos()
        {
            return objCapaDato.ListarActivos();
        }

        public int Registrar(TipoGasto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de gasto es obligatorio.";
                return 0;
            }

            obj.Nombre = obj.Nombre.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del tipo de gasto no puede superar los 45 caracteres.";
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

        public bool Editar(TipoGasto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdTipoGasto <= 0)
            {
                Mensaje = "Debe seleccionar un tipo de gasto válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de gasto es obligatorio.";
                return false;
            }

            obj.Nombre = obj.Nombre.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del tipo de gasto no puede superar los 45 caracteres.";
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

        public bool Inactivar(int idTipoGasto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idTipoGasto <= 0)
            {
                Mensaje = "Debe seleccionar un tipo de gasto válido.";
                return false;
            }

            return objCapaDato.Inactivar(idTipoGasto, out Mensaje);
        }
    }
}