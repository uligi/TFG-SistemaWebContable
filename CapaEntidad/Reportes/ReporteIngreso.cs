using System;

namespace CapaEntidad.Reportes
{
    public class ReporteIngreso
    {
        public int IdIngreso { get; set; }

        public DateTime FechaIngreso { get; set; }

        public string TipoIngreso { get; set; }

        public string OrigenIngreso { get; set; }

        public string NumeroFactura { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public string Descripcion { get; set; }

        public decimal Monto { get; set; }

        public bool Activo { get; set; }
    }
}