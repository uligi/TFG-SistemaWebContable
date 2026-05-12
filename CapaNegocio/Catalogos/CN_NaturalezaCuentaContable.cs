using CapaDatos.Catalogos;
using CapaEntidad.Catalogos;
using System.Collections.Generic;

namespace CapaNegocio.Catalogos
{
    public class CN_NaturalezaCuentaContable
    {
        private CD_NaturalezaCuentaContable objCapaDato = new CD_NaturalezaCuentaContable();

        public List<NaturalezaCuentaContable> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<NaturalezaCuentaContable> ListarActivos()
        {
            return objCapaDato.ListarActivos();
        }

        public int Registrar(NaturalezaCuentaContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Naturaleza))
            {
                Mensaje = "La naturaleza es obligatoria.";
                return 0;
            }

            if (obj.Naturaleza.Length > 45)
            {
                Mensaje = "La naturaleza no puede superar los 45 caracteres.";
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

        public bool Editar(NaturalezaCuentaContable obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.IdNaturalezaCuentaContable == 0)
            {
                Mensaje = "Debe seleccionar una naturaleza válida.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Naturaleza))
            {
                Mensaje = "La naturaleza es obligatoria.";
                return false;
            }

            if (obj.Naturaleza.Length > 45)
            {
                Mensaje = "La naturaleza no puede superar los 45 caracteres.";
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
                Mensaje = "Debe seleccionar una naturaleza válida.";
                return false;
            }

            return objCapaDato.Inactivar(id, out Mensaje);
        }
    }
}