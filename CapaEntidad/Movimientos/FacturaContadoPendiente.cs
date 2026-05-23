using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CapaEntidad.Movimientos
{
    public class FacturaContadoPendiente
    {
        public int IdFactura { get; set; }
        public string NumeroFactura { get; set; }

        public string IdentificacionCliente { get; set; }
        public string ClienteNombre { get; set; }

        public DateTime FechaFactura { get; set; }

        public decimal TotalFactura { get; set; }

        public string TipoFactura { get; set; }
    }
}