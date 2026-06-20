using System;

namespace CapaEntidad.Consultas
{
    public class ConsultaCuentaPorPagar
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

        public decimal MontoPagado
        {
            get
            {
                return MontoOriginal - SaldoActual;
            }
        }

        public string ProveedorDescripcion
        {
            get
            {
                return (IdentificacionProveedor + " - " + ProveedorNombre).Trim();
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
                if (SaldoActual <= 0)
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

        public string Recomendacion
        {
            get
            {
                if (EstaVencida)
                {
                    return "Revisar pago pendiente al proveedor.";
                }

                if (EstaPorVencer)
                {
                    return "Programar pago antes del vencimiento.";
                }

                return "Seguimiento normal.";
            }
        }
    }
}