
using CapaEntidad.personas;
using CapaEntidad.Personas;
using CapaNegocio.personas;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Mantenimientos
{
    public class PersonaController : Controller
    {
        public ActionResult Clientes()
        {
            return View();
        }

        public ActionResult Empleados()
        {
            return View();
        }

        public ActionResult Telefono()
        {
            return View();
        }

        public ActionResult Correo()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarClientes()
        {
            var lista = new CN_Cliente().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCliente(Cliente obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.CodigoCliente))
            {
                resultado = new CN_Cliente().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Cliente().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarCliente(string identificacion)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Cliente().Inactivar(identificacion, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTelefonos()
        {
            var lista = new CN_Telefono().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTelefono(Telefono obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.Activo == false && string.IsNullOrWhiteSpace(obj.TipoTelefonoNombre))
            {
                obj.Activo = true;
            }

            if (Request.Form.Count == 0)
            {
                // Se usa Codigo lógico desde la vista:
                // si viene como nuevo, Activo se envía true y se registra.
            }

            bool existe = new CN_Telefono().Listar().Exists(x =>
                x.NumeroTelefono == obj.NumeroTelefono &&
                x.Identificacion == obj.Identificacion
            );

            if (!existe)
            {
                resultado = new CN_Telefono().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Telefono().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarTelefono(string numeroTelefono, string identificacion)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Telefono().Inactivar(numeroTelefono, identificacion, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTelefonosPorPersona(string identificacion)
        {
            var lista = new CN_Telefono().Listar()
                .FindAll(x => x.Identificacion == identificacion);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTelefonoDesdeCliente(Telefono obj, bool esNuevo, string numeroAnterior)
        {
            object resultado;
            string mensaje = string.Empty;

            if (esNuevo)
            {
                resultado = new CN_Telefono().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Telefono().EditarDesdeCliente(numeroAnterior, obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarTelefonoDesdeCliente(string numeroTelefono, string identificacion)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Telefono().Inactivar(numeroTelefono, identificacion, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCorreosPorPersona(string identificacion)
        {
            var lista = new CN_Correo().Listar()
                .FindAll(x => x.Identificacion == identificacion);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCorreoDesdeCliente(Correo obj, bool esNuevo, string correoAnterior)
        {
            object resultado;
            string mensaje = string.Empty;

            if (esNuevo)
            {
                resultado = new CN_Correo().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Correo().EditarDesdeCliente(correoAnterior, obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarCorreoDesdeCliente(string identificacion, string direccionCorreo)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Correo().Inactivar(identificacion, direccionCorreo, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        //**************Empleados****************

      

        [HttpGet]
        public JsonResult ListarEmpleados()
        {
            var lista = new CN_Empleado().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarEmpleado(Empleado obj)
        {
            object resultado;
            string mensaje = string.Empty;

            string claveTemporal = string.Empty;
            string nombreUsuario = string.Empty;
            string codigoEmpleado = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.CodigoEmpleado))
            {
                resultado = new CN_Empleado().Registrar(
                    obj,
                    out mensaje,
                    out claveTemporal,
                    out nombreUsuario,
                    out codigoEmpleado
                );
            }
            else
            {
                resultado = new CN_Empleado().Editar(obj, out mensaje);
            }

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje,
                claveTemporal = claveTemporal,
                nombreUsuario = nombreUsuario,
                codigoEmpleado = codigoEmpleado
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarEmpleado(string identificacion)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Empleado().Inactivar(identificacion, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RestablecerClaveEmpleado(string identificacion)
        {
            string mensaje = string.Empty;
            string claveTemporal = string.Empty;
            string nombreUsuario = string.Empty;

            bool resultado = new CN_Empleado().RestablecerClave(
                identificacion,
                out mensaje,
                out claveTemporal,
                out nombreUsuario
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje,
                claveTemporal = claveTemporal,
                nombreUsuario = nombreUsuario
            }, JsonRequestBehavior.AllowGet);
        }
    }
}