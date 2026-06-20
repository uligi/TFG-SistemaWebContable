using System;

namespace CapaEntidad.CuentasPorPagar
{
    public class CuentaPorPagar
    {
        public string IdentificacionProveedor { get; set; }

        public string ProveedorNombre { get; set; }

        public int DiasCredito { get; set; }

        public string NumeroDocumento { get; set; }

        public DateTime FechaEmision { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public string Concepto { get; set; }

        public decimal MontoOriginal { get; set; }

        public decimal SaldoActual { get; set; }

        public string Estado { get; set; }

        public int DiasRestantes { get; set; }

        public string EstadoCredito { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Activo { get; set; }

        public string ProveedorDescripcion
        {
            get
            {
                return (IdentificacionProveedor + " - " + ProveedorNombre).Trim();
            }
        }

        public string CuentaDescripcion
        {
            get
            {
                return (NumeroDocumento + " - " + ProveedorDescripcion).Trim();
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

        public bool EstaPorVencer
        {
            get
            {
                return Activo &&
                       SaldoActual > 0 &&
                       DiasRestantes >= 0 &&
                       DiasRestantes <= 3;
            }
        }

        public bool EstaVencida
        {
            get
            {
                return Activo &&
                       SaldoActual > 0 &&
                       DiasRestantes < 0;
            }
        }

        public bool PuedeRecibirAbono
        {
            get
            {
                return Activo &&
                       SaldoActual > 0 &&
                       !EstaPagada &&
                       Estado != null &&
                       Estado.Trim().ToLower() != "anulada";
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
    }
}