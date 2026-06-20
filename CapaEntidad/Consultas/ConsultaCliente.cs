using System;

namespace CapaEntidad.Consultas
{
    public class ConsultaCliente
    {
        public string Identificacion { get; set; }

        public string ClienteNombre { get; set; }

        public decimal LimiteCredito { get; set; }

        public int DiasCredito { get; set; }

        public bool Activo { get; set; }

        public decimal SaldoPendiente { get; set; }

        public int CantidadFacturasPendientes { get; set; }

        public string ClienteDescripcion
        {
            get
            {
                return (Identificacion + " - " + ClienteNombre).Trim();
            }
        }

        public bool TieneSaldoPendiente
        {
            get
            {
                return SaldoPendiente > 0;
            }
        }

        public string EstadoCliente
        {
            get
            {
                if (!Activo)
                {
                    return "Inactivo";
                }

                if (TieneSaldoPendiente)
                {
                    return "Con saldo pendiente";
                }

                return "Al día";
            }
        }

        public string Recomendacion
        {
            get
            {
                if (TieneSaldoPendiente)
                {
                    return "Revisar cuentas por cobrar del cliente.";
                }

                return "Cliente sin saldo pendiente.";
            }
        }

        public string LimiteCreditoTexto
        {
            get
            {
                return LimiteCredito.ToString("N2");
            }
        }

        public string SaldoPendienteTexto
        {
            get
            {
                return SaldoPendiente.ToString("N2");
            }
        }
    }
}