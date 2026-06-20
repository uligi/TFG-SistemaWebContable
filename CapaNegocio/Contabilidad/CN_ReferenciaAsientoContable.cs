using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;

namespace CapaNegocio.Contabilidad
{
    public class CN_ReferenciaAsientoContable
    {
        private readonly CD_ReferenciaAsientoContable objCapaDato = new CD_ReferenciaAsientoContable();

        public bool Existe(
            string moduloOrigen,
            string documentoOrigen,
            string tipoMovimiento,
            out string numeroAsiento
        )
        {
            numeroAsiento = string.Empty;

            if (string.IsNullOrWhiteSpace(moduloOrigen))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(documentoOrigen))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(tipoMovimiento))
            {
                return false;
            }

            return objCapaDato.Existe(
                moduloOrigen.Trim(),
                documentoOrigen.Trim(),
                tipoMovimiento.Trim(),
                out numeroAsiento
            );
        }

        public int Registrar(
            ReferenciaAsientoContable obj,
            out string mensaje
        )
        {
            mensaje = string.Empty;

            if (obj == null)
            {
                mensaje = "No se recibió información de la referencia del asiento.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.NumeroAsiento))
            {
                mensaje = "Debe indicar el número de asiento contable.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.ModuloOrigen))
            {
                mensaje = "Debe indicar el módulo de origen.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.DocumentoOrigen))
            {
                mensaje = "Debe indicar el documento de origen.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.TipoMovimiento))
            {
                mensaje = "Debe indicar el tipo de movimiento.";
                return 0;
            }

            obj.NumeroAsiento = obj.NumeroAsiento.Trim();
            obj.ModuloOrigen = obj.ModuloOrigen.Trim();
            obj.DocumentoOrigen = obj.DocumentoOrigen.Trim();
            obj.TipoMovimiento = obj.TipoMovimiento.Trim();

            if (obj.ModuloOrigen.Length > 45)
            {
                mensaje = "El módulo de origen no puede superar los 45 caracteres.";
                return 0;
            }

            if (obj.DocumentoOrigen.Length > 45)
            {
                mensaje = "El documento de origen no puede superar los 45 caracteres.";
                return 0;
            }

            if (obj.TipoMovimiento.Length > 45)
            {
                mensaje = "El tipo de movimiento no puede superar los 45 caracteres.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out mensaje);
        }
    }
}