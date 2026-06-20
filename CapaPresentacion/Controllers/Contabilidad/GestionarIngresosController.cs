using CapaEntidad.Movimientos;
using CapaNegocio.Movimientos;
using CapaNegocio.Catalogos;
using CapaNegocio.Contabilidad;
using System.Linq;
using System.Web.Mvc;
using CapaPresentacion.Filtros;
using CapaNegocio.Contabilidad;

namespace CapaPresentacion.Controllers.Movimientos
{
    public class GestionarIngresosController : Controller
    {
        [PermisoAuthorize(CodigoModulo = "INGRESOS")]
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

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarIngresosActivos()
        {
            var lista = new CN_Ingreso().ListarActivos();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerIngreso(string numeroIngreso)
        {
            var obj = new CN_Ingreso().Obtener(numeroIngreso);

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarIngreso(Ingreso obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.NumeroIngreso))
            {
                string numeroIngresoGenerado = string.Empty;

                resultado = new CN_Ingreso().Registrar(
                    obj,
                    out mensaje,
                    out numeroIngresoGenerado
                );

                if (resultado != null && resultado.ToString() != "0")
                {
                    obj.NumeroIngreso = numeroIngresoGenerado;

                    string mensajeAsiento = string.Empty;
                    string numeroAsiento = string.Empty;

                    bool asientoGenerado = new CN_AsientoAutomatico().GenerarPorIngreso(
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
                        mensaje = mensaje + " El ingreso fue registrado, pero no se pudo generar el asiento contable: " + mensajeAsiento;
                    }
                }

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroIngreso = numeroIngresoGenerado
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultado = new CN_Ingreso().Editar(obj, out mensaje);

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroIngreso = obj.NumeroIngreso
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult InactivarIngreso(string numeroIngreso)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_Ingreso().Inactivar(
                numeroIngreso,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // FACTURAS CONTADO PENDIENTES
        // ===============================

        [HttpGet]
        public JsonResult ListarFacturasContadoPendientes()
        {
            var lista = new CN_Ingreso().ListarFacturasContadoPendientes();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // ABONOS CXC PENDIENTES
        // ===============================

        [HttpGet]
        public JsonResult ListarAbonosPendientesIngreso()
        {
            var lista = new CN_Ingreso().ListarAbonosPendientes();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CATÁLOGOS
        // ===============================

        [HttpGet]
        public JsonResult ListarTiposIngresoActivos()
        {
            var lista = new CN_TipoIngreso().Listar()
                .Where(x => x.Activo)
                .ToList();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCuentasContablesIngreso()
        {
            var lista = new CN_CuentaContable().Listar()
                .Where(x =>
                    x.Activo == true &&
                    x.AceptaMovimientos == true &&
                    x.IdTipoCuentaContable == 4
                )
                .ToList();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }
    }
}