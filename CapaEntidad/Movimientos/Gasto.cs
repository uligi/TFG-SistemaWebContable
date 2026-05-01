using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Movimientos
{
    public class Gasto
    {
        public int IdGasto { get; set; }
        public DateTime FechaGasto { get; set; }
        public string Descripcion { get; set; }
        public string CategoriaGasto { get; set; }
        public decimal Monto { get; set; }
        public string IdentificacionProveedor { get; set; }
        public string NumeroComprobante { get; set; }
        public string NombreArchivoComprobante { get; set; }
        public string RutaComprobante { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
