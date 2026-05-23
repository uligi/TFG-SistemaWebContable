using CapaEntidad.Presupuesto;
using CapaNegocio.Presupuesto;
using CapaNegocio.Contabilidad;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Contabilidad
{
    public class GestionarPresupuestoMensualController : Controller
    {
        public ActionResult PresupuestoMensual()
        {
            return View();
        }

        // ===============================
        // PRESUPUESTO MENSUAL
        // ===============================

        [HttpGet]
        public JsonResult ListarPresupuestosMensuales()
        {
            var lista = new CN_PresupuestoMensual().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarPresupuestoMensual(PresupuestoMensual obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdPresupuestoMensual == 0)
            {
                resultado = new CN_PresupuestoMensual().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_PresupuestoMensual().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarPresupuestoMensual(int idPresupuestoMensual)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_PresupuestoMensual().Inactivar(
                idPresupuestoMensual,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // DETALLE PRESUPUESTO
        // ===============================

        [HttpGet]
        public JsonResult ListarDetallesPresupuestoMensual(int idPresupuestoMensual)
        {
            var lista = new CN_DetallePresupuestoMensual().ListarPorPresupuesto(idPresupuestoMensual);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarDetallePresupuestoMensual(DetallePresupuestoMensual obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdDetallePresupuestoMensual == 0)
            {
                resultado = new CN_DetallePresupuestoMensual().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_DetallePresupuestoMensual().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarDetallePresupuestoMensual(int idDetallePresupuestoMensual)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_DetallePresupuestoMensual().Inactivar(
                idDetallePresupuestoMensual,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // RESUMEN VS REAL
        // ===============================

        [HttpGet]
        public JsonResult ResumenVsReal(int idPresupuestoMensual)
        {
            var lista = new CN_PresupuestoMensual().ResumenVsReal(idPresupuestoMensual);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CUENTAS CONTABLES
        // ===============================

        [HttpGet]
        public JsonResult ListarCuentasContablesMovimiento()
        {
            var lista = new CN_CuentaContable().Listar()
                .Where(x => x.Activo == true && x.AceptaMovimientos == true)
                .ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}