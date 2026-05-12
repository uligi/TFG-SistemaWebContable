using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.personas
{
    public class Telefono
    {
        public string NumeroTelefono { get; set; }
        public string Identificacion { get; set; }
        public int IdTipoTelefono { get; set; }
        public bool EsPrincipal { get; set; }
        public bool Activo { get; set; }

        public string TipoTelefonoNombre { get; set; }
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
