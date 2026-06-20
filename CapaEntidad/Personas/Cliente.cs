using System;

namespace CapaEntidad.Personas
{
    public class Cliente
    {
        public string Identificacion { get; set; }

        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        public int CodigoDistrito { get; set; }
        public string Direccion { get; set; }

        public string CodigoCliente { get; set; }
        public decimal LimiteCredito { get; set; }
        public int DiasCredito { get; set; }

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