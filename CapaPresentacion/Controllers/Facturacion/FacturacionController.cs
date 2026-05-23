using CapaEntidad.Facturacion;
using CapaNegocio.Facturacion;
using CapaNegocio.personas;
using CapaNegocio.Catalogos;
using CapaNegocio.Inventario;
using System.Linq;

using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Facturacion
{
    public class FacturacionController : Controller
    {
        public ActionResult Facturas()
        {
            return View();
        }

        // ===============================
        // FACTURA
        // ===============================

        [HttpGet]
        public JsonResult ListarFacturas()
        {
            var lista = new CN_Factura().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarFactura(Factura obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdFactura == 0)
            {
                resultado = new CN_Factura().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Factura().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarFactura(int idFactura)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_Factura().Inactivar(
                idFactura,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmitirFactura(int idFactura)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_Factura().Emitir(
                idFactura,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarClientesFactura(string filtro)
        {
            filtro = string.IsNullOrWhiteSpace(filtro) ? "" : filtro.Trim().ToLower();

            var lista = new CapaNegocio.personas.CN_Cliente().Listar()
                .FindAll(x =>
                    x.Activo == true &&
                    (
                        x.Identificacion.ToLower().Contains(filtro) ||
                        x.Nombre.ToLower().Contains(filtro) ||
                        x.PrimerApellido.ToLower().Contains(filtro) ||
                        (!string.IsNullOrEmpty(x.SegundoApellido) && x.SegundoApellido.ToLower().Contains(filtro))
                    )
                );

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        // ===============================
        // DETALLE FACTURA
        // ===============================

        [HttpGet]
        public JsonResult ListarDetallesFactura(int idFactura)
        {
            var lista = new CN_DetalleFactura().ListarPorFactura(idFactura);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarDetalleFactura(DetalleFactura obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdDetalleFactura == 0)
            {
                resultado = new CN_DetalleFactura().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_DetalleFactura().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarDetalleFactura(int idDetalleFactura)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_DetalleFactura().Inactivar(
                idDetalleFactura,
                out mensaje
            );

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDescuentosActivos()
        {
            var lista = new CN_Descuento().ListarActivos();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarProductosFactura(string filtro)
        {
            filtro = string.IsNullOrWhiteSpace(filtro) ? "" : filtro.Trim().ToLower();

            var lista = new CN_Producto().Listar()
                .Where(x =>
                    x.Activo == true &&
                    x.StockActual > 0 &&
                    (
                        x.CodigoProducto.ToLower().Contains(filtro) ||
                        x.NombreProducto.ToLower().Contains(filtro)
                    )
                )
                .Take(10)
                .ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}