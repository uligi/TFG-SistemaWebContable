using System;

namespace CapaEntidad.Reportes
{
    public class ReporteIngreso
    {
        public string NumeroIngreso { get; set; }

        public DateTime FechaIngreso { get; set; }

        public string TipoIngreso { get; set; }

        public string OrigenIngreso { get; set; }

        public string NumeroFactura { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public string Descripcion { get; set; }

        public decimal Monto { get; set; }

        public bool Activo { get; set; }

        public string CuentaDescripcion
        {
            get
            {
                return (CodigoCuenta + " - " + NombreCuenta).Trim();
            }
        }

        public string ReferenciaDescripcion
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(NumeroFactura))
                {
                    return "Factura: " + NumeroFactura;
                }

                return "Sin factura asociada";
            }
        }

        public bool EsIngresoPorFactura
        {
            get
            {
                return !string.IsNullOrWhiteSpace(NumeroFactura);
            }
        }

        public bool EsIngresoManual
        {
            get
            {
                return string.IsNullOrWhiteSpace(NumeroFactura);
            }
        }

        public string MontoTexto
        {
            get
            {
                return Monto.ToString("N2");
            }
        }

        public string FechaIngresoTexto
        {
            get
            {
                return FechaIngreso.ToString("yyyy-MM-dd");
            }
        }
    }
}