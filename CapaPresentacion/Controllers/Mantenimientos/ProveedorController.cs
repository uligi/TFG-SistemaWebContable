using CapaEntidad.Proveedores;
using CapaNegocio.Proveedores;
using CapaPresentacion.Filtros;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Mantenimientos
{
    [PermisoAuthorize(CodigoModulo = "PROVEEDORES")]
    public class ProveedorController : Controller
    {
        // ===============================
        // PROVEEDORES
        // ===============================

        public ActionResult Proveedores()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarProveedores()
        {
            var lista = new CN_Proveedor().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarProveedoresActivos()
        {
            var lista = new CN_Proveedor().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProveedor(Proveedor obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.IdentificacionProveedor))
            {
                string identificacionProveedorGenerada;

                resultado = new CN_Proveedor().Registrar(
                    obj,
                    out mensaje,
                    out identificacionProveedorGenerada
                );

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    identificacionProveedor = identificacionProveedorGenerada
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultado = new CN_Proveedor().Editar(obj, out mensaje);

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    identificacionProveedor = obj.IdentificacionProveedor
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult InactivarProveedor(string identificacionProveedor)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Proveedor().Inactivar(identificacionProveedor, out mensaje);

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // TELEFONOS PROVEEDOR
        // ===============================

        [HttpGet]
        public JsonResult ListarTelefonosProveedor()
        {
            var lista = new CN_TelefonoProveedor().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTelefonosProveedorPorProveedor(string identificacionProveedor)
        {
            var lista = new CN_TelefonoProveedor().ListarPorProveedor(identificacionProveedor);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTelefonoProveedor(TelefonoProveedor obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.NumeroTelefonoAnterior))
            {
                resultado = new CN_TelefonoProveedor().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_TelefonoProveedor().Editar(obj, out mensaje);
            }

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarTelefonoProveedor(string identificacionProveedor, string numeroTelefono)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_TelefonoProveedor().Inactivar(
                identificacionProveedor,
                numeroTelefono,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CORREOS PROVEEDOR
        // ===============================

        [HttpGet]
        public JsonResult ListarCorreosProveedor()
        {
            var lista = new CN_CorreoProveedor().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCorreosProveedorPorProveedor(string identificacionProveedor)
        {
            var lista = new CN_CorreoProveedor().ListarPorProveedor(identificacionProveedor);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCorreoProveedor(CorreoProveedor obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.DireccionCorreoAnterior))
            {
                resultado = new CN_CorreoProveedor().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_CorreoProveedor().Editar(obj, out mensaje);
            }

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarCorreoProveedor(string identificacionProveedor, string direccionCorreo)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_CorreoProveedor().Inactivar(
                identificacionProveedor,
                direccionCorreo,
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