using System;

namespace CapaEntidad.Personas
{
    public class Telefono
    {
        public string Identificacion { get; set; }

        public string NumeroTelefono { get; set; }

        // Se usa para editar cuando el número cambia,
        // porque la PK es compuesta: Identificacion + NumeroTelefono
        public string NumeroTelefonoAnterior { get; set; }

        public int IdTipoTelefono { get; set; }
        public string TipoTelefonoNombre { get; set; }

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