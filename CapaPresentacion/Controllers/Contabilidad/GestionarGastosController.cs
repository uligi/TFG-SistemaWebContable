using CapaEntidad.Movimientos;
using CapaNegocio.Movimientos;
using CapaNegocio.Catalogos;
using CapaNegocio.Contabilidad;
using CapaNegocio.CuentasPorPagar;
using System.Linq;
using System.Web.Mvc;
using CapaPresentacion.Filtros;
using CapaNegocio.Contabilidad;

namespace CapaPresentacion.Controllers.Contabilidad
{
    [PermisoAuthorize(CodigoModulo = "GASTOS")]
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

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarGastosActivos()
        {
            var lista = new CN_Gasto().ListarActivos();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerGasto(string numeroGasto)
        {
            var obj = new CN_Gasto().Obtener(numeroGasto);

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GuardarGasto(Gasto obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.NumeroGasto))
            {
                string numeroGastoGenerado = string.Empty;

                resultado = new CN_Gasto().Registrar(
                    obj,
                    out mensaje,
                    out numeroGastoGenerado
                );

                if (resultado != null && resultado.ToString() != "0")
                {
                    obj.NumeroGasto = numeroGastoGenerado;

                    string mensajeAsiento = string.Empty;
                    string numeroAsiento = string.Empty;

                    bool asientoGenerado = new CN_AsientoAutomatico().GenerarPorGasto(
                        obj,
                        out mensajeAsiento,
                        out numeroAsiento
                    );

                    if (asientoGenerado)
                    {
                        mensaje = mensaje + " Asiento contable generado: " + numeroAsiento + ".";
                    }
                    else
                    {
                        mensaje = mensaje + " El gasto fue registrado, pero no se pudo generar el asiento contable: " + mensajeAsiento;
                    }
                }

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroGasto = numeroGastoGenerado
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultado = new CN_Gasto().Editar(obj, out mensaje);

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroGasto = obj.NumeroGasto
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult InactivarGasto(string numeroGasto)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_Gasto().Inactivar(
                numeroGasto,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // TIPOS DE GASTO
        // ===============================

        [HttpGet]
        public JsonResult ListarTiposGastoActivos()
        {
            var lista = new CN_TipoGasto().Listar()
                .Where(x => x.Activo == true)
                .ToList();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CUENTAS CONTABLES
        // ===============================

        [HttpGet]
        public JsonResult ListarCuentasContablesGasto()
        {
            var lista = new CN_CuentaContable().Listar()
                .Where(x =>
                    x.Activo == true &&
                    x.AceptaMovimientos == true &&
                    x.IdTipoCuentaContable == 5
                )
                .ToList();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // ABONOS CXP
        // ===============================

        [HttpGet]
        public JsonResult ListarAbonosCuentaPorPagar()
        {
            var lista = new CN_AbonoCuentaPorPagar().Listar();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarAbonosCuentaPorPagarActivos()
        {
            var lista = new CN_AbonoCuentaPorPagar().Listar()
                .Where(x => x.Activo == true)
                .ToList();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }
    }
}