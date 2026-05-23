using System;

namespace CapaEntidad.Movimientos
{
    public class Ingreso
    {
        public int IdIngreso { get; set; }

        public int? IdFactura { get; set; }
        public string NumeroFactura { get; set; }

        public int? IdAbonoCuentaPorCobrar { get; set; }

        public int IdCuentaContable { get; set; }
        public string CodigoCuenta { get; set; }
        public string NombreCuenta { get; set; }

        public DateTime FechaIngreso { get; set; }

        public string Descripcion { get; set; }

        public int IdTipoIngreso { get; set; }
        public string TipoIngresoNombre { get; set; }

        public string OrigenIngreso { get; set; }

        public decimal Monto { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public bool Activo { get; set; }
    }
}