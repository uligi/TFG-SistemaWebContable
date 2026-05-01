using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Mantenimientos
{
    public class CatalogosController : Controller
    {
        // GET: Catalogos
        public ActionResult TipoTelefono()
        {
            return View();
        }
        public ActionResult TipoCorreo()
        {
            return View();
        }

        public ActionResult TipoFactura()
        {
            return View();
        }

        public ActionResult TipoDeObservacion()
        {
            return View();
        }

        public ActionResult TipoPago()
        {
            return View();
        }

        public ActionResult TipoProducto()
        {
            return View();
        }


    }
}