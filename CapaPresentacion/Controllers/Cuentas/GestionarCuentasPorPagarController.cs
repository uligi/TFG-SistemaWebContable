using CapaEntidad.CuentasPorPagar;
using CapaNegocio.CuentasPorPagar;
using CapaNegocio.Proveedores;
using CapaPresentacion.Filtros;
using System.Linq;
using System.Web.Mvc;
using CapaNegocio.Contabilidad;

namespace CapaPresentacion.Controllers.Cuentas
{
    [PermisoAuthorize(CodigoModulo = "CUENTAS_PAGAR")]
    public class GestionarCuentasPorPagarController : Controller
    {
        public ActionResult CuentasPorPagar()
        {
            return View();
        }

        // ===============================
        // CUENTAS POR PAGAR
        // ===============================

        [HttpGet]
        public JsonResult ListarCuentasPorPagar()
        {
            var lista = new CN_CuentaPorPagar().Listar();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCuentasPorPagarPendientes()
        {
            var lista = new CN_CuentaPorPagar().ListarPendientes();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCuentasPorPagarPorProveedor(string identificacionProveedor)
        {
            var lista = new CN_CuentaPorPagar().ListarPorProveedor(identificacionProveedor);

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerCuentaPorPagar(string identificacionProveedor, string numeroDocumento)
        {
            var obj = new CN_CuentaPorPagar().Obtener(
                identificacionProveedor,
                numeroDocumento
            );

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCuentaPorPagar(CuentaPorPagar obj)
        {
            object resultado;
            string mensaje = string.Empty;

            var cuentaExistente = string.IsNullOrWhiteSpace(obj.NumeroDocumento)
                ? null
                : new CN_CuentaPorPagar().Obtener(
                    obj.IdentificacionProveedor,
                    obj.NumeroDocumento
                );

            if (cuentaExistente == null)
            {
                string numeroDocumentoGenerado = string.Empty;

                resultado = new CN_CuentaPorPagar().Registrar(
                    obj,
                    out mensaje,
                    out numeroDocumentoGenerado
                );

                if (resultado != null && resultado.ToString() != "0")
                {
                    obj.NumeroDocumento = numeroDocumentoGenerado;

                    string mensajeAsiento = string.Empty;
                    string numeroAsiento = string.Empty;

                    bool asientoGenerado = new CN_AsientoAutomatico().GenerarPorCuentaPorPagar(
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
                        mensaje = mensaje + " La cuenta por pagar fue registrada, pero no se pudo generar el asiento contable: " + mensajeAsiento;
                    }
                }

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroDocumento = numeroDocumentoGenerado
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultado = new CN_CuentaPorPagar().Editar(obj, out mensaje);

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroDocumento = obj.NumeroDocumento
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RecalcularSaldoCuentaPorPagar(string identificacionProveedor, string numeroDocumento)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_CuentaPorPagar().RecalcularSaldo(
                identificacionProveedor,
                numeroDocumento,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarCuentaPorPagar(string identificacionProveedor, string numeroDocumento)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_CuentaPorPagar().Inactivar(
                identificacionProveedor,
                numeroDocumento,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // ABONOS CUENTAS POR PAGAR
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
        public JsonResult ListarAbonosCuentaPorPagarPorCuenta(string identificacionProveedor, string numeroDocumento)
        {
            var lista = new CN_AbonoCuentaPorPagar().ListarPorCuenta(
                identificacionProveedor,
                numeroDocumento
            );

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerAbonoCuentaPorPagar(string identificacionProveedor, string numeroDocumento, int numeroAbono)
        {
            var obj = new CN_AbonoCuentaPorPagar().Obtener(
                identificacionProveedor,
                numeroDocumento,
                numeroAbono
            );

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarAbonoCuentaPorPagar(AbonoCuentaPorPagar obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.NumeroAbono <= 0)
            {
                int numeroAbonoGenerado = 0;

                resultado = new CN_AbonoCuentaPorPagar().Registrar(
                    obj,
                    out mensaje,
                    out numeroAbonoGenerado
                );

                if (resultado != null && resultado.ToString() != "0")
                {
                    obj.NumeroAbono = numeroAbonoGenerado;

                    string mensajeAsiento = string.Empty;
                    string numeroAsiento = string.Empty;

                    bool asientoGenerado = new CN_AsientoAutomatico().GenerarPorAbonoCuentaPorPagar(
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
                resultado = new CN_AbonoCuentaPorPagar().Editar(obj, out mensaje);

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroAbono = obj.NumeroAbono
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult InactivarAbonoCuentaPorPagar(string identificacionProveedor, string numeroDocumento, int numeroAbono)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_AbonoCuentaPorPagar().Inactivar(
                identificacionProveedor,
                numeroDocumento,
                numeroAbono,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // PROVEEDORES
        // ===============================

        [HttpGet]
        public JsonResult BuscarProveedoresCuentaPorPagar(string filtro)
        {
            filtro = string.IsNullOrWhiteSpace(filtro) ? "" : filtro.Trim().ToLower();

            var lista = new CN_Proveedor().Listar()
                .Where(x =>
                    x.Activo == true &&
                    (
                        (!string.IsNullOrEmpty(x.IdentificacionProveedor) &&
                            x.IdentificacionProveedor.ToLower().Contains(filtro)) ||

                        (!string.IsNullOrEmpty(x.RazonSocial) &&
                            x.RazonSocial.ToLower().Contains(filtro)) ||

                        (!string.IsNullOrEmpty(x.NombreContacto) &&
                            x.NombreContacto.ToLower().Contains(filtro))
                    )
                )
                .Take(10)
                .ToList();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }


    }
}