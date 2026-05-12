using CapaDatos.personas;
using CapaEntidad.personas;
using System.Collections.Generic;

namespace CapaNegocio.personas
{
    public class CN_Telefono
    {
        private CD_Telefono objCapaDato = new CD_Telefono();

        public List<Telefono> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Telefono obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.NumeroTelefono))
            {
                Mensaje = "El número de teléfono es obligatorio.";
                return 0;
            }

            if (obj.NumeroTelefono.Length > 45)
            {
                Mensaje = "El número de teléfono no puede superar los 45 caracteres.";
                return 0;
            }

            if (string.IsNullOrWhiteSpace(obj.Identificacion))
            {
                Mensaje = "Debe seleccionar una persona.";
                return 0;
            }

            if (obj.IdTipoTelefono == 0)
            {
                Mensaje = "Debe seleccionar un tipo de teléfono.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Editar(Telefono obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.NumeroTelefono))
            {
                Mensaje = "Debe seleccionar un teléfono válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Identificacion))
            {
                Mensaje = "Debe seleccionar una persona válida.";
                return false;
            }

            if (obj.IdTipoTelefono == 0)
            {
                Mensaje = "Debe seleccionar un tipo de teléfono.";
                return false;
            }

            return objCapaDato.Editar(obj, out Mensaje);
        }

        public bool Inactivar(string numeroTelefono, string identificacion, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(numeroTelefono))
            {
                Mensaje = "Debe seleccionar un teléfono válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(identificacion))
            {
                Mensaje = "Debe seleccionar una persona válida.";
                return false;
            }

            return objCapaDato.Inactivar(numeroTelefono, identificacion, out Mensaje);
        }

        public bool EditarDesdeCliente(string numeroAnterior, Telefono obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(numeroAnterior))
            {
                Mensaje = "Debe seleccionar un teléfono válido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.NumeroTelefono))
            {
                Mensaje = "El número de teléfono es obligatorio.";
                return false;
            }

            if (obj.NumeroTelefono.Length > 45)
            {
                Mensaje = "El número de teléfono no puede superar los 45 caracteres.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(obj.Identificacion))
            {
                Mensaje = "Debe seleccionar una persona válida.";
                return false;
            }

            if (obj.IdTipoTelefono == 0)
            {
                Mensaje = "Debe seleccionar un tipo de teléfono.";
                return false;
            }

            return objCapaDato.EditarDesdeCliente(numeroAnterior, obj, out Mensaje);
        }
    }
}