using CapaEntidad.CuentasPorPagar;
using CapaNegocio.CuentasPorPagar;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Cuentas
{
    public class GestionarCuentasPorPagarController : Controller
    {
        public ActionResult CuentasPorPagar()
        {
            return View();
        }

        // ===============================
        // CUENTAS POR PAGAR
        // ===============================

        [HttpGet]
        public JsonResult ListarCuentasPorPagar()
        {
            var lista = new CN_CuentaPorPagar().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCuentasPorPagarPendientes()
        {
            var lista = new CN_CuentaPorPagar().ListarPendientes();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCuentaPorPagar(CuentaPorPagar obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdCuentaPorPagar == 0)
            {
                resultado = new CN_CuentaPorPagar().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_CuentaPorPagar().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarCuentaPorPagar(int idCuentaPorPagar)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_CuentaPorPagar().Inactivar(
                idCuentaPorPagar,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // ABONOS CUENTA POR PAGAR
        // ===============================

        [HttpGet]
        public JsonResult ListarAbonosCuentaPorPagar(int idCuentaPorPagar)
        {
            var lista = new CN_AbonoCuentaPorPagar().ListarPorCuenta(idCuentaPorPagar);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarAbonoCuentaPorPagar(AbonoCuentaPorPagar obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdAbonoCuentaPorPagar == 0)
            {
                resultado = new CN_AbonoCuentaPorPagar().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_AbonoCuentaPorPagar().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarAbonoCuentaPorPagar(int idAbonoCuentaPorPagar)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_AbonoCuentaPorPagar().Inactivar(
                idAbonoCuentaPorPagar,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}