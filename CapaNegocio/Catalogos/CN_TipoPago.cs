using CapaDatos.Catalogos;
using CapaEntidad.Catalogos;
using System.Collections.Generic;

namespace CapaNegocio.Catalogos
{
    public class CN_TipoPago
    {
        private CD_TipoPago objCapaDato = new CD_TipoPago();

        public List<TipoPago> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(TipoPago obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de pago es obligatorio.";
                return 0;
            }

            obj.Nombre = obj.Nombre.Trim();

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del tipo de pago no puede superar los 45 caracteres.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(TipoPago obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdTipoPago <= 0)
            {
                Mensaje = "Debe seleccionar un tipo de pago válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de pago es obligatorio.";
                return false;
            }

            obj.Nombre = obj.Nombre.Trim();

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del tipo de pago no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idTipoPago, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idTipoPago <= 0)
            {
                Mensaje = "Debe seleccionar un tipo de pago válido.";
                return false;
            }

            return objCapaDato.Inactivar(idTipoPago, out Mensaje);
        }
    }
}