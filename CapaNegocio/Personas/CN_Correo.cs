using CapaDatos.personas;
using CapaEntidad.Personas;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.personas
{
    public class CN_Correo
    {
        private CD_Correo objCapaDato = new CD_Correo();

        public List<Correo> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<Correo> ListarPorPersona(string identificacion)
        {
            identificacion = identificacion == null ? "" : identificacion.Trim();

            if (identificacion == "")
            {
                return new List<Correo>();
            }

            return objCapaDato.ListarPorPersona(identificacion);
        }

        public int Registrar(Correo obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            PrepararDatos(obj);

            string error = Validar(obj, false);

            if (error != "")
            {
                Mensaje = error;
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Correo obj, out string Mensaje)
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

        public bool Inactivar(string identificacion, string direccionCorreo, out string Mensaje)
        {
            Mensaje = string.Empty;

            identificacion = identificacion == null ? "" : identificacion.Trim();
            direccionCorreo = direccionCorreo == null ? "" : direccionCorreo.Trim().ToLower();

            if (identificacion == "")
            {
                Mensaje = "Debe seleccionar una persona válida.";
                return false;
            }

            if (!Regex.IsMatch(identificacion, @"^[0-9]{8,11}$"))
            {
                Mensaje = "La identificación debe contener solo números y tener entre 8 y 11 dígitos.";
                return false;
            }

            if (direccionCorreo == "")
            {
                Mensaje = "Debe seleccionar un correo válido.";
                return false;
            }

            if (!ValidarCorreo(direccionCorreo))
            {
                Mensaje = "Debe ingresar una dirección de correo válida.";
                return false;
            }

            return objCapaDato.Inactivar(identificacion, direccionCorreo, out Mensaje);
        }

        private void PrepararDatos(Correo obj)
        {
            obj.Identificacion = obj.Identificacion == null ? "" : obj.Identificacion.Trim();
            obj.DireccionCorreo = obj.DireccionCorreo == null ? "" : obj.DireccionCorreo.Trim().ToLower();
            obj.DireccionCorreoAnterior = obj.DireccionCorreoAnterior == null ? "" : obj.DireccionCorreoAnterior.Trim().ToLower();
            obj.TipoCorreoNombre = obj.TipoCorreoNombre == null ? "" : obj.TipoCorreoNombre.Trim();

            obj.Nombre = obj.Nombre == null ? "" : obj.Nombre.Trim();
            obj.PrimerApellido = obj.PrimerApellido == null ? "" : obj.PrimerApellido.Trim();
            obj.SegundoApellido = obj.SegundoApellido == null ? "" : obj.SegundoApellido.Trim();
        }

        private string Validar(Correo obj, bool esEdicion)
        {
            if (string.IsNullOrWhiteSpace(obj.Identificacion))
            {
                return "Debe seleccionar una persona.";
            }

            if (!Regex.IsMatch(obj.Identificacion, @"^[0-9]{8,11}$"))
            {
                return "La identificación debe contener solo números y tener entre 8 y 11 dígitos.";
            }

            if (esEdicion)
            {
                if (string.IsNullOrWhiteSpace(obj.DireccionCorreoAnterior))
                {
                    return "Debe seleccionar un correo válido.";
                }

                if (!ValidarCorreo(obj.DireccionCorreoAnterior))
                {
                    return "El correo anterior no tiene un formato válido.";
                }
            }

            if (string.IsNullOrWhiteSpace(obj.DireccionCorreo))
            {
                return "La dirección de correo es obligatoria.";
            }

            if (obj.DireccionCorreo.Length > 255)
            {
                return "La dirección de correo no puede superar los 255 caracteres.";
            }

            if (!ValidarCorreo(obj.DireccionCorreo))
            {
                return "Debe ingresar una dirección de correo válida.";
            }

            if (obj.IdTipoCorreo <= 0)
            {
                return "Debe seleccionar un tipo de correo.";
            }

            return "";
        }

        private bool ValidarCorreo(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                return false;
            }

            return Regex.IsMatch(
                correo,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
            );
        }
    }
}