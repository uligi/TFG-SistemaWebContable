using CapaEntidad.Catalogos;
using CapaNegocio.Catalogos;
using CapaPresentacion.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CapaPresentacion.Controllers.Mantenimientos
{
    public class CatalogosController : Controller
    {
        [PermisoAuthorize(CodigoModulo = "CATALOGOS")]
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
        public JsonResult InactivarTipoTelefono(int idTipoTelefono)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoTelefono().Inactivar(idTipoTelefono, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [PermisoAuthorize(CodigoModulo = "CATALOGOS")]
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
        public JsonResult InactivarTipoCorreo(int idTipoCorreo)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoCorreo().Inactivar(idTipoCorreo, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [PermisoAuthorize(CodigoModulo = "CATALOGOS")]
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

        [HttpGet]
        public JsonResult ListarTipoFacturasActivos()
        {
            var lista = new CN_TipoFactura().Listar()
                .FindAll(x => x.Activo == true);

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
        public JsonResult InactivarTipoFactura(int idTipoFactura)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoFactura().Inactivar(idTipoFactura, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [PermisoAuthorize(CodigoModulo = "CATALOGOS")]
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

        [HttpGet]
        public JsonResult ListarTipoDeObservacionesActivos()
        {
            var lista = new CN_TipoDeObservacion().Listar()
                .FindAll(x => x.Activo == true);

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
        public JsonResult InactivarTipoDeObservacion(int idTipoDeObservacion)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoDeObservacion().Inactivar(idTipoDeObservacion, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [PermisoAuthorize(CodigoModulo = "CATALOGOS")]
        // GET: ********************TipoPago*********************
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

        [HttpGet]
        public JsonResult ListarTipoPagosActivos()
        {
            var lista = new CN_TipoPago().Listar()
                .FindAll(x => x.Activo == true);

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
        public JsonResult InactivarTipoPago(int idTipoPago)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoPago().Inactivar(idTipoPago, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // TIPO PRODUCTO
        // ===============================
        [PermisoAuthorize(CodigoModulo = "CATALOGOS")]
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

        [HttpGet]
        public JsonResult ListarTipoProductosActivos()
        {
            var lista = new CN_TipoProducto().ListarActivos();
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
        public JsonResult InactivarTipoProducto(int idTipoProducto)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoProducto().Inactivar(idTipoProducto, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // PUESTO
        // ===============================
        [PermisoAuthorize(CodigoModulo = "CATALOGOS")]
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
        public JsonResult InactivarPuesto(int idPuesto)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Puesto().Inactivar(idPuesto, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        // ===============================
        // NATURALEZA CUENTA CONTABLE
        // ===============================
        [PermisoAuthorize(CodigoModulo = "CATALOGOS")]
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
        public JsonResult InactivarNaturalezaCuentaContable(int idNaturalezaCuentaContable)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_NaturalezaCuentaContable().Inactivar(idNaturalezaCuentaContable, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        // ===============================
        // TIPO GASTO
        // ===============================
        [PermisoAuthorize(CodigoModulo = "TIPOS_GASTO")]
        public ActionResult TiposGasto()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarTipoGastos()
        {
            var lista = new CN_TipoGasto().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTipoGastosActivos()
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
        public JsonResult InactivarTipoGasto(int idTipoGasto)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoGasto().Inactivar(idTipoGasto, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        // ===============================
        // TIPO INGRESO
        // ===============================
        [PermisoAuthorize(CodigoModulo = "TIPOS_INGRESO")]
        public ActionResult TiposIngreso()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarTipoIngresos()
        {
            var lista = new CN_TipoIngreso().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTipoIngresosActivos()
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
        public JsonResult InactivarTipoIngreso(int idTipoIngreso)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoIngreso().Inactivar(idTipoIngreso, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        // ===============================
        // TIPO CUENTA CONTABLE
        // ===============================
        [PermisoAuthorize(CodigoModulo = "CATALOGOS")]
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
        public JsonResult InactivarTipoCuentaContable(int idTipoCuentaContable)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_TipoCuentaContable().Inactivar(idTipoCuentaContable, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        // ===============================
        // IMPUESTOS
        // ===============================
        [PermisoAuthorize(CodigoModulo = "CATALOGOS")]
        public ActionResult Impuestos()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarImpuestos()
        {
            var lista = new CN_Impuesto().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarImpuestosActivos()
        {
            var lista = new CN_Impuesto().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarImpuesto(Impuesto obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdImpuesto == 0)
            {
                resultado = new CN_Impuesto().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Impuesto().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarImpuesto(int idImpuesto)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Impuesto().Inactivar(idImpuesto, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        // ===============================
        // DESCUENTOS
        // ===============================

        public ActionResult Descuentos()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarDescuentos()
        {
            var lista = new CN_Descuento().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDescuentosActivos()
        {
            var lista = new CN_Descuento().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarDescuento(Descuento obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdDescuento == 0)
            {
                resultado = new CN_Descuento().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Descuento().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarDescuento(int idDescuento)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Descuento().Inactivar(idDescuento, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

    }
}