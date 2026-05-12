using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Personas
{
    public class Correo
    {
        public string Identificacion { get; set; }
        public string DireccionCorreo { get; set; }
        public int IdTipoCorreo { get; set; }
        public bool EsPrincipal { get; set; }
        public bool Activo { get; set; }

        public string TipoCorreoNombre { get; set; }
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