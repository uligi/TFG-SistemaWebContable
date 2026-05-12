using CapaDatos.Ubicacion;
using CapaEntidad.Ubicacion;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System;

using System.IO;
using System.Text;

namespace CapaNegocio.Ubicacion
{
    public class CN_Canton
    {
        private CD_Canton objCapaDato = new CD_Canton();

        public List<Canton> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Canton obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdProvincia == 0)
            {
                Mensaje = "Debe seleccionar una provincia.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del cantón es obligatorio.";
                return 0;
            }

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del cantón no puede superar los 45 caracteres.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Canton obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdCanton == 0)
            {
                Mensaje = "Debe seleccionar un cantón válido.";
                return false;
            }

            if (obj.IdProvincia == 0)
            {
                Mensaje = "Debe seleccionar una provincia.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del cantón es obligatorio.";
                return false;
            }

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del cantón no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int id, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (id == 0)
            {
                Mensaje = "Debe seleccionar un cantón válido.";
                return false;
            }

            return objCapaDato.Inactivar(id, out Mensaje);
        }

        public List<Canton> ListarActivosPorProvincia(int idProvincia)
        {
            if (idProvincia == 0)
            {
                return new List<Canton>();
            }

            return objCapaDato.ListarActivosPorProvincia(idProvincia);
        }

        public object CargarCsv(Stream archivo, out string Mensaje)
        {
            Mensaje = string.Empty;

            int insertados = 0;
            int errores = 0;
            List<string> detalleErrores = new List<string>();

            try
            {
                using (TextFieldParser parser = new TextFieldParser(archivo, Encoding.UTF8))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    parser.HasFieldsEnclosedInQuotes = true;

                    bool primeraFila = true;
                    int numeroLinea = 0;

                    while (!parser.EndOfData)
                    {
                        string[] campos = parser.ReadFields();
                        numeroLinea++;

                        if (primeraFila)
                        {
                            primeraFila = false;
                            continue;
                        }

                        if (campos == null || campos.Length < 2)
                        {
                            errores++;
                            detalleErrores.Add("Línea " + numeroLinea + ": formato inválido. Debe tener IdProvincia y Nombre.");
                            continue;
                        }

                        int idProvincia;

                        if (!int.TryParse(campos[0].Trim(), out idProvincia))
                        {
                            errores++;
                            detalleErrores.Add("Línea " + numeroLinea + ": IdProvincia inválido.");
                            continue;
                        }

                        string nombre = campos[1].Trim();

                        Canton obj = new Canton()
                        {
                            IdProvincia = idProvincia,
                            Nombre = nombre,
                            Activo = true
                        };

                        string mensajeRegistro;
                        int resultado = Registrar(obj, out mensajeRegistro);

                        if (resultado > 0)
                        {
                            insertados++;
                        }
                        else
                        {
                            errores++;
                            detalleErrores.Add("Línea " + numeroLinea + ": " + mensajeRegistro);
                        }
                    }
                }

                Mensaje = "Carga finalizada.";
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }

            return new
            {
                insertados = insertados,
                errores = errores,
                detalleErrores = detalleErrores
            };
        }
    }
}