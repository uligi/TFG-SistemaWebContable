using CapaEntidad.Seguridad;
using CapaNegocio.Seguridad;

using CapaPresentacion.Filtros;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Mantenimientos
{
    public class SeguridadController : Controller
    {
        [PermisoAuthorize(CodigoModulo = "ROLES")]
    

        [HttpPost]
        public JsonResult GuardarRol(Rol obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdRol == 0)
            {
                resultado = new CN_Rol().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Rol().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarRol(int idRol)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Rol().Inactivar(idRol, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [PermisoAuthorize(CodigoModulo = "HISTORIAL_CAMBIOS")]
        public ActionResult HistorialCambios()
        {
            return View();
        }

        [HttpGet]
        [PermisoAuthorize(CodigoModulo = "HISTORIAL_CAMBIOS")]
        public JsonResult ListarHistorialCambios(string filtro, string fechaInicio, string fechaFin, string tipoDeObservacion)
        {
            string mensaje = string.Empty;

            var lista = new CN_Seguridad().ListarHistorialCambios(
                filtro,
                fechaInicio,
                fechaFin,
                tipoDeObservacion,
                out mensaje
            );

            if (!string.IsNullOrWhiteSpace(mensaje))
            {
                return Json(new
                {
                    resultado = false,
                    mensaje = mensaje,
                    data = new object[0]
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                resultado = true,
                mensaje = string.Empty,
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [PermisoAuthorize(CodigoModulo = "ROLES")]
        public ActionResult Rol()
        {
            return View();
        }

        [PermisoAuthorize(CodigoModulo = "ROLES")]
        public ActionResult RolPermiso()
        {
            return View();
        }

        [HttpGet]
        [PermisoAuthorize(CodigoModulo = "ROLES")]
        public JsonResult ListarRoles()
        {
            var lista = new CN_Rol().Listar();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [PermisoAuthorize(CodigoModulo = "ROLES")]
        public JsonResult ListarPermisosPorRol(int idRol)
        {
            var lista = new CN_Seguridad().ListarPermisosPorRol(idRol);

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PermisoAuthorize(CodigoModulo = "ROLES")]
        public JsonResult GuardarPermisoRolModulo(PermisoRolModulo obj)
        {
            string mensaje = string.Empty;

            string identificacionEmpleado = Session["IdentificacionEmpleado"] == null
                ? "000000000"
                : Session["IdentificacionEmpleado"].ToString();

            bool resultado = new CN_Seguridad().GuardarPermisoRolModulo(
                obj,
                identificacionEmpleado,
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