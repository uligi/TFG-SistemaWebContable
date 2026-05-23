namespace CapaEntidad.Contabilidad
{
    public class DetalleAsientoContable
    {
        public int IdDetalleAsientoContable { get; set; }

        public int IdAsientoContable { get; set; }

        public int IdCuentaContable { get; set; }
        public string CodigoCuenta { get; set; }
        public string NombreCuenta { get; set; }

        public decimal Debe { get; set; }
        public decimal Haber { get; set; }

        public string DescripcionLinea { get; set; }

        public bool Activo { get; set; }
    }
}