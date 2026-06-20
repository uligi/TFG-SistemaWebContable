using System;

namespace CapaEntidad.Reportes
{
    public class ReporteCuentaPorPagar
    {
        public string IdentificacionProveedor { get; set; }

        public string ProveedorNombre { get; set; }

        public string NumeroDocumento { get; set; }

        public DateTime FechaEmision { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public string Concepto { get; set; }

        public decimal MontoOriginal { get; set; }

        public decimal SaldoActual { get; set; }

        public string Estado { get; set; }

        public bool Activo { get; set; }

        public int DiasRestantes { get; set; }

        public string EstadoCredito { get; set; }

        public string ProveedorDescripcion
        {
            get
            {
                return (IdentificacionProveedor + " - " + ProveedorNombre).Trim();
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

        public string PrioridadPago
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

        public string RecomendacionPago
        {
            get
            {
                if (EstaVencida)
                {
                    return "Revisar pago pendiente para evitar problemas con el proveedor.";
                }

                if (EstaPorVencer)
                {
                    return "Programar el pago antes del vencimiento.";
                }

                if (EstaParcial)
                {
                    return "Dar seguimiento al saldo restante del documento.";
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