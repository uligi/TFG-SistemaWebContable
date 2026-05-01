using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Seguridad
{
    public class HistorialCambio
    {
        public int IdHistorialCambio { get; set; }
        public string Identificacion { get; set; }
        public string NombreTabla { get; set; }
        public int IdTipoDeObservacion { get; set; }
        public DateTime FechaHoraCambio { get; set; }
        public string Detalle { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public bool Activo { get; set; }
        public int IdRegistro { get; set; }
    }
}
