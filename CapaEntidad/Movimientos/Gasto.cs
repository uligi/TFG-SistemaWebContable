using System;

namespace CapaEntidad.Movimientos
{
    public class Gasto
    {
        public string NumeroGasto { get; set; }

        public DateTime FechaGasto { get; set; }

        public string Descripcion { get; set; }

        public int IdTipoGasto { get; set; }

        public string TipoGastoNombre { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public decimal Monto { get; set; }

        public string IdentificacionProveedor { get; set; }

        public string NumeroDocumento { get; set; }

        public string ProveedorNombre { get; set; }

        public string ConceptoCuentaPorPagar { get; set; }

        public decimal MontoOriginal { get; set; }

        public decimal SaldoActual { get; set; }

        public string EstadoCuentaPorPagar { get; set; }

        public string IdentificacionProveedorAbono { get; set; }

        public string NumeroDocumentoAbono { get; set; }

        public int NumeroAbonoCuentaPorPagar { get; set; }

        public DateTime FechaAbono { get; set; }

        public decimal MontoAbono { get; set; }

        public string NumeroComprobante { get; set; }

        public string NombreArchivoComprobante { get; set; }

        public string RutaComprobante { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

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

        public string CuentaPorPagarDescripcion
        {
            get
            {
                return (NumeroDocumento + " - " + ConceptoCuentaPorPagar).Trim();
            }
        }

        public string AbonoDescripcion
        {
            get
            {
                return NumeroDocumentoAbono + " / Abono #" + NumeroAbonoCuentaPorPagar.ToString();
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

        public bool EsGastoPorAbonoCuentaPorPagar
        {
            get
            {
                return !string.IsNullOrWhiteSpace(IdentificacionProveedorAbono) &&
                       !string.IsNullOrWhiteSpace(NumeroDocumentoAbono) &&
                       NumeroAbonoCuentaPorPagar > 0;
            }
        }

        public string MontoTexto
        {
            get
            {
                return Monto.ToString("N2");
            }
        }

        public string MontoAbonoTexto
        {
            get
            {
                return MontoAbono.ToString("N2");
            }
        }

        public string SaldoActualTexto
        {
            get
            {
                return SaldoActual.ToString("N2");
            }
        }
    }
}