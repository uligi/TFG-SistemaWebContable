using CapaEntidad.Contabilidad;
using CapaNegocio.Contabilidad;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Contabilidad
{
    public class GestionarPeriodoContableController : Controller
    {
        public ActionResult PeriodosContables()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarPeriodosContables()
        {
            var lista = new CN_PeriodoContable().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarPeriodosContablesActivos()
        {
            var lista = new CN_PeriodoContable().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarPeriodoContableAbierto()
        {
            var lista = new CN_PeriodoContable().ListarAbierto();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarPeriodoContable(PeriodoContable obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdPeriodoContable == 0)
            {
                resultado = new CN_PeriodoContable().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_PeriodoContable().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CerrarPeriodoContable(int idPeriodoContable)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_PeriodoContable().Cerrar(
                idPeriodoContable,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarPeriodoContable(int idPeriodoContable)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_PeriodoContable().Inactivar(
                idPeriodoContable,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}