using CapaNegocio.Consultas;
using CapaPresentacion.Filtros;
using System;
using System.Web.Mvc;


namespace CapaPresentacion.Controllers.Contabilidad
{
    public class ConsultasController : Controller
    {
        [PermisoAuthorize(CodigoModulo = "CONSULTAS")]
        public ActionResult Consultas()
        {
            return View();
        }

        // ===============================
        // FACTURAS
        // ===============================

        [HttpGet]
        public JsonResult ConsultarFacturas(string filtro, string fechaInicio, string fechaFin, string estado)
        {
            DateTime? inicio;
            DateTime? fin;
            string mensaje = string.Empty;

            if (!new CN_Consultas().ValidarFechasTexto(fechaInicio, fechaFin, out inicio, out fin, out mensaje))
            {
                return Json(new
                {
                    resultado = false,
                    mensaje = mensaje,
                    data = new object[0]
                }, JsonRequestBehavior.AllowGet);
            }

            var lista = new CN_Consultas().ConsultarFacturas(
                filtro,
                inicio,
                fin,
                estado
            );

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ConsultarDetalleFactura(string numeroFactura)
        {
            var lista = new CN_Consultas().ConsultarDetalleFactura(numeroFactura);

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CUENTAS POR COBRAR
        // ===============================

        [HttpGet]
        public JsonResult ConsultarCuentasPorCobrar(string filtro, string estado, string estadoCredito)
        {
            var lista = new CN_Consultas().ConsultarCuentasPorCobrar(
                filtro,
                estado,
                estadoCredito
            );

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CUENTAS POR PAGAR
        // ===============================

        [HttpGet]
        public JsonResult ConsultarCuentasPorPagar(string filtro, string estado, string estadoCredito)
        {
            var lista = new CN_Consultas().ConsultarCuentasPorPagar(
                filtro,
                estado,
                estadoCredito
            );

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // PRODUCTOS
        // ===============================

        [HttpGet]
        public JsonResult ConsultarProductos(string filtro, bool soloBajoStock = false, bool soloSinStock = false, bool soloActivos = true)
        {
            var lista = new CN_Consultas().ConsultarProductos(
                filtro,
                soloBajoStock,
                soloSinStock,
                soloActivos
            );

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CLIENTES
        // ===============================

        [HttpGet]
        public JsonResult ConsultarClientes(string filtro, bool soloConSaldo = false, bool soloActivos = true)
        {
            var lista = new CN_Consultas().ConsultarClientes(
                filtro,
                soloConSaldo,
                soloActivos
            );

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // PROVEEDORES
        // ===============================

        [HttpGet]
        public JsonResult ConsultarProveedores(string filtro, bool soloConSaldo = false, bool soloActivos = true)
        {
            var lista = new CN_Consultas().ConsultarProveedores(
                filtro,
                soloConSaldo,
                soloActivos
            );

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // MOVIMIENTOS CONTABLES
        // ===============================

        [HttpGet]
        public JsonResult ConsultarMovimientosContables(string filtro, string fechaInicio, string fechaFin, string tipoMovimiento)
        {
            DateTime? inicio;
            DateTime? fin;
            string mensaje = string.Empty;

            if (!new CN_Consultas().ValidarFechasTexto(fechaInicio, fechaFin, out inicio, out fin, out mensaje))
            {
                return Json(new
                {
                    resultado = false,
                    mensaje = mensaje,
                    data = new object[0]
                }, JsonRequestBehavior.AllowGet);
            }

            var lista = new CN_Consultas().ConsultarMovimientosContables(
                filtro,
                inicio,
                fin,
                tipoMovimiento
            );

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }
    }
}