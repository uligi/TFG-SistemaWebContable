using CapaDatos.Ubicacion;
using CapaEntidad.Ubicacion;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System;

using System.IO;
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

            if (obj.IdCanton == 0)
            {
                Mensaje = "Debe seleccionar un cantón.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del distrito es obligatorio.";
                return 0;
            }

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

            if (obj.IdDistrito == 0)
            {
                Mensaje = "Debe seleccionar un distrito válido.";
                return false;
            }

            if (obj.IdCanton == 0)
            {
                Mensaje = "Debe seleccionar un cantón.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del distrito es obligatorio.";
                return false;
            }

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del distrito no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int id, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (id == 0)
            {
                Mensaje = "Debe seleccionar un distrito válido.";
                return false;
            }

            return objCapaDato.Inactivar(id, out Mensaje);
        }

        public List<Distrito> ListarActivosPorCanton(int idCanton)
        {
            if (idCanton == 0)
            {
                return new List<Distrito>();
            }

            return objCapaDato.ListarActivosPorCanton(idCanton);
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
                            detalleErrores.Add("Línea " + numeroLinea + ": formato inválido. Debe tener IdCanton y Nombre.");
                            continue;
                        }

                        int idCanton;

                        if (!int.TryParse(campos[0].Trim(), out idCanton))
                        {
                            errores++;
                            detalleErrores.Add("Línea " + numeroLinea + ": IdCanton inválido.");
                            continue;
                        }

                        string nombre = campos[1].Trim();

                        Distrito obj = new Distrito()
                        {
                            IdCanton = idCanton,
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
