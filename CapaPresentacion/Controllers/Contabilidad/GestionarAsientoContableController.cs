using CapaEntidad.Contabilidad;
using CapaNegocio.Contabilidad;
using System.Web.Mvc;
using CapaPresentacion.Filtros;

namespace CapaPresentacion.Controllers.Contabilidad
{
    [PermisoAuthorize(CodigoModulo = "ASIENTOS")]
    public class GestionarAsientoContableController : Controller
    {
        public ActionResult AsientosContables()
        {
            return View();
        }

        // ===============================
        // ASIENTO CONTABLE
        // ===============================

        [HttpGet]
        public JsonResult ListarAsientosContables()
        {
            var lista = new CN_AsientoContable().Listar();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarAsientosContablesPorPeriodo(int anio, int mes)
        {
            var lista = new CN_AsientoContable().ListarPorPeriodo(anio, mes);

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerAsientoContable(string numeroAsiento)
        {
            var obj = new CN_AsientoContable().Obtener(numeroAsiento);

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarAsientoContable(AsientoContable obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.NumeroAsiento))
            {
                string numeroAsientoGenerado = string.Empty;

                resultado = new CN_AsientoContable().Registrar(
                    obj,
                    out mensaje,
                    out numeroAsientoGenerado
                );

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroAsiento = numeroAsientoGenerado
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultado = new CN_AsientoContable().Editar(obj, out mensaje);

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroAsiento = obj.NumeroAsiento
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ValidarCuadreAsientoContable(string numeroAsiento)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_AsientoContable().ValidarCuadre(
                numeroAsiento,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RecalcularTotalesAsientoContable(string numeroAsiento)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_AsientoContable().RecalcularTotales(
                numeroAsiento,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarAsientoContable(string numeroAsiento)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_AsientoContable().Inactivar(
                numeroAsiento,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // DETALLE ASIENTO CONTABLE
        // ===============================

        [HttpGet]
        public JsonResult ListarDetallesAsientoContable()
        {
            var lista = new CN_DetalleAsientoContable().Listar();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDetallesAsientoContablePorAsiento(string numeroAsiento)
        {
            var lista = new CN_DetalleAsientoContable().ListarPorAsiento(numeroAsiento);

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerDetalleAsientoContable(string numeroAsiento, int numeroLinea)
        {
            var obj = new CN_DetalleAsientoContable().Obtener(
                numeroAsiento,
                numeroLinea
            );

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarDetalleAsientoContable(DetalleAsientoContable obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.NumeroLinea <= 0)
            {
                int numeroLineaGenerada = 0;

                resultado = new CN_DetalleAsientoContable().Registrar(
                    obj,
                    out mensaje,
                    out numeroLineaGenerada
                );

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroLinea = numeroLineaGenerada
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultado = new CN_DetalleAsientoContable().Editar(obj, out mensaje);

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroLinea = obj.NumeroLinea
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult InactivarDetalleAsientoContable(string numeroAsiento, int numeroLinea)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_DetalleAsientoContable().Inactivar(
                numeroAsiento,
                numeroLinea,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarDetallesPorAsiento(string numeroAsiento)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_DetalleAsientoContable().InactivarPorAsiento(
                numeroAsiento,
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