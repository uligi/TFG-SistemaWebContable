using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Catalogos
{
    public class TipoDeObservacion
    {
        public int IdTipoDeObservacion { get; set; }
        public string Tipo_Observacion { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}