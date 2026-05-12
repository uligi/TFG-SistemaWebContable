using CapaDatos.Ubicacion;
using CapaEntidad.Ubicacion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualBasic.FileIO;


namespace CapaNegocio.Ubicacion
{
    public class CN_Provincia
    {
        private CD_Provincia objCapaDato = new CD_Provincia();

        public List<Provincia> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<Provincia> ListarActivas()
        {
            return objCapaDato.ListarActivas();
        }

        public int Registrar(Provincia obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre de la provincia es obligatorio.";
                return 0;
            }

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre de la provincia no puede superar los 45 caracteres.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Provincia obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdProvincia == 0)
            {
                Mensaje = "Debe seleccionar una provincia válida.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre de la provincia es obligatorio.";
                return false;
            }

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre de la provincia no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int id, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (id == 0)
            {
                Mensaje = "Debe seleccionar una provincia válida.";
                return false;
            }

            return objCapaDato.Inactivar(id, out Mensaje);
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

                        if (campos == null || campos.Length < 1)
                        {
                            errores++;
                            detalleErrores.Add("Línea " + numeroLinea + ": fila vacía o formato inválido.");
                            continue;
                        }

                        string nombre = campos[0].Trim();

                        Provincia obj = new Provincia()
                        {
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