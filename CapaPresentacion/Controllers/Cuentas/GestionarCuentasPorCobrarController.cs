using CapaEntidad.Cuentas;
using CapaNegocio.Cuentas;
using CapaPresentacion.Filtros;
using System.Web.Mvc;
using CapaNegocio.Contabilidad;


namespace CapaPresentacion.Controllers.Cuentas
{
    [PermisoAuthorize(CodigoModulo = "CUENTAS_COBRAR")]
    public class GestionarCuentasPorCobrarController : Controller
    {
        public ActionResult CuentasPorCobrar()
        {
            return View();
        }

        // ===============================
        // CUENTAS POR COBRAR
        // ===============================

        [HttpGet]
        public JsonResult ListarCuentasPorCobrar()
        {
            var lista = new CN_CuentaPorCobrar().Listar();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCuentasPorCobrarPendientes()
        {
            var lista = new CN_CuentaPorCobrar().ListarPendientes();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCuentasPorCobrarPorCliente(string identificacionCliente)
        {
            var lista = new CN_CuentaPorCobrar().ListarPorCliente(identificacionCliente);

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerCuentaPorCobrar(string identificacionCliente, string numeroFactura)
        {
            var obj = new CN_CuentaPorCobrar().Obtener(
                identificacionCliente,
                numeroFactura
            );

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCuentaPorCobrar(CuentaPorCobrar obj, bool esEdicion)
        {
            object resultado;
            string mensaje = string.Empty;

            if (esEdicion)
            {
                resultado = new CN_CuentaPorCobrar().Editar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_CuentaPorCobrar().Registrar(obj, out mensaje);
            }

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RecalcularSaldoCuentaPorCobrar(string identificacionCliente, string numeroFactura)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_CuentaPorCobrar().RecalcularSaldo(
                identificacionCliente,
                numeroFactura,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarCuentaPorCobrar(string identificacionCliente, string numeroFactura)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_CuentaPorCobrar().Inactivar(
                identificacionCliente,
                numeroFactura,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // ABONOS CUENTA POR COBRAR
        // ===============================

        [HttpGet]
        public JsonResult ListarAbonosCuentaPorCobrar()
        {
            var lista = new CN_AbonoCuentaPorCobrar().Listar();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarAbonosCuentaPorCobrarPorCuenta(string identificacionCliente, string numeroFactura)
        {
            var lista = new CN_AbonoCuentaPorCobrar().ListarPorCuenta(
                identificacionCliente,
                numeroFactura
            );

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerAbonoCuentaPorCobrar(string identificacionCliente, string numeroFactura, int numeroAbono)
        {
            var obj = new CN_AbonoCuentaPorCobrar().Obtener(
                identificacionCliente,
                numeroFactura,
                numeroAbono
            );

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarAbonoCuentaPorCobrar(AbonoCuentaPorCobrar obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.NumeroAbono <= 0)
            {
                int numeroAbonoGenerado = 0;

                resultado = new CN_AbonoCuentaPorCobrar().Registrar(
                    obj,
                    out mensaje,
                    out numeroAbonoGenerado
                );

                if (resultado != null && resultado.ToString() != "0")
                {
                    obj.NumeroAbono = numeroAbonoGenerado;

                    string mensajeAsiento = string.Empty;
                    string numeroAsiento = string.Empty;

                    bool asientoGenerado = new CN_AsientoAutomatico().GenerarPorAbonoCuentaPorCobrar(
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
                        mensaje = mensaje + " El abono fue registrado, pero no se pudo generar el asiento contable: " + mensajeAsiento;
                    }
                }

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroAbono = numeroAbonoGenerado
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultado = new CN_AbonoCuentaPorCobrar().Editar(obj, out mensaje);

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroAbono = obj.NumeroAbono
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult InactivarAbonoCuentaPorCobrar(string identificacionCliente, string numeroFactura, int numeroAbono)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_AbonoCuentaPorCobrar().Inactivar(
                identificacionCliente,
                numeroFactura,
                numeroAbono,
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