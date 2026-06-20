using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Contabilidad
{
    public class CN_DetalleAsientoContable
    {
        private CD_DetalleAsientoContable objCapaDato = new CD_DetalleAsientoContable();

        public List<DetalleAsientoContable> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<DetalleAsientoContable> ListarPorAsiento(string numeroAsiento)
        {
            numeroAsiento = numeroAsiento == null ? "" : numeroAsiento.Trim();

            if (numeroAsiento == "")
            {
                return new List<DetalleAsientoContable>();
            }

            if (numeroAsiento.Length > 45)
            {
                return new List<DetalleAsientoContable>();
            }

            return objCapaDato.ListarPorAsiento(numeroAsiento);
        }

        public DetalleAsientoContable Obtener(string numeroAsiento, int numeroLinea)
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

            if (numeroLinea <= 0)
            {
                return null;
            }

            return objCapaDato.Obtener(numeroAsiento, numeroLinea);
        }

        public int Registrar(DetalleAsientoContable obj, out string Mensaje, out int NumeroLineaGenerada)
        {
            Mensaje = string.Empty;
            NumeroLineaGenerada = 0;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje, out NumeroLineaGenerada);
        }

        public bool Editar(DetalleAsientoContable obj, out string Mensaje)
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

        public bool Inactivar(string numeroAsiento, int numeroLinea, out string Mensaje)
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

            if (numeroLinea <= 0)
            {
                Mensaje = "Debe seleccionar una línea válida.";
                return false;
            }

            return objCapaDato.Inactivar(numeroAsiento, numeroLinea, out Mensaje);
        }

        public bool InactivarPorAsiento(string numeroAsiento, out string Mensaje)
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

            return objCapaDato.InactivarPorAsiento(numeroAsiento, out Mensaje);
        }

        private void PrepararDatos(DetalleAsientoContable obj)
        {
            obj.NumeroAsiento = obj.NumeroAsiento == null ? "" : obj.NumeroAsiento.Trim();
            obj.CodigoCuenta = obj.CodigoCuenta == null ? "" : obj.CodigoCuenta.Trim();
            obj.NombreCuenta = obj.NombreCuenta == null ? "" : obj.NombreCuenta.Trim();
            obj.TipoCuentaContableNombre = obj.TipoCuentaContableNombre == null ? "" : obj.TipoCuentaContableNombre.Trim();
            obj.NaturalezaCuentaContableNombre = obj.NaturalezaCuentaContableNombre == null ? "" : obj.NaturalezaCuentaContableNombre.Trim();
            obj.DescripcionLinea = obj.DescripcionLinea == null ? "" : obj.DescripcionLinea.Trim();

            if (obj.Debe < 0)
            {
                obj.Debe = 0;
            }

            if (obj.Haber < 0)
            {
                obj.Haber = 0;
            }
        }

        private string Validar(DetalleAsientoContable obj, bool esEdicion)
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

            if (esEdicion)
            {
                if (obj.NumeroLinea <= 0)
                {
                    return "Debe seleccionar una línea válida.";
                }
            }

            if (string.IsNullOrWhiteSpace(obj.CodigoCuenta))
            {
                return "Debe seleccionar una cuenta contable.";
            }

            if (obj.CodigoCuenta.Length > 45)
            {
                return "El código de cuenta no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(obj.CodigoCuenta, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "El código de cuenta solo puede contener letras, números, puntos, guiones y guion bajo.";
            }

            if (obj.CodigoCuenta == "0")
            {
                return "No se puede usar la cuenta raíz en un asiento contable.";
            }

            if (obj.Debe < 0 || obj.Haber < 0)
            {
                return "Los montos de debe y haber no pueden ser negativos.";
            }

            if (obj.Debe == 0 && obj.Haber == 0)
            {
                return "Debe ingresar un monto en debe o en haber.";
            }

            if (obj.Debe > 0 && obj.Haber > 0)
            {
                return "No puede ingresar monto en debe y haber al mismo tiempo.";
            }

            if (obj.DescripcionLinea.Length > 150)
            {
                return "La descripción de la línea no puede superar los 150 caracteres.";
            }

            if (obj.DescripcionLinea != "" &&
                !Regex.IsMatch(obj.DescripcionLinea, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9\s.,#\-_/&()]+$"))
            {
                return "La descripción de la línea solo puede contener letras, números, espacios y caracteres básicos.";
            }

            return "";
        }
    }
}