using CapaDatos.personas;
using CapaEntidad.Personas;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CapaNegocio.personas
{
    public class CN_Telefono
    {
        private CD_Telefono objCapaDato = new CD_Telefono();

        public List<Telefono> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<Telefono> ListarPorPersona(string identificacion)
        {
            identificacion = identificacion == null ? "" : identificacion.Trim();

            if (identificacion == "")
            {
                return new List<Telefono>();
            }

            return objCapaDato.ListarPorPersona(identificacion);
        }

        public int Registrar(Telefono obj, out string Mensaje)
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

        public bool Editar(Telefono obj, out string Mensaje)
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

        public bool Inactivar(string identificacion, string numeroTelefono, out string Mensaje)
        {
            Mensaje = string.Empty;

            identificacion = identificacion == null ? "" : identificacion.Trim();
            numeroTelefono = numeroTelefono == null ? "" : numeroTelefono.Trim();

            if (identificacion == "")
            {
                Mensaje = "Debe seleccionar una persona válida.";
                return false;
            }

            if (!Regex.IsMatch(identificacion, @"^([0-9]{9}|[0-9]{11})$"))
            {
                Mensaje = "La identificación debe contener solo números: 9 dígitos para cédula nacional o 11 dígitos para cédula extranjera.";
                return false;
            }

            if (numeroTelefono == "")
            {
                Mensaje = "Debe seleccionar un teléfono válido.";
                return false;
            }

            if (!Regex.IsMatch(numeroTelefono, @"^[0-9]{8}$"))
            {
                Mensaje = "El número de teléfono debe contener exactamente 8 dígitos numéricos, sin prefijo ni caracteres especiales.";
                return false;
            }

            return objCapaDato.Inactivar(identificacion, numeroTelefono, out Mensaje);
        }

        private void PrepararDatos(Telefono obj)
        {
            obj.Identificacion = obj.Identificacion == null ? "" : obj.Identificacion.Trim();
            obj.NumeroTelefono = obj.NumeroTelefono == null ? "" : obj.NumeroTelefono.Trim();
            obj.NumeroTelefonoAnterior = obj.NumeroTelefonoAnterior == null ? "" : obj.NumeroTelefonoAnterior.Trim();
            obj.TipoTelefonoNombre = obj.TipoTelefonoNombre == null ? "" : obj.TipoTelefonoNombre.Trim();

            obj.Nombre = obj.Nombre == null ? "" : obj.Nombre.Trim();
            obj.PrimerApellido = obj.PrimerApellido == null ? "" : obj.PrimerApellido.Trim();
            obj.SegundoApellido = obj.SegundoApellido == null ? "" : obj.SegundoApellido.Trim();
        }

        private string Validar(Telefono obj, bool esEdicion)
        {
            if (string.IsNullOrWhiteSpace(obj.Identificacion))
            {
                return "Debe seleccionar una persona.";
            }

            if (!Regex.IsMatch(obj.Identificacion, @"^([0-9]{9}|[0-9]{11})$"))
            {
                return "La identificación debe contener solo números: 9 dígitos para cédula nacional o 11 dígitos para cédula extranjera.";
            }

            if (esEdicion)
            {
                if (string.IsNullOrWhiteSpace(obj.NumeroTelefonoAnterior))
                {
                    return "Debe seleccionar un teléfono válido.";
                }

                if (!Regex.IsMatch(obj.NumeroTelefonoAnterior, @"^[0-9]{8}$"))
                {
                    return "El teléfono anterior debe contener exactamente 8 dígitos numéricos.";
                }
            }

            if (string.IsNullOrWhiteSpace(obj.NumeroTelefono))
            {
                return "El número de teléfono es obligatorio.";
            }

            if (!Regex.IsMatch(obj.NumeroTelefono, @"^[0-9]{8}$"))
            {
                return "El número de teléfono debe contener exactamente 8 dígitos numéricos, sin prefijo ni caracteres especiales.";
            }

            if (obj.IdTipoTelefono <= 0)
            {
                return "Debe seleccionar un tipo de teléfono.";
            }

            return "";
        }
    }
}