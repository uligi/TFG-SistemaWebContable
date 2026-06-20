using CapaNegocio.Reportes;
using CapaPresentacion.Filtros;
using System;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Contabilidad
{
    [PermisoAuthorize(CodigoModulo = "REPORTES")]
    public class ReportesController : Controller
    {
        public ActionResult Reportes()
        {
            return View();
        }

        // ===============================
        // RESUMEN FINANCIERO
        // ===============================

        [HttpGet]
        public JsonResult ObtenerResumenFinanciero(string fechaInicio, string fechaFin)
        {
            DateTime inicio;
            DateTime fin;
            string mensaje = string.Empty;

            if (!ConvertirFechas(fechaInicio, fechaFin, out inicio, out fin, out mensaje))
            {
                return Json(new
                {
                    resultado = false,
                    mensaje = mensaje,
                    data = new object()
                }, JsonRequestBehavior.AllowGet);
            }

            var obj = new CN_Reportes().ObtenerResumenFinanciero(inicio, fin);

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // INGRESOS
        // ===============================

        [HttpGet]
        public JsonResult ReporteIngresos(string fechaInicio, string fechaFin)
        {
            DateTime inicio;
            DateTime fin;
            string mensaje = string.Empty;

            if (!ConvertirFechas(fechaInicio, fechaFin, out inicio, out fin, out mensaje))
            {
                return Json(new
                {
                    resultado = false,
                    mensaje = mensaje,
                    data = new object[0]
                }, JsonRequestBehavior.AllowGet);
            }

            var lista = new CN_Reportes().ReporteIngresos(inicio, fin);

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // GASTOS
        // ===============================

        [HttpGet]
        public JsonResult ReporteGastos(string fechaInicio, string fechaFin)
        {
            DateTime inicio;
            DateTime fin;
            string mensaje = string.Empty;

            if (!ConvertirFechas(fechaInicio, fechaFin, out inicio, out fin, out mensaje))
            {
                return Json(new
                {
                    resultado = false,
                    mensaje = mensaje,
                    data = new object[0]
                }, JsonRequestBehavior.AllowGet);
            }

            var lista = new CN_Reportes().ReporteGastos(inicio, fin);

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
        public JsonResult ReporteCuentasPorCobrar()
        {
            var lista = new CN_Reportes().ReporteCuentasPorCobrar();

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
        public JsonResult ReporteCuentasPorPagar()
        {
            var lista = new CN_Reportes().ReporteCuentasPorPagar();

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // PRODUCTOS MÁS VENDIDOS
        // ===============================

        [HttpGet]
        public JsonResult ReporteProductosMasVendidos(string fechaInicio, string fechaFin)
        {
            DateTime inicio;
            DateTime fin;
            string mensaje = string.Empty;

            if (!ConvertirFechas(fechaInicio, fechaFin, out inicio, out fin, out mensaje))
            {
                return Json(new
                {
                    resultado = false,
                    mensaje = mensaje,
                    data = new object[0]
                }, JsonRequestBehavior.AllowGet);
            }

            var lista = new CN_Reportes().ReporteProductosMasVendidos(inicio, fin);

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CLIENTES CON MÁS VENTAS
        // ===============================

        [HttpGet]
        public JsonResult ReporteClientesMasVentas(string fechaInicio, string fechaFin)
        {
            DateTime inicio;
            DateTime fin;
            string mensaje = string.Empty;

            if (!ConvertirFechas(fechaInicio, fechaFin, out inicio, out fin, out mensaje))
            {
                return Json(new
                {
                    resultado = false,
                    mensaje = mensaje,
                    data = new object[0]
                }, JsonRequestBehavior.AllowGet);
            }

            var lista = new CN_Reportes().ReporteClientesMasVentas(inicio, fin);

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // FLUJO DIARIO
        // ===============================

        [HttpGet]
        public JsonResult ReporteFlujoDiario(string fechaInicio, string fechaFin)
        {
            DateTime inicio;
            DateTime fin;
            string mensaje = string.Empty;

            if (!ConvertirFechas(fechaInicio, fechaFin, out inicio, out fin, out mensaje))
            {
                return Json(new
                {
                    resultado = false,
                    mensaje = mensaje,
                    data = new object[0]
                }, JsonRequestBehavior.AllowGet);
            }

            var lista = new CN_Reportes().ReporteFlujoDiario(inicio, fin);

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // VALIDACIÓN INTERNA DE FECHAS
        // ===============================

        private bool ConvertirFechas(string fechaInicioTexto, string fechaFinTexto, out DateTime fechaInicio, out DateTime fechaFin, out string mensaje)
        {
            fechaInicio = DateTime.MinValue;
            fechaFin = DateTime.MinValue;
            mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(fechaInicioTexto))
            {
                mensaje = "Debe ingresar la fecha de inicio.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(fechaFinTexto))
            {
                mensaje = "Debe ingresar la fecha final.";
                return false;
            }

            if (!DateTime.TryParse(fechaInicioTexto, out fechaInicio))
            {
                mensaje = "La fecha de inicio no tiene un formato válido.";
                return false;
            }

            if (!DateTime.TryParse(fechaFinTexto, out fechaFin))
            {
                mensaje = "La fecha final no tiene un formato válido.";
                return false;
            }

            fechaInicio = fechaInicio.Date;
            fechaFin = fechaFin.Date;

            return new CN_Reportes().ValidarFechas(fechaInicio, fechaFin, out mensaje);
        }
    }
}