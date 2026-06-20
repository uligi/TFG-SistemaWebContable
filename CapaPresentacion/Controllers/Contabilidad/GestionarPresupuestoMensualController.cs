using CapaEntidad.Presupuesto;
using CapaNegocio.Contabilidad;
using CapaNegocio.Presupuesto;
using CapaPresentacion.Filtros;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Contabilidad
{
    [PermisoAuthorize(CodigoModulo = "PRESUPUESTOS")]
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

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerPresupuestoMensual(int anio, int mes)
        {
            var obj = new CN_PresupuestoMensual().Obtener(anio, mes);

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarPresupuestoMensual(PresupuestoMensual obj)
        {
            object resultado;
            string mensaje = string.Empty;

            var presupuestoExistente = new CN_PresupuestoMensual().Obtener(
                obj.Anio,
                obj.Mes
            );

            if (presupuestoExistente == null)
            {
                resultado = new CN_PresupuestoMensual().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_PresupuestoMensual().Editar(obj, out mensaje);
            }

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarPresupuestoMensual(int anio, int mes)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_PresupuestoMensual().Inactivar(
                anio,
                mes,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CerrarPresupuestoMensual(int anio, int mes)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_PresupuestoMensual().Cerrar(
                anio,
                mes,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReabrirPresupuestoMensual(int anio, int mes)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_PresupuestoMensual().Reabrir(
                anio,
                mes,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // DETALLE PRESUPUESTO MENSUAL
        // ===============================

        [HttpGet]
        public JsonResult ListarDetallesPresupuestoMensual(int anio, int mes)
        {
            var lista = new CN_DetallePresupuestoMensual().ListarPorPresupuesto(
                anio,
                mes
            );

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerDetallePresupuestoMensual(int anio, int mes, string codigoCuenta, string tipoMovimiento)
        {
            var obj = new CN_DetallePresupuestoMensual().Obtener(
                anio,
                mes,
                codigoCuenta,
                tipoMovimiento
            );

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarDetallePresupuestoMensual(DetallePresupuestoMensual obj)
        {
            object resultado;
            string mensaje = string.Empty;

            var detalleExistente = new CN_DetallePresupuestoMensual().Obtener(
                obj.Anio,
                obj.Mes,
                obj.CodigoCuenta,
                obj.TipoMovimiento
            );

            if (detalleExistente == null)
            {
                resultado = new CN_DetallePresupuestoMensual().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_DetallePresupuestoMensual().Editar(obj, out mensaje);
            }

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarDetallePresupuestoMensual(int anio, int mes, string codigoCuenta, string tipoMovimiento)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_DetallePresupuestoMensual().Inactivar(
                anio,
                mes,
                codigoCuenta,
                tipoMovimiento,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivarDetallePresupuestoMensual(int anio, int mes, string codigoCuenta, string tipoMovimiento)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_DetallePresupuestoMensual().Activar(
                anio,
                mes,
                codigoCuenta,
                tipoMovimiento,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // RESUMEN PRESUPUESTO VS REAL
        // ===============================

        [HttpGet]
        public JsonResult ObtenerResumenVsReal(int anio, int mes)
        {
            var lista = new CN_PresupuestoMensual().ResumenVsReal(
                anio,
                mes
            );

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerResumenGeneral(int anio, int mes)
        {
            var obj = new CN_PresupuestoMensual().ResumenGeneral(
                anio,
                mes
            );

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CUENTAS CONTABLES
        // ===============================

        [HttpGet]
        public JsonResult ListarCuentasContablesPresupuesto()
        {
            var lista = new CN_CuentaContable().Listar()
                .Where(x =>
                    x.Activo == true &&
                    x.AceptaMovimientos == true
                )
                .ToList();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }
    }
}