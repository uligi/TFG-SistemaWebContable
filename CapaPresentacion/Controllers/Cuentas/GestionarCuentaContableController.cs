using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad.Contabilidad;
using CapaNegocio.Contabilidad;


namespace CapaPresentacion.Controllers.Cuentas
{
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

        [HttpPost]
        public JsonResult GuardarCuentaContable(CuentaContable obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdCuentaContable == 0)
            {
                resultado = new CN_CuentaContable().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_CuentaContable().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarCuentaContable(int idCuentaContable)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_CuentaContable().Inactivar(
                idCuentaContable,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GenerarCodigoCuentaContable(int idCuentaPadre)
        {
            string mensaje = string.Empty;

            string codigo = new CN_CuentaContable().GenerarCodigo(
                idCuentaPadre,
                out mensaje
            );

            return Json(new
            {
                codigo = codigo,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }
    }
}