using CapaEntidad.Contabilidad;
using CapaNegocio.Contabilidad;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Contabilidad
{
    public class ConfiguracionCuentaContableController : Controller
    {
        public ActionResult ConfiguracionCuentaContable()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarConfiguraciones()
        {
            var lista = new CN_ConfiguracionCuentaContable().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarConfiguracion(ConfiguracionCuentaContable obj)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_ConfiguracionCuentaContable().Editar(
                obj,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

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