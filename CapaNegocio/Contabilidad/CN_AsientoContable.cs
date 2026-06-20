using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Contabilidad
{
    public class CN_AsientoContable
    {
        private CD_AsientoContable objCapaDato = new CD_AsientoContable();

        public List<AsientoContable> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<AsientoContable> ListarPorPeriodo(int anio, int mes)
        {
            if (anio < 2000 || anio > 2100)
            {
                return new List<AsientoContable>();
            }

            if (mes < 1 || mes > 12)
            {
                return new List<AsientoContable>();
            }

            return objCapaDato.ListarPorPeriodo(anio, mes);
        }

        public AsientoContable Obtener(string numeroAsiento)
        {
            numeroAsiento = numeroAsiento == null ? "" : numeroAsiento.Trim();

            if (numeroAsiento == "")
            {
                return null;
            }

            if (numeroAsiento.Length > 45)
            {
                return null;
            }

            return objCapaDato.Obtener(numeroAsiento);
        }

        public int Registrar(AsientoContable obj, out string Mensaje, out string NumeroAsientoGenerado)
        {
            Mensaje = string.Empty;
            NumeroAsientoGenerado = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje, out NumeroAsientoGenerado);
        }

        public bool Editar(AsientoContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, true);

            if (error != "")
            {
                Mensaje = error;
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool ValidarCuadre(string numeroAsiento, out string Mensaje)
        {
            Mensaje = string.Empty;

            numeroAsiento = numeroAsiento == null ? "" : numeroAsiento.Trim();

            if (numeroAsiento == "")
            {
                Mensaje = "Debe seleccionar un asiento contable válido.";
                return false;
            }

            if (numeroAsiento.Length > 45)
            {
                Mensaje = "El número de asiento no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.ValidarCuadre(numeroAsiento, out Mensaje);
        }

        public bool RecalcularTotales(string numeroAsiento, out string Mensaje)
        {
            Mensaje = string.Empty;

            numeroAsiento = numeroAsiento == null ? "" : numeroAsiento.Trim();

            if (numeroAsiento == "")
            {
                Mensaje = "Debe seleccionar un asiento contable válido.";
                return false;
            }

            if (numeroAsiento.Length > 45)
            {
                Mensaje = "El número de asiento no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.RecalcularTotales(numeroAsiento, out Mensaje);
        }

        public bool Inactivar(string numeroAsiento, out string Mensaje)
        {
            Mensaje = string.Empty;

            numeroAsiento = numeroAsiento == null ? "" : numeroAsiento.Trim();

            if (numeroAsiento == "")
            {
                Mensaje = "Debe seleccionar un asiento contable válido.";
                return false;
            }

            if (numeroAsiento.Length > 45)
            {
                Mensaje = "El número de asiento no puede superar los 45 caracteres.";
                return false;
            }

            return objCapaDato.Inactivar(numeroAsiento, out Mensaje);
        }

        private void PrepararDatos(AsientoContable obj)
        {
            obj.NumeroAsiento = obj.NumeroAsiento == null ? "" : obj.NumeroAsiento.Trim();
            obj.TipoAsiento = obj.TipoAsiento == null ? "" : obj.TipoAsiento.Trim();
            obj.Concepto = obj.Concepto == null ? "" : obj.Concepto.Trim();
            obj.EstadoPeriodo = obj.EstadoPeriodo == null ? "" : obj.EstadoPeriodo.Trim();

            if (obj.FechaAsiento == DateTime.MinValue)
            {
                obj.FechaAsiento = DateTime.Now;
            }
        }

        private string Validar(AsientoContable obj, bool esEdicion)
        {
            if (esEdicion)
            {
                if (string.IsNullOrWhiteSpace(obj.NumeroAsiento))
                {
                    return "Debe seleccionar un asiento contable válido.";
                }

                if (obj.NumeroAsiento.Length > 45)
                {
                    return "El número de asiento no puede superar los 45 caracteres.";
                }

                if (!Regex.IsMatch(obj.NumeroAsiento, @"^[0-9A-Za-z.\-_]+$"))
                {
                    return "El número de asiento solo puede contener letras, números, puntos, guiones y guion bajo.";
                }
            }

            if (obj.Anio < 2000 || obj.Anio > 2100)
            {
                return "Debe seleccionar un año válido.";
            }

            if (obj.Mes < 1 || obj.Mes > 12)
            {
                return "Debe seleccionar un mes válido.";
            }

            if (obj.FechaAsiento == DateTime.MinValue)
            {
                return "Debe ingresar la fecha del asiento.";
            }

            if (string.IsNullOrWhiteSpace(obj.TipoAsiento))
            {
                return "Debe ingresar el tipo de asiento.";
            }

            if (obj.TipoAsiento.Length > 45)
            {
                return "El tipo de asiento no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.TipoAsiento, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "El tipo de asiento solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (string.IsNullOrWhiteSpace(obj.Concepto))
            {
                return "Debe ingresar el concepto del asiento.";
            }

            if (obj.Concepto.Length > 150)
            {
                return "El concepto no puede superar los 150 caracteres.";
            }

            if (!Regex.IsMatch(obj.Concepto, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "El concepto solo puede contener letras, números, espacios y caracteres básicos.";
            }

            if (obj.TotalDebe < 0)
            {
                return "El total debe no puede ser negativo.";
            }

            if (obj.TotalHaber < 0)
            {
                return "El total haber no puede ser negativo.";
            }

            return "";
        }
    }
}