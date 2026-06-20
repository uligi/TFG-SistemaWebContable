using CapaDatos.Catalogos;
using CapaEntidad.Catalogos;
using System.Collections.Generic;

namespace CapaNegocio.Catalogos
{
    public class CN_TipoDeObservacion
    {
        private CD_TipoDeObservacion objCapaDato = new CD_TipoDeObservacion();

        public List<TipoDeObservacion> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(TipoDeObservacion obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Tipo_Observacion))
            {
                Mensaje = "El nombre del tipo de observación es obligatorio.";
                return 0;
            }

            obj.Tipo_Observacion = obj.Tipo_Observacion.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            if (obj.Tipo_Observacion.Length > 45)
            {
                Mensaje = "El nombre del tipo de observación no puede superar los 45 caracteres.";
                return 0;
            }

            if (obj.Descripcion.Length > 150)
            {
                Mensaje = "La descripción no puede superar los 150 caracteres.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(TipoDeObservacion obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdTipoDeObservacion <= 0)
            {
                Mensaje = "Debe seleccionar un tipo de observación válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Tipo_Observacion))
            {
                Mensaje = "El nombre del tipo de observación es obligatorio.";
                return false;
            }

            obj.Tipo_Observacion = obj.Tipo_Observacion.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            if (obj.Tipo_Observacion.Length > 45)
            {
                Mensaje = "El nombre del tipo de observación no puede superar los 45 caracteres.";
                return false;
            }

            if (obj.Descripcion.Length > 150)
            {
                Mensaje = "La descripción no puede superar los 150 caracteres.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idTipoDeObservacion, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idTipoDeObservacion <= 0)
            {
                Mensaje = "Debe seleccionar un tipo de observación válido.";
                return false;
            }

            return objCapaDato.Inactivar(idTipoDeObservacion, out Mensaje);
        }
    }
}