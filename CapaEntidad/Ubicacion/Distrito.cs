using CapaDatos.Ubicacion;
using CapaEntidad.Ubicacion;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CapaNegocio.Ubicacion
{
    public class CN_Distrito
    {
        private CD_Distrito objCapaDato = new CD_Distrito();

        public List<Distrito> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Distrito obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.CodigoDistrito <= 0)
            {
                Mensaje = "El código de distrito debe ser mayor a cero.";
                return 0;
            }

            if (obj.CodigoCanton <= 0)
            {
                Mensaje = "Debe seleccionar un cantón.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del distrito es obligatorio.";
                return 0;
            }

            obj.Nombre = obj.Nombre.Trim();

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del distrito no puede superar los 45 caracteres.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Distrito obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.CodigoDistrito <= 0)
            {
                Mensaje = "Debe seleccionar un distrito válido.";
                return false;
            }

            if (obj.CodigoCanton <= 0)
            {
                Mensaje = "Debe seleccionar un cantón.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del distrito es obligatorio.";
                return false;
            }

            obj.Nombre = obj.Nombre.Trim();

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del distrito no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int codigoDistrito, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (codigoDistrito <= 0)
            {
                Mensaje = "Debe seleccionar un distrito válido.";
                return false;
            }

            return objCapaDato.Inactivar(codigoDistrito, out Mensaje);
        }

        public List<Distrito> ListarActivosPorCanton(int codigoCanton)
        {
            if (codigoCanton <= 0)
            {
                return new List<Distrito>();
            }

            return objCapaDato.ListarActivosPorCanton(codigoCanton);
        }

        public object CargarCsv(Stream archivo, out string Mensaje)
        {
            Mensaje = string.Empty;

            int insertados = 0;
            int actualizados = 0;
            int errores = 0;
            List<string> detalleErrores = new List<string>();

            try
            {
                List<Distrito> distritosExistentes = Listar();

                using (TextFieldParser parser = new TextFieldParser(archivo, Encoding.UTF8))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    parser.HasFieldsEnclosedInQuotes = true;
                    parser.TrimWhiteSpace = true;

                    bool primeraFila = true;
                    bool tieneColumnaActivo = false;
                    int numeroLinea = 0;

                    while (!parser.EndOfData)
                    {
                        string[] campos = parser.ReadFields();
                        numeroLinea++;

                        if (campos == null || campos.Length == 0 || campos.All(c => string.IsNullOrWhiteSpace(c)))
                        {
                            continue;
                        }

                        if (primeraFila)
                        {
                            primeraFila = false;

                            string encabezado = string.Join(",", campos)
                                .Replace("\uFEFF", "")
                                .Replace(" ", "")
                                .Trim()
                                .ToLower();

                            if (encabezado == "codigodistrito,nombre,codigocanton")
                            {
                                tieneColumnaActivo = false;
                                continue;
                            }

                            if (encabezado == "codigodistrito,nombre,codigocanton,activo")
                            {
                                tieneColumnaActivo = true;
                                continue;
                            }

                            errores++;
                            detalleErrores.Add("Línea " + numeroLinea + ": encabezado inválido. Debe ser CodigoDistrito,Nombre,CodigoCanton o CodigoDistrito,Nombre,CodigoCanton,Activo.");
                            continue;
                        }

                        if (campos.Length < 3)
                        {
                            errores++;
                            detalleErrores.Add("Línea " + numeroLinea + ": formato inválido. Debe tener CodigoDistrito, Nombre y CodigoCanton.");
                            continue;
                        }

                        int codigoDistrito;
                        int codigoCanton;

                        string textoCodigoDistrito = (campos[0] ?? string.Empty).Replace("\uFEFF", "").Trim();
                        string nombre = (campos[1] ?? string.Empty).Trim();
                        string textoCodigoCanton = (campos[2] ?? string.Empty).Trim();

                        if (!int.TryParse(textoCodigoDistrito, out codigoDistrito) || codigoDistrito <= 0)
                        {
                            errores++;
                            detalleErrores.Add("Línea " + numeroLinea + ": CodigoDistrito inválido.");
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(nombre))
                        {
                            errores++;
                            detalleErrores.Add("Línea " + numeroLinea + ": el nombre del distrito es obligatorio.");
                            continue;
                        }

                        if (nombre.Length > 45)
                        {
                            errores++;
                            detalleErrores.Add("Línea " + numeroLinea + ": el nombre del distrito no puede superar los 45 caracteres.");
                            continue;
                        }

                        if (!int.TryParse(textoCodigoCanton, out codigoCanton) || codigoCanton <= 0)
                        {
                            errores++;
                            detalleErrores.Add("Línea " + numeroLinea + ": CodigoCanton inválido.");
                            continue;
                        }

                        Distrito distritoExistente = distritosExistentes
                            .FirstOrDefault(d => d.CodigoDistrito == codigoDistrito);

                        bool activo = distritoExistente != null ? distritoExistente.Activo : true;

                        if (tieneColumnaActivo && campos.Length >= 4)
                        {
                            string textoActivo = (campos[3] ?? string.Empty).Trim().ToLower();

                            if (textoActivo == "true" || textoActivo == "1" || textoActivo == "si" || textoActivo == "sí" || textoActivo == "activo")
                            {
                                activo = true;
                            }
                            else if (textoActivo == "false" || textoActivo == "0" || textoActivo == "no" || textoActivo == "inactivo")
                            {
                                activo = false;
                            }
                            else
                            {
                                errores++;
                                detalleErrores.Add("Línea " + numeroLinea + ": Activo inválido. Use true/false, 1/0, sí/no o activo/inactivo.");
                                continue;
                            }
                        }

                        Distrito obj = new Distrito()
                        {
                            CodigoDistrito = codigoDistrito,
                            CodigoCanton = codigoCanton,
                            Nombre = nombre,
                            Activo = activo
                        };

                        string mensajeRegistro;

                        if (distritoExistente == null)
                        {
                            int resultado = Registrar(obj, out mensajeRegistro);

                            if (resultado > 0)
                            {
                                insertados++;
                                distritosExistentes.Add(obj);
                            }
                            else
                            {
                                errores++;
                                detalleErrores.Add("Línea " + numeroLinea + ": " + mensajeRegistro);
                            }
                        }
                        else
                        {
                            bool resultado = Editar(obj, out mensajeRegistro);

                            if (resultado)
                            {
                                actualizados++;
                                distritoExistente.CodigoCanton = obj.CodigoCanton;
                                distritoExistente.Nombre = obj.Nombre;
                                distritoExistente.Activo = obj.Activo;
                            }
                            else
                            {
                                errores++;
                                detalleErrores.Add("Línea " + numeroLinea + ": " + mensajeRegistro);
                            }
                        }
                    }
                }

                Mensaje = "Carga finalizada.";
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                errores++;
                detalleErrores.Add("Error general: " + ex.Message);
            }

            return new
            {
                insertados = insertados,
                actualizados = actualizados,
                errores = errores,
                detalleErrores = detalleErrores
            };
        }
    }
}