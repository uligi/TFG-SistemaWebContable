
using CapaEntidad.Personas;

using CapaNegocio.personas;
using CapaPresentacion.Filtros;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Mantenimientos
{
    public class PersonaController : Controller
    {



        //**************Clientes**********
        [PermisoAuthorize(CodigoModulo = "CLIENTES")]
        public ActionResult Clientes()
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

        // ===============================
        // TELEFONOS
        // ===============================

        [PermisoAuthorize(CodigoModulo = "CLIENTES")]
        public ActionResult Telefono()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ListarTelefonos()
        {
            var lista = new CN_Telefono().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTelefonosPorPersona(string identificacion)
        {
            var lista = new CN_Telefono().ListarPorPersona(identificacion);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarTelefono(Telefono obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.NumeroTelefonoAnterior))
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
        public JsonResult InactivarTelefono(string identificacion, string numeroTelefono)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Telefono().Inactivar(identificacion, numeroTelefono, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CORREOS
        // ===============================
        [PermisoAuthorize(CodigoModulo = "CLIENTES")]
        public ActionResult Correo()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarCorreos()
        {
            var lista = new CN_Correo().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCorreosPorPersona(string identificacion)
        {
            var lista = new CN_Correo().ListarPorPersona(identificacion);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCorreo(Correo obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.DireccionCorreoAnterior))
            {
                resultado = new CN_Correo().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Correo().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarCorreo(string identificacion, string direccionCorreo)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Correo().Inactivar(identificacion, direccionCorreo, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // EMPLEADOS
        // ===============================
        [PermisoAuthorize(CodigoModulo = "EMPLEADOS")]
        public ActionResult Empleados()
        {
            return View();
        }

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

            if (string.IsNullOrWhiteSpace(obj.CodigoEmpleado))
            {
                string nombreUsuarioGenerado;
                string codigoEmpleadoGenerado;
                string claveTemporalGenerada;

                resultado = new CN_Empleado().Registrar(
                    obj,
                    out mensaje,
                    out nombreUsuarioGenerado,
                    out codigoEmpleadoGenerado,
                    out claveTemporalGenerada
                );

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    nombreUsuario = nombreUsuarioGenerado,
                    codigoEmpleado = codigoEmpleadoGenerado,
                    claveTemporal = claveTemporalGenerada
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultado = new CN_Empleado().Editar(obj, out mensaje);

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    nombreUsuario = obj.NombreUsuario,
                    codigoEmpleado = obj.CodigoEmpleado,
                    claveTemporal = ""
                }, JsonRequestBehavior.AllowGet);
            }
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
            string nombreUsuario = string.Empty;
            string claveTemporalGenerada = string.Empty;

            bool resultado = new CN_Empleado().RestablecerClave(
                identificacion,
                out mensaje,
                out nombreUsuario,
                out claveTemporalGenerada
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje,
                nombreUsuario = nombreUsuario,
                claveTemporal = claveTemporalGenerada
            }, JsonRequestBehavior.AllowGet);
        }

    }
    }
