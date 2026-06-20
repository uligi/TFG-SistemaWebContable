using System;

namespace CapaEntidad.Reportes
{
    public class ReporteClienteMasVenta
    {
        public string IdentificacionCliente { get; set; }

        public string ClienteNombre { get; set; }

        public int CantidadFacturas { get; set; }

        public decimal TotalComprado { get; set; }

        public decimal PromedioCompra { get; set; }

        public DateTime UltimaCompra { get; set; }

        public decimal LimiteCredito { get; set; }

        public int DiasCredito { get; set; }

        public string ClasificacionCliente { get; set; }

        public string ClienteDescripcion
        {
            get
            {
                return (IdentificacionCliente + " - " + ClienteNombre).Trim();
            }
        }

        public bool EsClienteFrecuente
        {
            get
            {
                return ClasificacionCliente != null &&
                       ClasificacionCliente.Trim().ToLower() == "cliente frecuente";
            }
        }

        public bool EsClienteAltoValor
        {
            get
            {
                return ClasificacionCliente != null &&
                       ClasificacionCliente.Trim().ToLower() == "cliente de alto valor";
            }
        }

        public bool EsClienteOcasional
        {
            get
            {
                return ClasificacionCliente != null &&
                       ClasificacionCliente.Trim().ToLower() == "cliente ocasional";
            }
        }

        public string AccionSugerida
        {
            get
            {
                if (EsClienteFrecuente)
                {
                    return "Mantener relación cercana y ofrecer beneficios por fidelidad.";
                }

                if (EsClienteAltoValor)
                {
                    return "Dar seguimiento comercial y revisar oportunidades de venta adicional.";
                }

                if (EsClienteOcasional)
                {
                    return "Ofrecer promociones o recordatorios para aumentar recurrencia.";
                }

                return "Dar seguimiento al comportamiento de compra del cliente.";
            }
        }

        public string TotalCompradoTexto
        {
            get
            {
                return TotalComprado.ToString("N2");
            }
        }

        public string PromedioCompraTexto
        {
            get
            {
                return PromedioCompra.ToString("N2");
            }
        }

        public string LimiteCreditoTexto
        {
            get
            {
                return LimiteCredito.ToString("N2");
            }
        }

        public string UltimaCompraTexto
        {
            get
            {
                return UltimaCompra.ToString("yyyy-MM-dd");
            }
        }
    }
}