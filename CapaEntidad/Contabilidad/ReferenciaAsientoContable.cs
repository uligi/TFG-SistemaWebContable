using System;

namespace CapaEntidad.Contabilidad
{
    public class ReferenciaAsientoContable
    {
        public int IdReferenciaAsiento { get; set; }
        public string NumeroAsiento { get; set; }
        public string ModuloOrigen { get; set; }
        public string DocumentoOrigen { get; set; }
        public string TipoMovimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }

        public string ReferenciaCompleta
        {
            get
            {
                return ModuloOrigen + " - " + DocumentoOrigen + " - " + TipoMovimiento;
            }
        }
    }
}