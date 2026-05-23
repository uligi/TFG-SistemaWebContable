using CapaNegocio.Reportes;
using System;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Reportes
{
    public class ReportesController : Controller
    {
        public ActionResult Reportes()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ResumenFinanciero(string fechaInicio, string fechaFin)
        {
            string mensaje = string.Empty;

            DateTime inicio;
            DateTime fin;

            if (!DateTime.TryParse(fechaInicio, out inicio))
            {
                return Json(new { data = new { }, mensaje = "Debe ingresar una fecha de inicio válida." }, JsonRequestBehavior.AllowGet);
            }

            if (!DateTime.TryParse(fechaFin, out fin))
            {
                return Json(new { data = new { }, mensaje = "Debe ingresar una fecha final válida." }, JsonRequestBehavior.AllowGet);
            }

            var resumen = new CN_Reportes().ResumenFinanciero(inicio, fin, out mensaje);

            return Json(new { data = resumen, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ReporteIngresos(string fechaInicio, string fechaFin)
        {
            string mensaje = string.Empty;

            DateTime inicio;
            DateTime fin;

            if (!DateTime.TryParse(fechaInicio, out inicio))
            {
                return Json(new { data = new object[] { }, mensaje = "Debe ingresar una fecha de inicio válida." }, JsonRequestBehavior.AllowGet);
            }

            if (!DateTime.TryParse(fechaFin, out fin))
            {
                return Json(new { data = new object[] { }, mensaje = "Debe ingresar una fecha final válida." }, JsonRequestBehavior.AllowGet);
            }

            var lista = new CN_Reportes().ReporteIngresos(inicio, fin, out mensaje);

            return Json(new { data = lista, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ReporteGastos(string fechaInicio, string fechaFin)
        {
            string mensaje = string.Empty;

            DateTime inicio;
            DateTime fin;

            if (!DateTime.TryParse(fechaInicio, out inicio))
            {
                return Json(new { data = new object[] { }, mensaje = "Debe ingresar una fecha de inicio válida." }, JsonRequestBehavior.AllowGet);
            }

            if (!DateTime.TryParse(fechaFin, out fin))
            {
                return Json(new { data = new object[] { }, mensaje = "Debe ingresar una fecha final válida." }, JsonRequestBehavior.AllowGet);
            }

            var lista = new CN_Reportes().ReporteGastos(inicio, fin, out mensaje);

            return Json(new { data = lista, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ReporteCuentasPorCobrar()
        {
            var lista = new CN_Reportes().ReporteCuentasPorCobrar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ReporteCuentasPorPagar()
        {
            var lista = new CN_Reportes().ReporteCuentasPorPagar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}