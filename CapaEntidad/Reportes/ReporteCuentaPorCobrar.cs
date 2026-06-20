using System;

namespace CapaEntidad.Reportes
{
    public class ReporteCuentaPorCobrar
    {
        public string IdentificacionCliente { get; set; }

        public string NumeroFactura { get; set; }

        public string ClienteNombre { get; set; }

        public DateTime FechaEmision { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public decimal MontoOriginal { get; set; }

        public decimal SaldoActual { get; set; }

        public string Estado { get; set; }

        public bool Activo { get; set; }

        public int DiasRestantes { get; set; }

        public string EstadoCredito { get; set; }

        public string ClienteDescripcion
        {
            get
            {
                return (IdentificacionCliente + " - " + ClienteNombre).Trim();
            }
        }

        public decimal MontoPagado
        {
            get
            {
                return MontoOriginal - SaldoActual;
            }
        }

        public bool EstaPagada
        {
            get
            {
                return SaldoActual <= 0 ||
                       (
                           Estado != null &&
                           Estado.Trim().ToLower() == "pagada"
                       );
            }
        }

        public bool EstaPendiente
        {
            get
            {
                return Estado != null &&
                       Estado.Trim().ToLower() == "pendiente";
            }
        }

        public bool EstaParcial
        {
            get
            {
                return Estado != null &&
                       Estado.Trim().ToLower() == "parcial";
            }
        }

        public bool EstaVencida
        {
            get
            {
                return EstadoCredito != null &&
                       EstadoCredito.Trim().ToLower() == "vencida";
            }
        }

        public bool EstaPorVencer
        {
            get
            {
                return EstadoCredito != null &&
                       EstadoCredito.Trim().ToLower() == "por vencer";
            }
        }

        public string DiasRestantesTexto
        {
            get
            {
                if (EstaPagada)
                {
                    return "Pagada";
                }

                if (DiasRestantes < 0)
                {
                    return Math.Abs(DiasRestantes).ToString() + " día(s) vencida";
                }

                if (DiasRestantes == 0)
                {
                    return "Vence hoy";
                }

                return DiasRestantes.ToString() + " día(s)";
            }
        }

        public string PrioridadCobro
        {
            get
            {
                if (EstaVencida)
                {
                    return "Alta";
                }

                if (EstaPorVencer)
                {
                    return "Media";
                }

                return "Normal";
            }
        }

        public string RecomendacionCobro
        {
            get
            {
                if (EstaVencida)
                {
                    return "Contactar al cliente y priorizar recuperación de saldo vencido.";
                }

                if (EstaPorVencer)
                {
                    return "Dar seguimiento preventivo antes del vencimiento.";
                }

                if (EstaParcial)
                {
                    return "Dar seguimiento al saldo pendiente de la factura.";
                }

                return "Mantener seguimiento normal.";
            }
        }

        public string MontoOriginalTexto
        {
            get
            {
                return MontoOriginal.ToString("N2");
            }
        }

        public string SaldoActualTexto
        {
            get
            {
                return SaldoActual.ToString("N2");
            }
        }

        public string MontoPagadoTexto
        {
            get
            {
                return MontoPagado.ToString("N2");
            }
        }

        public string FechaEmisionTexto
        {
            get
            {
                return FechaEmision.ToString("yyyy-MM-dd");
            }
        }

        public string FechaVencimientoTexto
        {
            get
            {
                return FechaVencimiento.ToString("yyyy-MM-dd");
            }
        }
    }
}