using CapaEntidad.Contabilidad;
using CapaNegocio.Contabilidad;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Contabilidad
{
    public class GestionarAsientoContableController : Controller
    {
        public ActionResult AsientosContables()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarAsientosContables()
        {
            var lista = new CN_AsientoContable().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDetallesAsientoContable(int idAsientoContable)
        {
            var lista = new CN_DetalleAsientoContable().ListarPorAsiento(idAsientoContable);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCuentasParaMovimientos()
        {
            var lista = new CN_CuentaContable().ListarParaMovimientos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarAsientoContable(AsientoContable obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdAsientoContable == 0)
            {
                resultado = new CN_AsientoContable().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_AsientoContable().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarAsientoContable(int idAsientoContable)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_AsientoContable().Inactivar(
                idAsientoContable,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ValidarCuadreAsientoContable(int idAsientoContable)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_AsientoContable().ValidarCuadre(
                idAsientoContable,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarDetalleAsientoContable(DetalleAsientoContable obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdDetalleAsientoContable == 0)
            {
                resultado = new CN_DetalleAsientoContable().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_DetalleAsientoContable().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarDetalleAsientoContable(int idDetalleAsientoContable)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_DetalleAsientoContable().Inactivar(
                idDetalleAsientoContable,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}