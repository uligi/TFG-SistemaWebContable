using CapaEntidad.Seguridad;
using CapaNegocio.Seguridad;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CapaPresentacion.Filtros
{
    public class PermisoAuthorizeAttribute : AuthorizeAttribute
    {
        public string CodigoModulo { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                return false;
            }

            UsuarioSesion usuario = httpContext.Session["UsuarioSesion"] as UsuarioSesion;

            if (usuario == null)
            {
                return false;
            }

            if (!usuario.Activo)
            {
                return false;
            }

            string controlador = httpContext.Request.RequestContext.RouteData.Values["controller"] == null
                ? ""
                : httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

            string accion = httpContext.Request.RequestContext.RouteData.Values["action"] == null
                ? ""
                : httpContext.Request.RequestContext.RouteData.Values["action"].ToString();

            if (controlador == "" || accion == "")
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(CodigoModulo))
            {
                return usuario.TienePermisoVer(CodigoModulo);
            }

            return new CN_Seguridad().ValidarAcceso(
                usuario.IdRol,
                controlador,
                accion
            );
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            UsuarioSesion usuario = filterContext.HttpContext.Session["UsuarioSesion"] as UsuarioSesion;

            if (usuario == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Acceso" },
                        { "action", "InicioSesion" }
                    }
                );

                return;
            }

            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "SinPermiso" }
                }
            );
        }
    }
}