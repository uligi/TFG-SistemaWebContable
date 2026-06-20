using CapaEntidad.Contabilidad;
using CapaNegocio.Contabilidad;
using CapaPresentacion.Filtros;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Contabilidad
{
    public class GestionarPeriodoContableController : Controller
    {
        [PermisoAuthorize(CodigoModulo = "PERIODO_CONTABLE")]
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
        public JsonResult ListarPeriodosContablesAbiertos()
        {
            var lista = new CN_PeriodoContable().ListarAbiertos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerPeriodoContable(int anio, int mes)
        {
            var obj = new CN_PeriodoContable().Obtener(anio, mes);
            return Json(new { data = obj }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarPeriodoContable(PeriodoContable obj)
        {
            object resultado;
            string mensaje = string.Empty;

            PeriodoContable periodoExistente = new CN_PeriodoContable().Obtener(obj.Anio, obj.Mes);

            if (periodoExistente == null)
            {
                resultado = new CN_PeriodoContable().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_PeriodoContable().Editar(obj, out mensaje);
            }

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CerrarPeriodoContable(int anio, int mes)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_PeriodoContable().Cerrar(
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
        public JsonResult InactivarPeriodoContable(int anio, int mes)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_PeriodoContable().Inactivar(
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
    }
}