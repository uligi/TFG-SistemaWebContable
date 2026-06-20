using CapaDatos.Seguridad;
using CapaEntidad.Seguridad;
using System.Collections.Generic;

namespace CapaNegocio.Seguridad
{
    public class CN_Rol
    {
        private CD_Rol objCapaDato = new CD_Rol();

        public List<Rol> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Rol obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del rol es obligatorio.";
                return 0;
            }

            obj.Nombre = obj.Nombre.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del rol no puede superar los 45 caracteres.";
                return 0;
            }

            if (obj.Descripcion.Length > 150)
            {
                Mensaje = "La descripción del rol no puede superar los 150 caracteres.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Rol obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdRol <= 0)
            {
                Mensaje = "Debe seleccionar un rol válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del rol es obligatorio.";
                return false;
            }

            obj.Nombre = obj.Nombre.Trim();
            obj.Descripcion = obj.Descripcion == null ? "" : obj.Descripcion.Trim();

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del rol no puede superar los 45 caracteres.";
                return false;
            }

            if (obj.Descripcion.Length > 150)
            {
                Mensaje = "La descripción del rol no puede superar los 150 caracteres.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idRol, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idRol <= 0)
            {
                Mensaje = "Debe seleccionar un rol válido.";
                return false;
            }

            return objCapaDato.Inactivar(idRol, out Mensaje);
        }
    }
}