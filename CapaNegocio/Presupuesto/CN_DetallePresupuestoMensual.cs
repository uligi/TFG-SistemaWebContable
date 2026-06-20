using CapaDatos.Presupuesto;
using CapaEntidad.Presupuesto;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.Presupuesto
{
    public class CN_DetallePresupuestoMensual
    {
        private CD_DetallePresupuestoMensual objCapaDato = new CD_DetallePresupuestoMensual();

        public List<DetallePresupuestoMensual> ListarPorPresupuesto(int anio, int mes)
        {
            if (anio < 2000 || anio > 2100)
            {
                return new List<DetallePresupuestoMensual>();
            }

            if (mes < 1 || mes > 12)
            {
                return new List<DetallePresupuestoMensual>();
            }

            return objCapaDato.ListarPorPresupuesto(anio, mes);
        }

        public DetallePresupuestoMensual Obtener(int anio, int mes, string codigoCuenta, string tipoMovimiento)
        {
            codigoCuenta = codigoCuenta == null ? "" : codigoCuenta.Trim();
            tipoMovimiento = tipoMovimiento == null ? "" : tipoMovimiento.Trim();

            if (anio < 2000 || anio > 2100)
            {
                return null;
            }

            if (mes < 1 || mes > 12)
            {
                return null;
            }

            if (codigoCuenta == "" || tipoMovimiento == "")
            {
                return null;
            }

            if (codigoCuenta.Length > 45 || tipoMovimiento.Length > 45)
            {
                return null;
            }

            return objCapaDato.Obtener(anio, mes, codigoCuenta, tipoMovimiento);
        }

        public int Registrar(DetallePresupuestoMensual obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(DetallePresupuestoMensual obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj);

            if (error != "")
            {
                Mensaje = error;
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int anio, int mes, string codigoCuenta, string tipoMovimiento, out string Mensaje)
        {
            Mensaje = string.Empty;

            codigoCuenta = codigoCuenta == null ? "" : codigoCuenta.Trim();
            tipoMovimiento = tipoMovimiento == null ? "" : tipoMovimiento.Trim();

            string error = ValidarLlave(anio, mes, codigoCuenta, tipoMovimiento);

            if (error != "")
            {
                Mensaje = error;
                return false;
            }

            return objCapaDato.Inactivar(anio, mes, codigoCuenta, tipoMovimiento, out Mensaje);
        }

        public bool Activar(int anio, int mes, string codigoCuenta, string tipoMovimiento, out string Mensaje)
        {
            Mensaje = string.Empty;

            codigoCuenta = codigoCuenta == null ? "" : codigoCuenta.Trim();
            tipoMovimiento = tipoMovimiento == null ? "" : tipoMovimiento.Trim();

            string error = ValidarLlave(anio, mes, codigoCuenta, tipoMovimiento);

            if (error != "")
            {
                Mensaje = error;
                return false;
            }

            return objCapaDato.Activar(anio, mes, codigoCuenta, tipoMovimiento, out Mensaje);
        }

        private void PrepararDatos(DetallePresupuestoMensual obj)
        {
            obj.CodigoCuenta = obj.CodigoCuenta == null ? "" : obj.CodigoCuenta.Trim();
            obj.NombreCuenta = obj.NombreCuenta == null ? "" : obj.NombreCuenta.Trim();
            obj.TipoMovimiento = obj.TipoMovimiento == null ? "" : obj.TipoMovimiento.Trim();
            obj.MesNombre = obj.MesNombre == null ? "" : obj.MesNombre.Trim();

            if (obj.MontoPresupuestado < 0)
            {
                obj.MontoPresupuestado = 0;
            }
        }

        private string Validar(DetallePresupuestoMensual obj)
        {
            if (obj.Anio < 2000 || obj.Anio > 2100)
            {
                return "Debe ingresar un año válido.";
            }

            if (obj.Mes < 1 || obj.Mes > 12)
            {
                return "Debe ingresar un mes válido.";
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

            if (string.IsNullOrWhiteSpace(obj.TipoMovimiento))
            {
                return "Debe seleccionar el tipo de movimiento.";
            }

            if (obj.TipoMovimiento != "Ingreso" && obj.TipoMovimiento != "Gasto")
            {
                return "El tipo de movimiento debe ser Ingreso o Gasto.";
            }

            if (obj.TipoMovimiento.Length > 45)
            {
                return "El tipo de movimiento no puede superar los 45 caracteres.";
            }

            if (obj.MontoPresupuestado < 0)
            {
                return "El monto presupuestado no puede ser negativo.";
            }

            return "";
        }

        private string ValidarLlave(int anio, int mes, string codigoCuenta, string tipoMovimiento)
        {
            if (anio < 2000 || anio > 2100)
            {
                return "Debe ingresar un año válido.";
            }

            if (mes < 1 || mes > 12)
            {
                return "Debe ingresar un mes válido.";
            }

            if (string.IsNullOrWhiteSpace(codigoCuenta))
            {
                return "Debe seleccionar una cuenta contable.";
            }

            if (codigoCuenta.Length > 45)
            {
                return "El código de cuenta no puede superar los 45 caracteres.";
            }

            if (!Regex.IsMatch(codigoCuenta, @"^[0-9A-Za-z.\-_]+$"))
            {
                return "El código de cuenta solo puede contener letras, números, puntos, guiones y guion bajo.";
            }

            if (string.IsNullOrWhiteSpace(tipoMovimiento))
            {
                return "Debe seleccionar el tipo de movimiento.";
            }

            if (tipoMovimiento != "Ingreso" && tipoMovimiento != "Gasto")
            {
                return "El tipo de movimiento debe ser Ingreso o Gasto.";
            }

            return "";
        }
    }
}