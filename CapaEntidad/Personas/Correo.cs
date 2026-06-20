using System;

namespace CapaEntidad.Personas
{
    public class Correo
    {
        public string Identificacion { get; set; }

        public string DireccionCorreo { get; set; }

        // Se usa para editar cuando el correo cambia,
        // porque la PK es compuesta: Identificacion + DireccionCorreo
        public string DireccionCorreoAnterior { get; set; }

        public int IdTipoCorreo { get; set; }
        public string TipoCorreoNombre { get; set; }

        public bool EsPrincipal { get; set; }
        public bool Activo { get; set; }

        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }

        public string NombreCompleto
        {
            get
            {
                return (Nombre + " " + PrimerApellido + " " + SegundoApellido).Trim();
            }
        }
    }
}