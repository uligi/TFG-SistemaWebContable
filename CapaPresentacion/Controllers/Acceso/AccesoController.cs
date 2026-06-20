using CapaEntidad.Seguridad;
using CapaNegocio.Seguridad;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Acceso
{
    public class AccesoController : Controller
    {
        [HttpGet]
        public ActionResult InicioSesion()
        {
            if (Session["UsuarioSesion"] != null)
            {
                UsuarioSesion usuario = Session["UsuarioSesion"] as UsuarioSesion;

                if (usuario != null && usuario.RestablecerClave)
                {
                    return RedirectToAction("CambiarClave", "Acceso");
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InicioSesion(string NombreUsuario, string Clave)
        {
            string mensaje = string.Empty;

            UsuarioSesion usuario = new CN_Seguridad().IniciarSesion(
                NombreUsuario,
                Clave,
                out mensaje
            );

            if (usuario == null)
            {
                ViewBag.Mensaje = mensaje;
                return View();
            }

            Session["UsuarioSesion"] = usuario;
            Session["IdentificacionEmpleado"] = usuario.Identificacion;
            Session["NombreUsuario"] = usuario.NombreUsuario;
            Session["NombreCompleto"] = usuario.NombreCompleto;
            Session["IdRol"] = usuario.IdRol;
            Session["NombreRol"] = usuario.NombreRol;

            if (usuario.RestablecerClave)
            {
                TempData["MensajeCambioClave"] = "Debe cambiar su contraseña temporal antes de ingresar al sistema.";
                return RedirectToAction("CambiarClave", "Acceso");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CambiarClave()
        {
            UsuarioSesion usuario = Session["UsuarioSesion"] as UsuarioSesion;

            if (usuario == null)
            {
                return RedirectToAction("InicioSesion", "Acceso");
            }

            ViewBag.MensajeInfo = TempData["MensajeCambioClave"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarClave(string ClaveActual, string ClaveNueva, string ConfirmarClave)
        {
            UsuarioSesion usuario = Session["UsuarioSesion"] as UsuarioSesion;

            if (usuario == null)
            {
                return RedirectToAction("InicioSesion", "Acceso");
            }

            string mensaje = string.Empty;

            bool resultado = new CN_Seguridad().CambiarClavePropia(
                usuario.Identificacion,
                ClaveActual,
                ClaveNueva,
                ConfirmarClave,
                out mensaje
            );

            if (!resultado)
            {
                ViewBag.Mensaje = mensaje;
                return View();
            }

            usuario.RestablecerClave = false;
            Session["UsuarioSesion"] = usuario;

            TempData["MensajeExito"] = "Contraseña actualizada correctamente.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult CerrarSesion()
        {
            UsuarioSesion usuario = Session["UsuarioSesion"] as UsuarioSesion;

            if (usuario != null)
            {
                new CN_Seguridad().RegistrarCierreSesion(usuario.Identificacion);
            }

            Session.Clear();
            Session.Abandon();

            return RedirectToAction("InicioSesion", "Acceso");
        }
    }
}