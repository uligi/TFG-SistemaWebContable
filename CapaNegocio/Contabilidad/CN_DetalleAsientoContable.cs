using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System.Collections.Generic;

namespace CapaNegocio.Contabilidad
{
    public class CN_DetalleAsientoContable
    {
        private CD_DetalleAsientoContable objCapaDato = new CD_DetalleAsientoContable();

        public List<DetalleAsientoContable> ListarPorAsiento(int idAsientoContable)
        {
            return objCapaDato.ListarPorAsiento(idAsientoContable);
        }

        public int Registrar(DetalleAsientoContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdAsientoContable == 0)
            {
                Mensaje = "Debe seleccionar un asiento contable.";
                return 0;
            }

            if (obj.IdCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una cuenta contable.";
                return 0;
            }

            if (obj.Debe < 0 || obj.Haber < 0)
            {
                Mensaje = "Los montos de debe y haber no pueden ser negativos.";
                return 0;
            }

            if (obj.Debe == 0 && obj.Haber == 0)
            {
                Mensaje = "Debe ingresar un monto en debe o en haber.";
                return 0;
            }

            if (obj.Debe > 0 && obj.Haber > 0)
            {
                Mensaje = "Una línea no puede tener monto en debe y haber al mismo tiempo.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.DescripcionLinea) && obj.DescripcionLinea.Length > 150)
            {
                Mensaje = "La descripción de la línea no puede superar los 150 caracteres.";
                return 0;
            }

            if (!string.IsNullOrWhiteSpace(obj.DescripcionLinea))
            {
                obj.DescripcionLinea = obj.DescripcionLinea.Trim();
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(DetalleAsientoContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdDetalleAsientoContable == 0)
            {
                Mensaje = "Debe seleccionar una línea de detalle válida.";
                return false;
            }

            if (obj.IdCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una cuenta contable.";
                return false;
            }

            if (obj.Debe < 0 || obj.Haber < 0)
            {
                Mensaje = "Los montos de debe y haber no pueden ser negativos.";
                return false;
            }

            if (obj.Activo && obj.Debe == 0 && obj.Haber == 0)
            {
                Mensaje = "Debe ingresar un monto en debe o en haber.";
                return false;
            }

            if (obj.Debe > 0 && obj.Haber > 0)
            {
                Mensaje = "Una línea no puede tener monto en debe y haber al mismo tiempo.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.DescripcionLinea) && obj.DescripcionLinea.Length > 150)
            {
                Mensaje = "La descripción de la línea no puede superar los 150 caracteres.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.DescripcionLinea))
            {
                obj.DescripcionLinea = obj.DescripcionLinea.Trim();
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idDetalleAsientoContable, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idDetalleAsientoContable == 0)
            {
                Mensaje = "Debe seleccionar una línea de detalle válida.";
                return false;
            }

            return objCapaDato.Inactivar(idDetalleAsientoContable, out Mensaje);
        }
    }
}