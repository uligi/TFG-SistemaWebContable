using CapaEntidad.Ubicacion;
using CapaNegocio.Ubicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web;

namespace CapaPresentacion.Controllers.Mantenimientos
{
    public class UbicacionController : Controller
    {
        // GET: ********************Provincia*********************
        public ActionResult Provincia()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarProvincias()
        {
            var lista = new CN_Provincia().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarProvinciasActivas()
        {
            var lista = new CN_Provincia().ListarActivas();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProvincia(Provincia obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdProvincia == 0)
            {
                resultado = new CN_Provincia().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Provincia().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarProvincia(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Provincia().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: ********************Canton*********************
        public ActionResult Canton()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarCantones()
        {
            var lista = new CN_Canton().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCanton(Canton obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdCanton == 0)
            {
                resultado = new CN_Canton().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Canton().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarCanton(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Canton().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: ********************Distrito*********************
        public ActionResult Distrito()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarDistritos()
        {
            var lista = new CN_Distrito().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCantonesActivosPorProvincia(int idProvincia)
        {
            var lista = new CN_Canton().ListarActivosPorProvincia(idProvincia);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarDistrito(Distrito obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdDistrito == 0)
            {
                resultado = new CN_Distrito().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Distrito().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarDistrito(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Distrito().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDistritosActivosPorCanton(int idCanton)
        {
            var lista = new CN_Distrito().ListarActivosPorCanton(idCanton);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CargarCsvProvincias(HttpPostedFileBase archivo)
        {
            string mensaje = string.Empty;

            if (archivo == null || archivo.ContentLength == 0)
            {
                return Json(new { resultado = false, mensaje = "Debe seleccionar un archivo CSV." });
            }

            if (!archivo.FileName.ToLower().EndsWith(".csv"))
            {
                return Json(new { resultado = false, mensaje = "El archivo debe tener formato CSV." });
            }

            var resultado = new CN_Provincia().CargarCsv(archivo.InputStream, out mensaje);

            return Json(new { resultado = true, mensaje = mensaje, data = resultado });
        }

        [HttpPost]
        public JsonResult CargarCsvCantones(HttpPostedFileBase archivo)
        {
            string mensaje = string.Empty;

            if (archivo == null || archivo.ContentLength == 0)
            {
                return Json(new { resultado = false, mensaje = "Debe seleccionar un archivo CSV." });
            }

            if (!archivo.FileName.ToLower().EndsWith(".csv"))
            {
                return Json(new { resultado = false, mensaje = "El archivo debe tener formato CSV." });
            }

            var resultado = new CN_Canton().CargarCsv(archivo.InputStream, out mensaje);

            return Json(new { resultado = true, mensaje = mensaje, data = resultado });
        }

        [HttpPost]
        public JsonResult CargarCsvDistritos(HttpPostedFileBase archivo)
        {
            string mensaje = string.Empty;

            if (archivo == null || archivo.ContentLength == 0)
            {
                return Json(new { resultado = false, mensaje = "Debe seleccionar un archivo CSV." });
            }

            if (!archivo.FileName.ToLower().EndsWith(".csv"))
            {
                return Json(new { resultado = false, mensaje = "El archivo debe tener formato CSV." });
            }

            var resultado = new CN_Distrito().CargarCsv(archivo.InputStream, out mensaje);

            return Json(new { resultado = true, mensaje = mensaje, data = resultado });
        }
    }
}