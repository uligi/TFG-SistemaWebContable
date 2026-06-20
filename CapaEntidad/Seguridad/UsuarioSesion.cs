using System.Collections.Generic;

namespace CapaEntidad.Seguridad
{
    public class UsuarioSesion
    {
        public string Identificacion { get; set; }

        public string NombreUsuario { get; set; }

        public string NombreCompleto { get; set; }

        public int IdRol { get; set; }

        public string NombreRol { get; set; }

        public bool Activo { get; set; }
        public bool RestablecerClave { get; set; }

        public List<PermisoRolModulo> Permisos { get; set; }

        public UsuarioSesion()
        {
            Permisos = new List<PermisoRolModulo>();
        }

        public bool TienePermisoVer(string codigoModulo)
        {
            if (Permisos == null)
            {
                return false;
            }

            foreach (PermisoRolModulo permiso in Permisos)
            {
                if (permiso.CodigoModulo == codigoModulo &&
                    permiso.Activo &&
                    permiso.PuedeVer)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TienePermisoCrear(string codigoModulo)
        {
            if (Permisos == null)
            {
                return false;
            }

            foreach (PermisoRolModulo permiso in Permisos)
            {
                if (permiso.CodigoModulo == codigoModulo &&
                    permiso.Activo &&
                    permiso.PuedeCrear)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TienePermisoEditar(string codigoModulo)
        {
            if (Permisos == null)
            {
                return false;
            }

            foreach (PermisoRolModulo permiso in Permisos)
            {
                if (permiso.CodigoModulo == codigoModulo &&
                    permiso.Activo &&
                    permiso.PuedeEditar)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TienePermisoEliminar(string codigoModulo)
        {
            if (Permisos == null)
            {
                return false;
            }

            foreach (PermisoRolModulo permiso in Permisos)
            {
                if (permiso.CodigoModulo == codigoModulo &&
                    permiso.Activo &&
                    permiso.PuedeEliminar)
                {
                    return true;
                }
            }

            return false;
        }
    }
}