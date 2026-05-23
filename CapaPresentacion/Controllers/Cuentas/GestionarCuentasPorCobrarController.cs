using CapaEntidad.CuentasPorCobrar;
using CapaNegocio.CuentasPorCobrar;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Cuentas
{
    public class GestionarCuentasPorCobrarController : Controller
    {
        public ActionResult CuentasPorCobrar()
        {
            return View();
        }

        // ===============================
        // CUENTAS POR COBRAR
        // ===============================

        [HttpGet]
        public JsonResult ListarCuentasPorCobrar()
        {
            var lista = new CN_CuentaPorCobrar().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCuentasPorCobrarPendientes()
        {
            var lista = new CN_CuentaPorCobrar().ListarPendientes();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCuentaPorCobrar(CuentaPorCobrar obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdCuentaPorCobrar == 0)
            {
                resultado = new CN_CuentaPorCobrar().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_CuentaPorCobrar().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarCuentaPorCobrar(int idCuentaPorCobrar)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_CuentaPorCobrar().Inactivar(
                idCuentaPorCobrar,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // ABONOS CUENTA POR COBRAR
        // ===============================

        [HttpGet]
        public JsonResult ListarAbonosCuentaPorCobrar(int idCuentaPorCobrar)
        {
            var lista = new CN_AbonoCuentaPorCobrar().ListarPorCuenta(idCuentaPorCobrar);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarAbonoCuentaPorCobrar(AbonoCuentaPorCobrar obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdAbonoCuentaPorCobrar == 0)
            {
                resultado = new CN_AbonoCuentaPorCobrar().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_AbonoCuentaPorCobrar().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarAbonoCuentaPorCobrar(int idAbonoCuentaPorCobrar)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_AbonoCuentaPorCobrar().Inactivar(
                idAbonoCuentaPorCobrar,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}