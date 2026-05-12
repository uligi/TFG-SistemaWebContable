using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad.Catalogos;
using CapaNegocio.Catalogos;


namespace CapaPresentacion.Controllers.Mantenimientos
{
    public class CatalogosController : Controller
    {
        // GET: ********************TipoTelefono*********************
        public ActionResult TipoTelefono()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarTipoTelefonos()
        {
            var lista = new CN_TipoTelefono().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTipoTelefono(TipoTelefono obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdTipoTelefono == 0)
            {
                resultado = new CN_TipoTelefono().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_TipoTelefono().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarTipoTelefono(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoTelefono().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: ********************TipoCorreo*********************
        public ActionResult TipoCorreo()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarTipoCorreos()
        {
            var lista = new CN_TipoCorreo().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTipoCorreo(TipoCorreo obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdTipoCorreo == 0)
            {
                resultado = new CN_TipoCorreo().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_TipoCorreo().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarTipoCorreo(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoCorreo().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: ********************TipoFactura*********************

        public ActionResult TipoFactura()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarTipoFacturas()
        {
            var lista = new CN_TipoFactura().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTipoFactura(TipoFactura obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdTipoFactura == 0)
            {
                resultado = new CN_TipoFactura().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_TipoFactura().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarTipoFactura(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoFactura().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        // GET: ********************TipoDeObservacion*********************
        public ActionResult TipoDeObservacion()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarTipoDeObservaciones()
        {
            var lista = new CN_TipoDeObservacion().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTipoDeObservacion(TipoDeObservacion obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdTipoDeObservacion == 0)
            {
                resultado = new CN_TipoDeObservacion().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_TipoDeObservacion().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarTipoDeObservacion(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoDeObservacion().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        // GET: ********************TipoProducto*********************
        public ActionResult TipoPago()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarTipoPagos()
        {
            var lista = new CN_TipoPago().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTipoPago(TipoPago obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdTipoPago == 0)
            {
                resultado = new CN_TipoPago().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_TipoPago().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarTipoPago(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoPago().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: ********************TipoProducto*********************
        public ActionResult TipoProducto()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarTipoProductos()
        {
            var lista = new CN_TipoProducto().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTipoProducto(TipoProducto obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdTipoProducto == 0)
            {
                resultado = new CN_TipoProducto().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_TipoProducto().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarTipoProducto(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoProducto().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

    
        // ===============================
        // PUESTO
        // ===============================

        public ActionResult Puestos()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarPuestos()
        {
            var lista = new CN_Puesto().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarPuestosActivos()
        {
            var lista = new CN_Puesto().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarPuesto(Puesto obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdPuesto == 0)
            {
                resultado = new CN_Puesto().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Puesto().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarPuesto(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Puesto().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        // ===============================
        // NATURALEZA CUENTA CONTABLE
        // ===============================

        public ActionResult NaturalezasCuentaContable()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarNaturalezasCuentaContable()
        {
            var lista = new CN_NaturalezaCuentaContable().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarNaturalezasCuentaContableActivas()
        {
            var lista = new CN_NaturalezaCuentaContable().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarNaturalezaCuentaContable(NaturalezaCuentaContable obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdNaturalezaCuentaContable == 0)
            {
                resultado = new CN_NaturalezaCuentaContable().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_NaturalezaCuentaContable().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarNaturalezaCuentaContable(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_NaturalezaCuentaContable().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        // ===============================
        // TIPO GASTO
        // ===============================

        public ActionResult TiposGasto()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarTiposGasto()
        {
            var lista = new CN_TipoGasto().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTiposGastoActivos()
        {
            var lista = new CN_TipoGasto().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTipoGasto(TipoGasto obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdTipoGasto == 0)
            {
                resultado = new CN_TipoGasto().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_TipoGasto().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarTipoGasto(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoGasto().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        // ===============================
        // TIPO INGRESO
        // ===============================

        public ActionResult TiposIngreso()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarTiposIngreso()
        {
            var lista = new CN_TipoIngreso().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTiposIngresoActivos()
        {
            var lista = new CN_TipoIngreso().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTipoIngreso(TipoIngreso obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdTipoIngreso == 0)
            {
                resultado = new CN_TipoIngreso().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_TipoIngreso().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarTipoIngreso(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoIngreso().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        // ===============================
        // TIPO CUENTA CONTABLE
        // ===============================

        public ActionResult TiposCuentaContable()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarTiposCuentaContable()
        {
            var lista = new CN_TipoCuentaContable().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTiposCuentaContableActivos()
        {
            var lista = new CN_TipoCuentaContable().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTipoCuentaContable(TipoCuentaContable obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdTipoCuentaContable == 0)
            {
                resultado = new CN_TipoCuentaContable().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_TipoCuentaContable().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarTipoCuentaContable(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoCuentaContable().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}