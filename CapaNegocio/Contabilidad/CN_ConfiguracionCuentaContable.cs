using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System.Collections.Generic;

namespace CapaNegocio.Contabilidad
{
    public class CN_ConfiguracionCuentaContable
    {
        private CD_ConfiguracionCuentaContable objCapaDato = new CD_ConfiguracionCuentaContable();

        public List<ConfiguracionCuentaContable> Listar()
        {
            return objCapaDato.Listar();
        }

        public ConfiguracionCuentaContable ObtenerPorOperacion(string codigoOperacion)
        {
            if (string.IsNullOrWhiteSpace(codigoOperacion))
            {
                return null;
            }

            return objCapaDato.ObtenerPorOperacion(codigoOperacion.Trim());
        }

        public bool Editar(ConfiguracionCuentaContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdConfiguracionCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una configuración válida.";
                return false;
            }

            if (obj.IdCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una cuenta contable.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.Descripcion) && obj.Descripcion.Length > 200)
            {
                Mensaje = "La descripción no puede superar los 200 caracteres.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                obj.Descripcion = obj.Descripcion.Trim();
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }
    }
}