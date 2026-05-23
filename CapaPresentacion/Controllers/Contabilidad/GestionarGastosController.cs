using CapaEntidad.Movimientos;
using CapaNegocio.Catalogos;
using CapaNegocio.Contabilidad;
using CapaNegocio.CuentasPorPagar;
using CapaNegocio.Movimientos;
using System.Web.Mvc;
using System.Linq;

namespace CapaPresentacion.Controllers.Contabilidad
{
    public class GestionarGastosController : Controller
    {
        public ActionResult Gastos()
        {
            return View();
        }

        // ===============================
        // GASTOS
        // ===============================

        [HttpGet]
        public JsonResult ListarGastos()
        {
            var lista = new CN_Gasto().Listar();
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
        public JsonResult GuardarGasto(Gasto obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdGasto == 0)
            {
                resultado = new CN_Gasto().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Gasto().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarGasto(int idGasto)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_Gasto().Inactivar(
                idGasto,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CUENTAS POR PAGAR DE APOYO
        // ===============================

        [HttpGet]
        public JsonResult ListarCuentasPorPagarPendientes()
        {
            var lista = new CN_CuentaPorPagar().ListarPendientes();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CATÁLOGOS DE APOYO
        // ===============================

        [HttpGet]
        public JsonResult ListarTipoGastosActivos()
        {
            var lista = new CN_TipoGasto().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}