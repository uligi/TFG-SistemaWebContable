using CapaEntidad.Seguridad;
using CapaPresentacion.Filtros;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers
{
    public class HomeController : Controller
    {
        [PermisoAuthorize(CodigoModulo = "HOME")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Mantenimientos()
        {
            UsuarioSesion usuario = Session["UsuarioSesion"] as UsuarioSesion;

            if (usuario == null)
            {
                return RedirectToAction("InicioSesion", "Acceso");
            }

            string[] permisosMantenimiento = new[]
            {
                "CATALOGOS",
                "PRODUCTOS",
                "CLIENTES",
                "EMPLEADOS",
                "PROVEEDORES",
                "UBICACIONES",
                "ROLES",
                "CONFIG_CUENTA_CONTABLE",
                "PERIODO_CONTABLE"
            };

            bool tienePermiso = usuario.Permisos != null &&
                usuario.Permisos.Any(p =>
                    permisosMantenimiento.Contains(p.CodigoModulo)
                    && p.Activo
                    && p.PuedeVer
                );

            if (!tienePermiso)
            {
                return RedirectToAction("SinPermiso", "Home");
            }

            return View();
        }

        public ActionResult SinPermiso()
        {
            return View();
        }
    }
}