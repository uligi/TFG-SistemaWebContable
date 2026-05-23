using CapaEntidad.Movimientos;
using CapaNegocio.Catalogos;
using CapaNegocio.Contabilidad;
using CapaNegocio.Movimientos;
using System.Web.Mvc;
using System.Linq;

namespace CapaPresentacion.Controllers.Contabilidad
{
    public class GestionarIngresosController : Controller
    {
        public ActionResult Ingresos()
        {
            return View();
        }

        // ===============================
        // INGRESOS
        // ===============================

        [HttpGet]
        public JsonResult ListarIngresos()
        {
            var lista = new CN_Ingreso().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCuentasContablesMovimiento()
        {
            var lista = new CN_CuentaContable().Listar()
                .Where(x => x.Activo == true && x.AceptaMovimientos == true)
                .ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarIngreso(Ingreso obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdIngreso == 0)
            {
                resultado = new CN_Ingreso().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Ingreso().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarIngreso(int idIngreso)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_Ingreso().Inactivar(
                idIngreso,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // FUENTES DE INGRESO
        // ===============================

        [HttpGet]
        public JsonResult ListarFacturasContadoPendientes()
        {
            var lista = new CN_Ingreso().ListarFacturasContadoPendientes();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarAbonosPendientes()
        {
            var lista = new CN_Ingreso().ListarAbonosPendientes();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CATÁLOGOS DE APOYO
        // ===============================

        [HttpGet]
        public JsonResult ListarTipoIngresosActivos()
        {
            var lista = new CN_TipoIngreso().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}