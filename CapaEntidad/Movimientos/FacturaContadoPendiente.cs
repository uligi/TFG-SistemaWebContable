using System;

namespace CapaEntidad.Movimientos
{
    public class FacturaContadoPendiente
    {
        public string IdentificacionCliente { get; set; }

        public string NumeroFactura { get; set; }

        public string ClienteNombre { get; set; }

        public DateTime FechaFactura { get; set; }

        public decimal TotalFactura { get; set; }

        public string TipoFactura { get; set; }

        public string ClienteDescripcion
        {
            get
            {
                return (IdentificacionCliente + " - " + ClienteNombre).Trim();
            }
        }

        public string FacturaDescripcion
        {
            get
            {
                return NumeroFactura + " - " + ClienteDescripcion;
            }
        }

        public string TotalFacturaTexto
        {
            get
            {
                return TotalFactura.ToString("N2");
            }
        }
    }
}