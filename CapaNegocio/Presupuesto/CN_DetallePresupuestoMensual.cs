using CapaDatos.Presupuesto;
using CapaEntidad.Presupuesto;
using System.Collections.Generic;

namespace CapaNegocio.Presupuesto
{
    public class CN_DetallePresupuestoMensual
    {
        private CD_DetallePresupuestoMensual objCapaDato = new CD_DetallePresupuestoMensual();

        public List<DetallePresupuestoMensual> ListarPorPresupuesto(int idPresupuestoMensual)
        {
            return objCapaDato.ListarPorPresupuesto(idPresupuestoMensual);
        }

        public int Registrar(DetallePresupuestoMensual obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdPresupuestoMensual == 0)
            {
                Mensaje = "Debe seleccionar un presupuesto mensual.";
                return 0;
            }

            if (obj.IdCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una cuenta contable.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.TipoMovimiento))
            {
                Mensaje = "Debe seleccionar el tipo de movimiento.";
                return 0;
            }

            obj.TipoMovimiento = obj.TipoMovimiento.Trim();

            if (obj.TipoMovimiento != "Ingreso" && obj.TipoMovimiento != "Gasto")
            {
                Mensaje = "El tipo de movimiento debe ser Ingreso o Gasto.";
                return 0;
            }

            if (obj.MontoPresupuestado <= 0)
            {
                Mensaje = "El monto presupuestado debe ser mayor a cero.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(DetallePresupuestoMensual obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdDetallePresupuestoMensual == 0)
            {
                Mensaje = "Debe seleccionar un detalle válido.";
                return false;
            }

            if (obj.IdCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una cuenta contable.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.TipoMovimiento))
            {
                Mensaje = "Debe seleccionar el tipo de movimiento.";
                return false;
            }

            obj.TipoMovimiento = obj.TipoMovimiento.Trim();

            if (obj.TipoMovimiento != "Ingreso" && obj.TipoMovimiento != "Gasto")
            {
                Mensaje = "El tipo de movimiento debe ser Ingreso o Gasto.";
                return false;
            }

            if (obj.Activo && obj.MontoPresupuestado <= 0)
            {
                Mensaje = "El monto presupuestado debe ser mayor a cero.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idDetallePresupuestoMensual, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idDetallePresupuestoMensual == 0)
            {
                Mensaje = "Debe seleccionar un detalle válido.";
                return false;
            }

            return objCapaDato.Inactivar(idDetallePresupuestoMensual, out Mensaje);
        }
    }
}