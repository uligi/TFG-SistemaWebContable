using CapaEntidad.Seguridad;
using CapaNegocio.Seguridad;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Mantenimientos
{
    public class SeguridadController : Controller
    {
        public ActionResult Rol()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarRoles()
        {
            var lista = new CN_Rol().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

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
        public JsonResult InactivarRol(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Rol().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}