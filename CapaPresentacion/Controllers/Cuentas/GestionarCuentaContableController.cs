using CapaEntidad.Contabilidad;
using CapaNegocio.Contabilidad;
using CapaPresentacion.Filtros;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Contabilidad
{
    [PermisoAuthorize(CodigoModulo = "CUENTAS_CONTABLES")]
    public class GestionarCuentaContableController : Controller
    {
        public ActionResult CuentasContables()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarCuentasContables()
        {
            var lista = new CN_CuentaContable().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCuentasContablesActivas()
        {
            var lista = new CN_CuentaContable().ListarActivas();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCuentasContablesParaMovimientos()
        {
            var lista = new CN_CuentaContable().ListarParaMovimientos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerCuentaContable(string codigoCuenta)
        {
            var obj = new CN_CuentaContable().Obtener(codigoCuenta);
            return Json(new { data = obj }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GenerarCodigoCuentaContable(string codigoCuentaPadre)
        {
            string codigoGenerado = string.Empty;
            string mensaje = string.Empty;

            bool resultado = new CN_CuentaContable().GenerarCodigo(
                codigoCuentaPadre,
                out codigoGenerado,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                codigoGenerado = codigoGenerado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCuentaContable(CuentaContable obj, bool esEdicion)
        {
            object resultado;
            string mensaje = string.Empty;

            if (esEdicion)
            {
                resultado = new CN_CuentaContable().Editar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_CuentaContable().Registrar(obj, out mensaje);
            }

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarCuentaContable(string codigoCuenta)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_CuentaContable().Inactivar(
                codigoCuenta,
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