using CapaEntidad.Facturacion;
using CapaEntidad.Personas;
using CapaNegocio.Catalogos;
using CapaNegocio.Facturacion;
using CapaNegocio.Inventario;
using CapaNegocio.personas;
using CapaPresentacion.Filtros;
using System.Linq;
using System.Web.Mvc;


namespace CapaPresentacion.Controllers.Facturacion
{
    [PermisoAuthorize(CodigoModulo = "FACTURAS")]
    public class FacturacionController : Controller
    {
        public ActionResult Facturas()
        {
            var usuarioSesion = Session["UsuarioSesion"] as CapaEntidad.Seguridad.UsuarioSesion;

            if (usuarioSesion != null &&
                !string.IsNullOrWhiteSpace(usuarioSesion.NombreRol) &&
                usuarioSesion.NombreRol.ToLower().Contains("cajero"))
            {
                return RedirectToAction("Caja");
            }

            return View();
        }

        public ActionResult Caja()
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

        [HttpGet]
        public JsonResult ListarFacturasActivas()
        {
            var lista = new CN_Factura().ListarActivas();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerFactura(string numeroFactura)
        {
            var obj = new CN_Factura().Obtener(numeroFactura);
            return Json(new { data = obj }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarFacturasPorCliente(string identificacionCliente)
        {
            var lista = new CN_Factura().ListarPorCliente(identificacionCliente);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarFactura(Factura obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.NumeroFactura))
            {
                string numeroFacturaGenerado = string.Empty;

                resultado = new CN_Factura().Registrar(
                    obj,
                    out mensaje,
                    out numeroFacturaGenerado
                );

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroFactura = numeroFacturaGenerado
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultado = new CN_Factura().Editar(obj, out mensaje);

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroFactura = obj.NumeroFactura
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult InactivarFactura(string numeroFactura)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_Factura().Inactivar(
                numeroFactura,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmitirFactura(string numeroFactura)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_Factura().Emitir(numeroFactura, out mensaje);

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CambiarEstadoFactura(string numeroFactura, string estado)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_Factura().CambiarEstado(
                numeroFactura,
                estado,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RecalcularTotalesFactura(string numeroFactura)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_Factura().RecalcularTotales(
                numeroFactura,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarClientesFactura(string filtro)
        {
            filtro = string.IsNullOrWhiteSpace(filtro) ? "" : filtro.Trim().ToLower();

            var lista = new CN_Cliente().Listar()
                .FindAll(x =>
                    x.Activo == true &&
                    (
                        x.Identificacion.ToLower().Contains(filtro) ||
                        x.Nombre.ToLower().Contains(filtro) ||
                        x.PrimerApellido.ToLower().Contains(filtro) ||
                        (!string.IsNullOrEmpty(x.SegundoApellido) && x.SegundoApellido.ToLower().Contains(filtro))
                    )
                )
                .Take(10)
                .ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarEmpleadosFactura(string filtro)
        {
            filtro = string.IsNullOrWhiteSpace(filtro) ? "" : filtro.Trim().ToLower();

            var lista = new CN_Empleado().Listar()
                .FindAll(x =>
                    x.Activo == true &&
                    (
                        x.Identificacion.ToLower().Contains(filtro) ||
                        x.Nombre.ToLower().Contains(filtro) ||
                        x.PrimerApellido.ToLower().Contains(filtro) ||
                        (!string.IsNullOrEmpty(x.SegundoApellido) && x.SegundoApellido.ToLower().Contains(filtro))
                    )
                )
                .Take(10)
                .ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // DETALLE FACTURA
        // ===============================

        [HttpGet]
        public JsonResult ListarDetallesFactura(string numeroFactura)
        {
            var lista = new CN_DetalleFactura().ListarPorFactura(numeroFactura);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerDetalleFactura(string numeroFactura, string codigoProducto)
        {
            var obj = new CN_DetalleFactura().Obtener(
                numeroFactura,
                codigoProducto
            );

            return Json(new { data = obj }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarDetalleFactura(DetalleFactura obj)
        {
            object resultado;
            string mensaje = string.Empty;

            var detalleExistente = new CN_DetalleFactura().Obtener(
                obj.NumeroFactura,
                obj.CodigoProducto
            );

            if (detalleExistente == null)
            {
                resultado = new CN_DetalleFactura().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_DetalleFactura().Editar(obj, out mensaje);
            }

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarDetalleFactura(string numeroFactura, string codigoProducto)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_DetalleFactura().Inactivar(
                numeroFactura,
                codigoProducto,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // NOTA CRÉDITO
        // ===============================
        public ActionResult NotasCredito()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarNotasCredito()
        {
            var lista = new CN_NotaCredito().Listar();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarNotasCreditoActivas()
        {
            var lista = new CN_NotaCredito().ListarActivas();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarNotasCreditoPorFactura(string numeroFactura)
        {
            var lista = new CN_NotaCredito().ListarPorFactura(numeroFactura);

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerNotaCredito(string numeroNotaCredito)
        {
            var obj = new CN_NotaCredito().Obtener(numeroNotaCredito);

            return Json(new
            {
                data = obj
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarNotaCredito(NotaCredito obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.NumeroNotaCredito))
            {
                string numeroNotaCreditoGenerado = string.Empty;

                resultado = new CN_NotaCredito().Registrar(
                    obj,
                    out mensaje,
                    out numeroNotaCreditoGenerado
                );

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroNotaCredito = numeroNotaCreditoGenerado
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultado = new CN_NotaCredito().Editar(obj, out mensaje);

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    numeroNotaCredito = obj.NumeroNotaCredito
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult InactivarNotaCredito(string numeroNotaCredito)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_NotaCredito().Inactivar(
                numeroNotaCredito,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarFacturasParaNotaCredito(string filtro)
        {
            filtro = string.IsNullOrWhiteSpace(filtro) ? "" : filtro.Trim().ToLower();

            var lista = new CN_Factura().Listar()
                .Where(x =>
                    x.Activo == true &&
                    x.Estado == "Emitida" &&
                    x.TotalFactura > 0 &&
                    (
                        x.NumeroFactura.ToLower().Contains(filtro) ||
                        x.IdentificacionCliente.ToLower().Contains(filtro) ||
                        (!string.IsNullOrEmpty(x.ClienteNombre) && x.ClienteNombre.ToLower().Contains(filtro))
                    )
                )
                .Take(10)
                .ToList();

            return Json(new
            {
                data = lista
            }, JsonRequestBehavior.AllowGet);
        }

        // ===============================
        // CATÁLOGOS PARA FACTURACIÓN
        // ===============================

        [HttpGet]
        public JsonResult ListarTiposPagoActivos()
        {
            var lista = new CN_TipoPago().Listar()
                .Where(x => x.Activo)
                .ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTiposFacturaActivos()
        {
            var lista = new CN_TipoFactura().Listar()
                .Where(x => x.Activo)
                .ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDescuentosActivos()
        {
            var lista = new CN_Descuento().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarImpuestosActivos()
        {
            var lista = new CN_Impuesto().ListarActivos();
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


        //-------------CAJA---------------

        [HttpPost]
        public JsonResult RegistrarClienteCaja(Cliente obj, string telefono, string correo)
        {
            string mensaje = string.Empty;

            if (obj == null)
            {
                return Json(new
                {
                    resultado = 0,
                    mensaje = "Debe ingresar los datos del cliente."
                }, JsonRequestBehavior.AllowGet);
            }

            obj.LimiteCredito = 0;
            obj.DiasCredito = 0;
            obj.Activo = true;

            int resultadoCliente = new CN_Cliente().Registrar(obj, out mensaje);

            if (resultadoCliente == 0)
            {
                return Json(new
                {
                    resultado = 0,
                    mensaje = mensaje
                }, JsonRequestBehavior.AllowGet);
            }

            string mensajeTelefono = string.Empty;
            string mensajeCorreo = string.Empty;

            if (!string.IsNullOrWhiteSpace(telefono))
            {
                Telefono objTelefono = new Telefono
                {
                    Identificacion = obj.Identificacion,
                    NumeroTelefono = telefono.Trim(),
                    NumeroTelefonoAnterior = "",
                    IdTipoTelefono = 1,
                    EsPrincipal = true,
                    Activo = true
                };

                new CN_Telefono().Registrar(objTelefono, out mensajeTelefono);
            }

            if (!string.IsNullOrWhiteSpace(correo))
            {
                Correo objCorreo = new Correo
                {
                    Identificacion = obj.Identificacion,
                    DireccionCorreo = correo.Trim(),
                    DireccionCorreoAnterior = "",
                    IdTipoCorreo = 1,
                    EsPrincipal = true,
                    Activo = true
                };

                new CN_Correo().Registrar(objCorreo, out mensajeCorreo);
            }

            return Json(new
            {
                resultado = resultadoCliente,
                mensaje = "Cliente registrado correctamente.",
                cliente = obj,
                telefono = telefono,
                correo = correo
            }, JsonRequestBehavior.AllowGet);
        }
    }
}