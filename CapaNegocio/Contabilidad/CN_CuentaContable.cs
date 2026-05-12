using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System.Collections.Generic;

namespace CapaNegocio.Contabilidad
{
    public class CN_CuentaContable
    {
        private CD_CuentaContable objCapaDato = new CD_CuentaContable();

        public List<CuentaContable> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<CuentaContable> ListarActivas()
        {
            return objCapaDato.ListarActivas();
        }

        public int Registrar(CuentaContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.CodigoCuenta))
            {
                Mensaje = "El código de cuenta es obligatorio.";
                return 0;
            }

            if (obj.CodigoCuenta.Length > 45)
            {
                Mensaje = "El código de cuenta no puede superar los 45 caracteres.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.NombreCuenta))
            {
                Mensaje = "El nombre de la cuenta es obligatorio.";
                return 0;
            }

            if (obj.NombreCuenta.Length > 100)
            {
                Mensaje = "El nombre de la cuenta no puede superar los 100 caracteres.";
                return 0;
            }

            if (obj.IdTipoCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar un tipo de cuenta contable.";
                return 0;
            }

            if (obj.IdNaturalezaCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una naturaleza de cuenta contable.";
                return 0;
            }

            if (obj.IdCuentaPadre.HasValue && obj.IdCuentaPadre.Value < 0)
            {
                Mensaje = "Debe seleccionar una cuenta padre válida.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(CuentaContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una cuenta contable válida.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.CodigoCuenta))
            {
                Mensaje = "El código de cuenta es obligatorio.";
                return false;
            }

            if (obj.CodigoCuenta.Length > 45)
            {
                Mensaje = "El código de cuenta no puede superar los 45 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.NombreCuenta))
            {
                Mensaje = "El nombre de la cuenta es obligatorio.";
                return false;
            }

            if (obj.NombreCuenta.Length > 100)
            {
                Mensaje = "El nombre de la cuenta no puede superar los 100 caracteres.";
                return false;
            }

            if (obj.IdTipoCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar un tipo de cuenta contable.";
                return false;
            }

            if (obj.IdNaturalezaCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una naturaleza de cuenta contable.";
                return false;
            }

            if (obj.IdCuentaPadre.HasValue && obj.IdCuentaPadre.Value < 0)
            {
                Mensaje = "Debe seleccionar una cuenta padre válida.";
                return false;
            }

            if (obj.IdCuentaPadre.HasValue && obj.IdCuentaPadre.Value == obj.IdCuentaContable)
            {
                Mensaje = "Una cuenta no puede ser su propia cuenta padre.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int idCuentaContable, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una cuenta contable válida.";
                return false;
            }

            return objCapaDato.Inactivar(idCuentaContable, out Mensaje);
        }

        public string GenerarCodigo(int idCuentaPadre, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (idCuentaPadre == 0)
            {
                Mensaje = "Debe seleccionar una cuenta padre.";
                return string.Empty;
            }

            return objCapaDato.GenerarCodigo(idCuentaPadre, out Mensaje);
        }
    }
}