using System;

namespace CapaEntidad.Reportes
{
    public class ReporteGasto
    {
        public int IdGasto { get; set; }

        public DateTime FechaGasto { get; set; }

        public string TipoGasto { get; set; }

        public string CodigoCuenta { get; set; }

        public string NombreCuenta { get; set; }

        public string IdentificacionProveedor { get; set; }

        public string ProveedorNombre { get; set; }

        public string NumeroDocumento { get; set; }

        public string Descripcion { get; set; }

        public decimal Monto { get; set; }

        public bool Activo { get; set; }
    }
}