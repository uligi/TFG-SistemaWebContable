using CapaEntidad.Contabilidad;
using CapaNegocio.Contabilidad;
using System.Web.Mvc;
using CapaPresentacion.Filtros;

namespace CapaPresentacion.Controllers.Contabilidad
{
    [PermisoAuthorize(CodigoModulo = "CUENTAS_CONTABLES")]
    public class ConfiguracionCuentaContableController : Controller
    {
        public ActionResult ConfiguracionCuentaContable()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarConfiguracionesCuentaContable()
        {
            var lista = new CN_ConfiguracionCuentaContable().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerConfiguracionCuentaContable(string codigoOperacion)
        {
            var obj = new CN_ConfiguracionCuentaContable().ObtenerPorOperacion(codigoOperacion);
            return Json(new { data = obj }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerConfiguracionCuentaContableActiva(string codigoOperacion)
        {
            var obj = new CN_ConfiguracionCuentaContable().ObtenerActivaPorOperacion(codigoOperacion);
            return Json(new { data = obj }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarConfiguracionCuentaContable(ConfiguracionCuentaContable obj)
        {
            object resultado;
            string mensaje = string.Empty;

            ConfiguracionCuentaContable configuracionExistente =
                new CN_ConfiguracionCuentaContable().ObtenerPorOperacion(obj.CodigoOperacion);

            if (configuracionExistente == null)
            {
                resultado = new CN_ConfiguracionCuentaContable().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_ConfiguracionCuentaContable().Editar(obj, out mensaje);
            }

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarConfiguracionCuentaContable(string codigoOperacion)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_ConfiguracionCuentaContable().Inactivar(
                codigoOperacion,
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