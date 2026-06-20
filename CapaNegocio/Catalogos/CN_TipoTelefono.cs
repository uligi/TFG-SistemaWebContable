using CapaDatos.Catalogos;
using CapaEntidad.Catalogos;
using System.Collections.Generic;

namespace CapaNegocio.Catalogos
{
    public class CN_TipoTelefono
    {
        private CD_TipoTelefono objCapaDato = new CD_TipoTelefono();

        public List<TipoTelefono> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(TipoTelefono obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de teléfono es obligatorio.";
                return 0;
            }

            obj.Nombre = obj.Nombre.Trim();

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del tipo de teléfono no puede superar los 45 caracteres.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(TipoTelefono obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdTipoTelefono <= 0)
            {
                Mensaje = "Debe seleccionar un tipo de teléfono válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de teléfono es obligatorio.";
                return false;
            }

            obj.Nombre = obj.Nombre.Trim();

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del tipo de teléfono no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idTipoTelefono, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idTipoTelefono <= 0)
            {
                Mensaje = "Debe seleccionar un tipo de teléfono válido.";
                return false;
            }

            return objCapaDato.Inactivar(idTipoTelefono, out Mensaje);
        }
    }
}