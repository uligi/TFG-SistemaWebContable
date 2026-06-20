using System;

namespace CapaEntidad.Consultas
{
    public class ConsultaProveedor
    {
        public string IdentificacionProveedor { get; set; }

        public string RazonSocial { get; set; }

        public string NombreContacto { get; set; }

        public int DiasCredito { get; set; }

        public bool Activo { get; set; }

        public decimal SaldoPendiente { get; set; }

        public int CantidadDocumentosPendientes { get; set; }

        public string ProveedorDescripcion
        {
            get
            {
                return (IdentificacionProveedor + " - " + RazonSocial).Trim();
            }
        }

        public bool TieneSaldoPendiente
        {
            get
            {
                return SaldoPendiente > 0;
            }
        }

        public string EstadoProveedor
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
                    return "Revisar cuentas por pagar del proveedor.";
                }

                return "Proveedor sin saldo pendiente.";
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