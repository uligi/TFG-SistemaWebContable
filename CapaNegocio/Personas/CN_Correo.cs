using CapaDatos.personas;
using CapaEntidad.personas;
using CapaEntidad.Personas;
using System.Collections.Generic;

namespace CapaNegocio.personas
{
    public class CN_Correo
    {
        private CD_Correo objCapaDato = new CD_Correo();

        public List<Correo> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Correo obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Identificacion))
            {
                Mensaje = "Debe seleccionar una persona.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.DireccionCorreo))
            {
                Mensaje = "La dirección de correo es obligatoria.";
                return 0;
            }

            if (obj.DireccionCorreo.Length > 100)
            {
                Mensaje = "La dirección de correo no puede superar los 100 caracteres.";
                return 0;
            }

            if (obj.IdTipoCorreo == 0)
            {
                Mensaje = "Debe seleccionar un tipo de correo.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool EditarDesdeCliente(string correoAnterior, Correo obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(correoAnterior))
            {
                Mensaje = "Debe seleccionar un correo válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Identificacion))
            {
                Mensaje = "Debe seleccionar una persona válida.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.DireccionCorreo))
            {
                Mensaje = "La dirección de correo es obligatoria.";
                return false;
            }

            if (obj.DireccionCorreo.Length > 100)
            {
                Mensaje = "La dirección de correo no puede superar los 100 caracteres.";
                return false;
            }

            if (obj.IdTipoCorreo == 0)
            {
                Mensaje = "Debe seleccionar un tipo de correo.";
                return false;
            }

            return objCapaDato.EditarDesdeCliente(correoAnterior, obj, out Mensaje);
        }

        public bool Inactivar(string identificacion, string direccionCorreo, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(identificacion))
            {
                Mensaje = "Debe seleccionar una persona válida.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(direccionCorreo))
            {
                Mensaje = "Debe seleccionar un correo válido.";
                return false;
            }

            return objCapaDato.Inactivar(identificacion, direccionCorreo, out Mensaje);
        }
    }
}