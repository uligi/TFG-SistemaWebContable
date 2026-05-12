using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.personas
{
    public class Empleado
    {
        public string Identificacion { get; set; }

        // Datos de Persona
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? IdDistrito { get; set; }
        public string Direccion { get; set; }

        // Datos de Empleado
        public string CodigoEmpleado { get; set; }
        public int? IdPuesto { get; set; }
        public string PuestoNombre { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public int IdRol { get; set; }
        public string RolNombre { get; set; }
        public string NombreUsuario { get; set; }
        public string ClaveHash { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public bool RestablecerClave { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }

        // Datos para mostrar ubicación
        public string DistritoNombre { get; set; }
        public int? IdCanton { get; set; }
        public string CantonNombre { get; set; }
        public int? IdProvincia { get; set; }
        public string ProvinciaNombre { get; set; }

        // Estos campos no se guardan directamente en la tabla.
        // Sirven para mostrar el popup de credenciales al administrador.
        public string ClaveTemporal { get; set; }

        public string NombreCompleto
        {
            get
            {
                return (Nombre + " " + PrimerApellido + " " + SegundoApellido).Trim();
            }
        }
    }
}