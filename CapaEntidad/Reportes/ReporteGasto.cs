using System;

namespace CapaEntidad.Reportes
{
    public class ReporteGasto
    {
        public string NumeroGasto { get; set; }

        public DateTime FechaGasto { get; set; }

        public string TipoGasto { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public string IdentificacionProveedor { get; set; }

        public string ProveedorNombre { get; set; }

        public string NumeroDocumento { get; set; }

        public string Descripcion { get; set; }

        public decimal Monto { get; set; }

        public string NumeroComprobante { get; set; }

        public string NombreArchivoComprobante { get; set; }

        public string RutaComprobante { get; set; }

        public bool Activo { get; set; }

        public string CuentaDescripcion
        {
            get
            {
                return (CodigoCuenta + " - " + NombreCuenta).Trim();
            }
        }

        public string ProveedorDescripcion
        {
            get
            {
                return (IdentificacionProveedor + " - " + ProveedorNombre).Trim();
            }
        }

        public string DocumentoDescripcion
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(NumeroDocumento))
                {
                    return "Documento: " + NumeroDocumento;
                }

                return "Sin documento asociado";
            }
        }

        public bool TieneProveedor
        {
            get
            {
                return !string.IsNullOrWhiteSpace(IdentificacionProveedor) &&
                       IdentificacionProveedor != "000000000";
            }
        }

        public bool TieneDocumento
        {
            get
            {
                return !string.IsNullOrWhiteSpace(NumeroDocumento) &&
                       NumeroDocumento != "CXP-GENERAL-000000";
            }
        }

        public bool TieneComprobante
        {
            get
            {
                return !string.IsNullOrWhiteSpace(NumeroComprobante) ||
                       !string.IsNullOrWhiteSpace(NombreArchivoComprobante) ||
                       !string.IsNullOrWhiteSpace(RutaComprobante);
            }
        }

        public string ComprobanteDescripcion
        {
            get
            {
                if (!TieneComprobante)
                {
                    return "Sin comprobante";
                }

                if (!string.IsNullOrWhiteSpace(NumeroComprobante))
                {
                    return NumeroComprobante;
                }

                if (!string.IsNullOrWhiteSpace(NombreArchivoComprobante))
                {
                    return NombreArchivoComprobante;
                }

                return RutaComprobante;
            }
        }

        public string MontoTexto
        {
            get
            {
                return Monto.ToString("N2");
            }
        }

        public string FechaGastoTexto
        {
            get
            {
                return FechaGasto.ToString("yyyy-MM-dd");
            }
        }
    }
}