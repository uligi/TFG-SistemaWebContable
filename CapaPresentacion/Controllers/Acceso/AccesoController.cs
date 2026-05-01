using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Acceso
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult InicioSesion()
        {
            return View();
        }
    }
}