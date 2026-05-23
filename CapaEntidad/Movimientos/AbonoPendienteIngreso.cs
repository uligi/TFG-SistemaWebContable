using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CapaEntidad.Movimientos
{
    public class AbonoPendienteIngreso
    {
        public int IdAbonoCuentaPorCobrar { get; set; }
        public int IdCuentaPorCobrar { get; set; }

        public DateTime FechaAbono { get; set; }
        public decimal MontoAbono { get; set; }

        public int IdFactura { get; set; }
        public string NumeroFactura { get; set; }

        public string IdentificacionCliente { get; set; }
        public string ClienteNombre { get; set; }
    }
}
