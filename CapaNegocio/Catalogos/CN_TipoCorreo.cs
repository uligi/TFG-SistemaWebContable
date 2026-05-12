using CapaDatos.Catalogos;
using CapaEntidad.Catalogos;
using System.Collections.Generic;

namespace CapaNegocio.Catalogos
{
    public class CN_TipoCorreo
    {
        private CD_TipoCorreo objCapaDato = new CD_TipoCorreo();

        public List<TipoCorreo> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(TipoCorreo obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de correo es obligatorio.";
                return 0;
            }

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del tipo de correo no puede superar los 45 caracteres.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(TipoCorreo obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdTipoCorreo == 0)
            {
                Mensaje = "Debe seleccionar un tipo de correo válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de correo es obligatorio.";
                return false;
            }

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del tipo de correo no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int id, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (id == 0)
            {
                Mensaje = "Debe seleccionar un tipo de correo válido.";
                return false;
            }

            return objCapaDato.Inactivar(id, out Mensaje);
        }
    }
}