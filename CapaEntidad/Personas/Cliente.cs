using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Personas
{
    public class Cliente
    {
        public string Identificacion { get; set; }

        // Datos de Persona
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? IdDistrito { get; set; }
        public string Direccion { get; set; }

        // Datos de Cliente
        public string CodigoCliente { get; set; }
        public decimal? LimiteCredito { get; set; }
        public int? DiasCredito { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }

        // Datos para mostrar ubicación
        public string DistritoNombre { get; set; }
        public int? IdCanton { get; set; }
        public string CantonNombre { get; set; }
        public int? IdProvincia { get; set; }
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
