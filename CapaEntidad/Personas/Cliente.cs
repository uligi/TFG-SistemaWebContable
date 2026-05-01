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
        public string CodigoCliente { get; set; }
        public decimal? LimiteCredito { get; set; }
        public int? DiasCredito { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
