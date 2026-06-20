using CapaEntidad.Ubicacion;
using CapaNegocio.Ubicacion;
using CapaPresentacion.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Mantenimientos
{
    [PermisoAuthorize(CodigoModulo = "UBICACIONES")]
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

            bool existe = new CN_Provincia()
                .Listar()
                .Exists(p => p.CodigoProvincia == obj.CodigoProvincia);

            if (!existe)
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
        public JsonResult InactivarProvincia(int codigoProvincia)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Provincia().Inactivar(codigoProvincia, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
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
        // GET: ********************Canton*********************
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

            bool existe = new CN_Canton()
                .Listar()
                .Exists(c => c.CodigoCanton == obj.CodigoCanton);

            if (!existe)
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
        public JsonResult InactivarCanton(int codigoCanton)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Canton().Inactivar(codigoCanton, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCantonesActivosPorProvincia(int codigoProvincia = 0, int idProvincia = 0)
        {
            if (codigoProvincia == 0)
            {
                codigoProvincia = idProvincia;
            }

            var lista = new CN_Canton().ListarActivosPorProvincia(codigoProvincia);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult GuardarDistrito(Distrito obj)
        {
            object resultado;
            string mensaje = string.Empty;

            bool existe = new CN_Distrito()
                .Listar()
                .Exists(d => d.CodigoDistrito == obj.CodigoDistrito);

            if (!existe)
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
        public JsonResult InactivarDistrito(int codigoDistrito)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Distrito().Inactivar(codigoDistrito, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDistritosActivosPorCanton(int codigoCanton = 0, int idCanton = 0)
        {
            if (codigoCanton == 0)
            {
                codigoCanton = idCanton;
            }

            var lista = new CN_Distrito().ListarActivosPorCanton(codigoCanton);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
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