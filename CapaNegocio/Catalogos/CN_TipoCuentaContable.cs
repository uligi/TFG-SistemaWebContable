using CapaDatos.Catalogos;
using CapaEntidad.Catalogos;
using System.Collections.Generic;

namespace CapaNegocio.Catalogos
{
    public class CN_TipoCuentaContable
    {
        private CD_TipoCuentaContable objCapaDato = new CD_TipoCuentaContable();

        public List<TipoCuentaContable> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<TipoCuentaContable> ListarActivos()
        {
            return objCapaDato.ListarActivos();
        }

        public int Registrar(TipoCuentaContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de cuenta contable es obligatorio.";
                return 0;
            }

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del tipo de cuenta contable no puede superar los 45 caracteres.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción es obligatoria.";
                return 0;
            }

            if (obj.Descripcion.Length > 255)
            {
                Mensaje = "La descripción no puede superar los 255 caracteres.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(TipoCuentaContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdTipoCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar un tipo de cuenta contable válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del tipo de cuenta contable es obligatorio.";
                return false;
            }

            if (obj.Nombre.Length > 45)
            {
                Mensaje = "El nombre del tipo de cuenta contable no puede superar los 45 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción es obligatoria.";
                return false;
            }

            if (obj.Descripcion.Length > 255)
            {
                Mensaje = "La descripción no puede superar los 255 caracteres.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(int id, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (id == 0)
            {
                Mensaje = "Debe seleccionar un tipo de cuenta contable válido.";
                return false;
            }

            return objCapaDato.Inactivar(id, out Mensaje);
        }
    }
}