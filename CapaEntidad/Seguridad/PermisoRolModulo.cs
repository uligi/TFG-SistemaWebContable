using System;

namespace CapaEntidad.Seguridad
{
    public class PermisoRolModulo
    {
        public int IdRol { get; set; }

        public string CodigoModulo { get; set; }

        public string NombreModulo { get; set; }

        public string AreaSistema { get; set; }

        public string Controlador { get; set; }

        public string Accion { get; set; }

        public string Url { get; set; }

        public string Icono { get; set; }

        public int Orden { get; set; }

        public bool PuedeVer { get; set; }

        public bool PuedeCrear { get; set; }

        public bool PuedeEditar { get; set; }

        public bool PuedeEliminar { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public bool TieneAlgunaAccion
        {
            get
            {
                return PuedeVer || PuedeCrear || PuedeEditar || PuedeEliminar;
            }
        }

        public string PermisosTexto
        {
            get
            {
                string texto = "";

                if (PuedeVer)
                {
                    texto += "Ver";
                }

                if (PuedeCrear)
                {
                    texto += texto == "" ? "Crear" : ", Crear";
                }

                if (PuedeEditar)
                {
                    texto += texto == "" ? "Editar" : ", Editar";
                }

                if (PuedeEliminar)
                {
                    texto += texto == "" ? "Eliminar" : ", Eliminar";
                }

                if (texto == "")
                {
                    texto = "Sin permisos";
                }

                return texto;
            }
        }

        public string EstadoTexto
        {
            get
            {
                return Activo ? "Activo" : "Inactivo";
            }
        }
    }
}