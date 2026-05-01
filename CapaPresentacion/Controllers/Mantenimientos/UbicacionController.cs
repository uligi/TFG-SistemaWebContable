using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Mantenimientos
{
    public class UbicacionController : Controller
    {
        // GET: Ubicacion
        public ActionResult Provincia()
        {
            return View();
        }

        public ActionResult Canton()
        {
            return View();
        }

        public ActionResult Distrito()
        {
            return View();
        }
    }
}