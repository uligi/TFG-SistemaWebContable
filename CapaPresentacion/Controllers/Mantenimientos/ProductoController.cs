using CapaEntidad.Inventario;
using CapaNegocio.Inventario;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Mantenimientos
{
    public class ProductoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Productos()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarProductos()
        {
            var lista = new CN_Producto().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTipoProductosActivos()
        {
            var lista = new CN_Producto().ListarTipoProductosActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(Producto obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdProducto == 0)
            {
                resultado = new CN_Producto().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Producto().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarProducto(int id)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Producto().Inactivar(id, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}