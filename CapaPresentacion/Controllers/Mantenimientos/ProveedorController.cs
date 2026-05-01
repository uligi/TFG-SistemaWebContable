using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Mantenimientos
{
    public class ProveedorController : Controller
    {
        // GET: Proveedor
        public ActionResult Proveedores()
        {
            return View();
        }

        public ActionResult TelefonoProveedor()
        {
            return View();
        }

        public ActionResult CorreoProveedor()
        {
            return View();
        }
    }
}