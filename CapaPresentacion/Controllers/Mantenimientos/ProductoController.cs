using CapaEntidad.Inventario;
using CapaNegocio.Catalogos;
using CapaNegocio.Inventario;
using CapaPresentacion.Filtros;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacion.Controllers.Mantenimientos
{
    [PermisoAuthorize(CodigoModulo = "PRODUCTOS")]
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
        public JsonResult ListarProductosActivos()
        {
            var lista = new CN_Producto().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerProducto(string codigoProducto)
        {
            var obj = new CN_Producto().Obtener(codigoProducto);
            return Json(new { data = obj }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarTipoProductosActivos()
        {
            var lista = new CN_TipoProducto().Listar()
                .Where(x => x.Activo)
                .ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarImpuestosActivos()
        {
            var lista = new CN_Impuesto().ListarActivos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(Producto obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.CodigoProducto))
            {
                string codigoProductoGenerado = string.Empty;

                resultado = new CN_Producto().Registrar(
                    obj,
                    out mensaje,
                    out codigoProductoGenerado
                );

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    codigoProducto = codigoProductoGenerado
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultado = new CN_Producto().Editar(obj, out mensaje);

                return Json(new
                {
                    resultado = resultado,
                    mensaje = mensaje,
                    codigoProducto = obj.CodigoProducto
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AjustarStockProducto(string codigoProducto, decimal cantidad, string tipoMovimiento)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_Producto().AjustarStock(
                codigoProducto,
                cantidad,
                tipoMovimiento,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InactivarProducto(string codigoProducto)
        {
            string mensaje = string.Empty;

            bool resultado = new CN_Producto().Inactivar(
                codigoProducto,
                out mensaje
            );

            return Json(new
            {
                resultado = resultado,
                mensaje = mensaje
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImportarProductosCsv(HttpPostedFileBase archivoCsv)
        {
            int registrados = 0;
            int actualizados = 0;
            int errores = 0;
            List<string> mensajes = new List<string>();

            try
            {
                if (archivoCsv == null || archivoCsv.ContentLength == 0)
                {
                    return Json(new
                    {
                        resultado = false,
                        mensaje = "Debe seleccionar un archivo CSV."
                    });
                }

                string extension = Path.GetExtension(archivoCsv.FileName).ToLower();

                if (extension != ".csv")
                {
                    return Json(new
                    {
                        resultado = false,
                        mensaje = "El archivo debe tener extensión .csv."
                    });
                }

                var tiposProducto = new CN_TipoProducto().Listar()
                    .Where(x => x.Activo)
                    .ToList();

                var impuestos = new CN_Impuesto().ListarActivos();

                var productosExistentes = new CN_Producto().Listar();

                using (var reader = new StreamReader(archivoCsv.InputStream, Encoding.UTF8, true))
                {
                    int linea = 0;

                    while (!reader.EndOfStream)
                    {
                        string fila = reader.ReadLine();
                        linea++;

                        if (string.IsNullOrWhiteSpace(fila))
                        {
                            continue;
                        }

                        string[] columnas = SepararCsv(fila);

                        if (linea == 1)
                        {
                            string encabezado = string.Join(",", columnas)
                                .Replace("\uFEFF", "")
                                .Replace(" ", "")
                                .Trim()
                                .ToLower();

                            if (encabezado != "codigoproducto,nombreproducto,idtipoproducto,idimpuesto,descripcion,precioventa,stockactual,activo")
                            {
                                return Json(new
                                {
                                    resultado = false,
                                    mensaje = "El encabezado del CSV debe ser: CodigoProducto,NombreProducto,IdTipoProducto,IdImpuesto,Descripcion,PrecioVenta,StockActual,Activo"
                                });
                            }

                            continue;
                        }

                        if (columnas.Length < 8)
                        {
                            errores++;
                            mensajes.Add("Línea " + linea + ": la fila no tiene todas las columnas requeridas.");
                            continue;
                        }

                        string codigoProducto = columnas[0].Trim().Replace("\uFEFF", "");
                        string nombreProducto = columnas[1].Trim();
                        string tipoProductoTexto = columnas[2].Trim();
                        string impuestoTexto = columnas[3].Trim();
                        string descripcion = columnas[4].Trim();
                        string precioTexto = columnas[5].Trim();
                        string stockTexto = columnas[6].Trim();
                        string activoTexto = columnas[7].Trim().ToLower();

                        if (string.IsNullOrWhiteSpace(nombreProducto))
                        {
                            errores++;
                            mensajes.Add("Línea " + linea + ": el nombre del producto es obligatorio.");
                            continue;
                        }

                        int idTipoProducto;
                        int idImpuesto;

                        if (!int.TryParse(tipoProductoTexto, out idTipoProducto) || idTipoProducto <= 0)
                        {
                            errores++;
                            mensajes.Add("Línea " + linea + ": IdTipoProducto inválido.");
                            continue;
                        }

                        if (!tiposProducto.Any(x => x.IdTipoProducto == idTipoProducto))
                        {
                            errores++;
                            mensajes.Add("Línea " + linea + ": IdTipoProducto no existe o está inactivo.");
                            continue;
                        }

                        if (!int.TryParse(impuestoTexto, out idImpuesto) || idImpuesto <= 0)
                        {
                            errores++;
                            mensajes.Add("Línea " + linea + ": IdImpuesto inválido.");
                            continue;
                        }

                        if (!impuestos.Any(x => x.IdImpuesto == idImpuesto))
                        {
                            errores++;
                            mensajes.Add("Línea " + linea + ": IdImpuesto no existe o está inactivo.");
                            continue;
                        }

                        decimal precioVenta;
                        decimal stockActual;

                        precioTexto = precioTexto.Replace(",", ".");
                        stockTexto = stockTexto.Replace(",", ".");

                        if (!decimal.TryParse(precioTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out precioVenta) || precioVenta < 0)
                        {
                            errores++;
                            mensajes.Add("Línea " + linea + ": el precio de venta no es válido.");
                            continue;
                        }

                        if (!decimal.TryParse(stockTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out stockActual) || stockActual < 0)
                        {
                            errores++;
                            mensajes.Add("Línea " + linea + ": el stock actual no es válido.");
                            continue;
                        }

                        bool activo;

                        if (activoTexto == "true" || activoTexto == "1" || activoTexto == "si" || activoTexto == "sí" || activoTexto == "activo")
                        {
                            activo = true;
                        }
                        else if (activoTexto == "false" || activoTexto == "0" || activoTexto == "no" || activoTexto == "inactivo")
                        {
                            activo = false;
                        }
                        else
                        {
                            errores++;
                            mensajes.Add("Línea " + linea + ": Activo inválido. Use true/false, 1/0, sí/no o activo/inactivo.");
                            continue;
                        }

                        Producto obj = new Producto()
                        {
                            CodigoProducto = codigoProducto,
                            NombreProducto = nombreProducto,
                            IdTipoProducto = idTipoProducto,
                            IdImpuesto = idImpuesto,
                            Descripcion = descripcion,
                            PrecioVenta = precioVenta,
                            StockActual = stockActual,
                            Activo = activo
                        };

                        string mensajeRegistro;

                        if (string.IsNullOrWhiteSpace(codigoProducto))
                        {
                            string codigoProductoGenerado;

                            int resultado = new CN_Producto().Registrar(
                                obj,
                                out mensajeRegistro,
                                out codigoProductoGenerado
                            );

                            if (resultado > 0)
                            {
                                registrados++;
                            }
                            else
                            {
                                errores++;
                                mensajes.Add("Línea " + linea + ": " + mensajeRegistro);
                            }
                        }
                        else
                        {
                            bool existeProducto = productosExistentes
                                .Any(p => p.CodigoProducto.Equals(codigoProducto, StringComparison.OrdinalIgnoreCase));

                            if (!existeProducto)
                            {
                                errores++;
                                mensajes.Add("Línea " + linea + ": el producto con código '" + codigoProducto + "' no existe. Para crear un producto nuevo deje CodigoProducto vacío.");
                                continue;
                            }

                            bool resultado = new CN_Producto().Editar(obj, out mensajeRegistro);

                            if (resultado)
                            {
                                actualizados++;
                            }
                            else
                            {
                                errores++;
                                mensajes.Add("Línea " + linea + ": " + mensajeRegistro);
                            }
                        }
                    }
                }

                return Json(new
                {
                    resultado = true,
                    mensaje = "Importación finalizada. Registrados: " + registrados + ". Actualizados: " + actualizados + ". Errores: " + errores + ".",
                    registrados = registrados,
                    actualizados = actualizados,
                    errores = errores,
                    detalle = mensajes.Take(30).ToList()
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    resultado = false,
                    mensaje = "Error al importar productos: " + ex.Message
                });
            }
        }
        private string[] SepararCsv(string fila)
        {
            List<string> columnas = new List<string>();
            bool dentroComillas = false;
            StringBuilder valor = new StringBuilder();

            for (int i = 0; i < fila.Length; i++)
            {
                char c = fila[i];

                if (c == '"')
                {
                    dentroComillas = !dentroComillas;
                }
                else if (c == ',' && !dentroComillas)
                {
                    columnas.Add(valor.ToString());
                    valor.Clear();
                }
                else
                {
                    valor.Append(c);
                }
            }

            columnas.Add(valor.ToString());

            return columnas.ToArray();
        }
    }
}