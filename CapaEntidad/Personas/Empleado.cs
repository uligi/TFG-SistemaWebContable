using System;

namespace CapaEntidad.Personas
{
    public class Empleado
    {
        public string Identificacion { get; set; }

        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        public int CodigoDistrito { get; set; }
        public string Direccion { get; set; }

        public string CodigoEmpleado { get; set; }

        public int IdPuesto { get; set; }
        public string PuestoNombre { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public int IdRol { get; set; }
        public string RolNombre { get; set; }

        public string NombreUsuario { get; set; }

        // Se usa internamente para mostrar la clave temporal generada.
        // Ya no se pide desde la vista.
        public string Clave { get; set; }

        // Se envía al SP ya encriptada.
        public string ClaveHash { get; set; }

        public bool RestablecerClave { get; set; }

        public DateTime? UltimoAcceso { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public string DistritoNombre { get; set; }

        public int CodigoCanton { get; set; }
        public string CantonNombre { get; set; }

        public int CodigoProvincia { get; set; }
        public string ProvinciaNombre { get; set; }

        public string NombreCompleto
        {
            get
            {
                return (Nombre + " " + PrimerApellido + " " + SegundoApellido).Trim();
            }
        }
    }
}