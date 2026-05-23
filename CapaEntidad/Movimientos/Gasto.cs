using System;

namespace CapaEntidad.Movimientos
{
    public class Gasto
    {
        public int IdGasto { get; set; }

        public int? IdCuentaPorPagar { get; set; }
        public string NumeroDocumento { get; set; }

        public int? IdAbonoCuentaPorPagar { get; set; }

        public int IdCuentaContable { get; set; }
        public string CodigoCuenta { get; set; }
        public string NombreCuenta { get; set; }

        public DateTime FechaGasto { get; set; }

        public string Descripcion { get; set; }

        public int IdTipoGasto { get; set; }
        public string TipoGastoNombre { get; set; }

        public decimal Monto { get; set; }

        public string IdentificacionProveedor { get; set; }
        public string ProveedorNombre { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public bool Activo { get; set; }
    }
}