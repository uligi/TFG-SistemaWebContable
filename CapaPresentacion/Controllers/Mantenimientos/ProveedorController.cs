using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad.Proveedores;
using CapaNegocio.Proveedores;


namespace CapaPresentacion.Controllers.Mantenimientos
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor
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
        public JsonResult GuardarProveedor(Proveedor obj, bool esNuevo)
        {
            object resultado;
            string mensaje = string.Empty;
            string identificacionProveedorGenerada = string.Empty;

            if (esNuevo)
            {
                resultado = new CN_Proveedor().Registrar(
                    obj,
                    out mensaje,
                    out identificacionProveedorGenerada
                );
            }
            else
            {
                resultado = new CN_Proveedor().Editar(obj, out mensaje);
            }

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje,
                identificacionProveedorGenerada = identificacionProveedorGenerada
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarProveedor(string identificacionProveedor)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Proveedor().Inactivar(identificacionProveedor, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TelefonoProveedor()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarTelefonosProveedor(string identificacionProveedor)
        {
            var lista = new CN_TelefonoProveedor().Listar()
                .FindAll(x => x.IdentificacionProveedor == identificacionProveedor);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTelefonoProveedor(TelefonoProveedor obj, bool esNuevo, string numeroAnterior)
        {
            object resultado;
            string mensaje = string.Empty;

            if (esNuevo)
            {
                resultado = new CN_TelefonoProveedor().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_TelefonoProveedor().Editar(numeroAnterior, obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
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

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CorreoProveedor()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarCorreosProveedor(string identificacionProveedor)
        {
            var lista = new CN_CorreoProveedor().Listar()
                .FindAll(x => x.IdentificacionProveedor == identificacionProveedor);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCorreoProveedor(CorreoProveedor obj, bool esNuevo, string correoAnterior)
        {
            object resultado;
            string mensaje = string.Empty;

            if (esNuevo)
            {
                resultado = new CN_CorreoProveedor().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_CorreoProveedor().Editar(correoAnterior, obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
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

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}